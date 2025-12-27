using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;

namespace O24OpenAPI.Client.Lib.ElasticSearch;

/// <summary>
/// The elastic search manager class
/// </summary>
/// <seealso cref="IDisposable"/>
public class ElasticSearchManager<T> : IDisposable
    where T : class
{
    /// <summary>
    /// The search result class
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// The page size
        /// </summary>
        private long _pageSize;

        /// <summary>
        /// Gets the value of the current page
        /// </summary>
        public long current_page { get; }

        /// <summary>
        /// Gets the value of the total
        /// </summary>
        public long total { get; }

        /// <summary>
        /// Gets the value of the pages
        /// </summary>
        public long pages
        {
            get
            {
                long num = total / _pageSize;
                if (total % _pageSize != 0L)
                {
                    num++;
                }
                return num;
            }
        }

        /// <summary>
        /// Gets or sets the value of the data
        /// </summary>
        public List<T> data { get; set; } = new List<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class
        /// </summary>
        /// <param name="pPageSize">The page size</param>
        /// <param name="pCurrentPage">The current page</param>
        /// <param name="pTotal">The total</param>
        public SearchResult(long pPageSize, long pCurrentPage, long pTotal)
        {
            _pageSize = pPageSize;
            current_page = pCurrentPage;
            total = pTotal;
        }
    }

    /// <summary>
    /// The es aggregation query result class
    /// </summary>
    private class ESAggregationQueryResult
    {
        /// <summary>
        /// Gets or sets the value of the value
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// Gets or sets the value of the value as string
        /// </summary>
        public string? ValueAsString { get; set; }

        /// <summary>
        /// Gets or sets the value of the meta
        /// </summary>
        public object? Meta { get; set; }
    }

    /// <summary>
    /// The client
    /// </summary>
    private readonly ElasticClient _client;

    /// <summary>
    /// The get single document func
    /// </summary>
    private Func<string, Task<T>> _GetSingleDocumentFunc;

    /// <summary>
    /// The id selector func
    /// </summary>
    private Func<T, string> _idSelectorFunc;

    /// <summary>
    /// The reload document task queue
    /// </summary>
    private Task _ReloadDocumentTaskQueue;

    /// <summary>
    /// The concurrent queue
    /// </summary>
    private ConcurrentQueue<string> _reloadDocumentQueue = new ConcurrentQueue<string>();

    /// <summary>
    /// The cancellation token source
    /// </summary>
    private CancellationTokenSource _reloadDocumentCancellationTokenSource =
        new CancellationTokenSource();

    /// <summary>
    /// Gets or sets the value of the  ispaginguploadrunning
    /// </summary>
    private bool _IsPagingUploadRunning { get; set; }

    /// <summary>
    /// Gets or sets the value of the index name
    /// </summary>
    public string IndexName { get; private set; }

    /// <summary>
    /// Gets or sets the value of the max degree of parallelism
    /// </summary>
    public int MaxDegreeOfParallelism { get; set; } = 50;

    /// <summary>
    /// Gets the value of the is paging upload running
    /// </summary>
    public bool IsPagingUploadRunning => _IsPagingUploadRunning;

    /// <summary>
    /// Gets the value of the elastic client
    /// </summary>
    public ElasticClient ElasticClient => _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="ElasticSearchManager{T}"/> class
    /// </summary>
    /// <param name="elasticSearchURI">The elastic search uri</param>
    /// <param name="IndexName">The index name</param>
    /// <param name="userName">The user name</param>
    /// <param name="password">The password</param>
    /// <param name="getSingleDocument">The get single document</param>
    /// <param name="iDselectorFunc">The dselector func</param>
    public ElasticSearchManager(
        string elasticSearchURI,
        string IndexName,
        string userName,
        string password,
        Func<string, Task<T>> getSingleDocument,
        Func<T, string> iDselectorFunc
    )
    {
        this.IndexName = IndexName;
        ConnectionSettings connectionSettings = new ConnectionSettings(new Uri(elasticSearchURI))
            .BasicAuthentication(userName, password)
            .DisableDirectStreaming()
            .DefaultMappingFor((ClrTypeMappingDescriptor<T> m) => m.IndexName(this.IndexName))
            .DefaultIndex(this.IndexName);
        _client = new ElasticClient(connectionSettings);
        _GetSingleDocumentFunc = getSingleDocument;
        _idSelectorFunc = iDselectorFunc;
        _ReloadDocumentTaskQueue = new Task(
            delegate
            {
                __ProcessQueue();
            }
        );
        _ReloadDocumentTaskQueue.ContinueWith(
            delegate(Task t)
            {
                t.Dispose();
            }
        );
        _ReloadDocumentTaskQueue.Start();
    }

    /// <summary>
    /// Processes the queue
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void __ProcessQueue()
    {
        while (!_reloadDocumentCancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                if (
                    !_IsPagingUploadRunning
                    && _GetSingleDocumentFunc != null
                    && _reloadDocumentQueue.TryDequeue(out var result)
                )
                {
                    Console.WriteLine("Call _GetSingleDocumentFunc(" + result + ")");
                    T result2 = _GetSingleDocumentFunc(result).GetAwaiter().GetResult();
                    DeleteResponse result3 = _client
                        .DeleteAsync(result, (DeleteDescriptor<T> d) => d.Index(IndexName))
                        .GetAwaiter()
                        .GetResult();
                    if (!result3.IsValid)
                    {
                        throw new Exception(
                            "Failed to delete document with ID '"
                                + result
                                + "': "
                                + result3.ServerError?.Error?.Reason
                        );
                    }
                    if (result2 != null)
                    {
                        Upload(new List<T> { result2 });
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception) { }
        }
    }

    /// <summary>
    /// Creates the index
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void CreateIndex()
    {
        CreateIndexResponse createIndexResponse = _client.Indices.Create(
            IndexName,
            (CreateIndexDescriptor c) =>
                c.Map(
                    (TypeMappingDescriptor<T> m) =>
                        m.AutoMap()
                            .Properties(
                                delegate(PropertiesDescriptor<T> ps)
                                {
                                    PropertyInfo[] properties = typeof(T).GetProperties();
                                    foreach (PropertyInfo property in properties)
                                    {
                                        if (Attribute.IsDefined(property, typeof(TextAttribute)))
                                        {
                                            ps.Text(
                                                (TextPropertyDescriptor<T> t) =>
                                                    t.Name(property.Name)
                                                        .Fielddata(true)
                                                        .Fields(
                                                            (PropertiesDescriptor<T> f) =>
                                                                f.Keyword(
                                                                    (
                                                                        KeywordPropertyDescriptor<T> k
                                                                    ) =>
                                                                        k.Name("keyword")
                                                                            .IgnoreAbove(256)
                                                                )
                                                        )
                                            );
                                        }
                                    }
                                    return ps;
                                }
                            )
                )
        );
        if (!createIndexResponse.IsValid)
        {
            throw new Exception(
                "Failed to create index: " + createIndexResponse.ServerError?.Error?.Reason
            );
        }
    }

    /// <summary>
    /// Ises the index exist
    /// </summary>
    /// <returns>The bool</returns>
    public bool IsIndexExist()
    {
        string indexName = IndexName;
        return _client.Indices.Exists(indexName).Exists;
    }

    /// <summary>
    /// Deletes the index
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void DeleteIndex()
    {
        string indexName = IndexName;
        DeleteIndexResponse deleteIndexResponse = _client.Indices.Delete(indexName);
        if (!deleteIndexResponse.IsValid)
        {
            throw new Exception(
                "Failed to delete index '"
                    + indexName
                    + "': "
                    + deleteIndexResponse.ServerError?.Error?.Reason
            );
        }
    }

    /// <summary>
    /// Uploads the items
    /// </summary>
    /// <param name="items">The items</param>
    public void Upload(List<T> items)
    {
        BulkRequest bulkRequest = new BulkRequest(IndexName)
        {
            Operations = new List<IBulkOperation>(),
        };
        foreach (T item in items)
        {
            string text = _idSelectorFunc(item);
            bulkRequest.Operations.Add(new BulkIndexOperation<T>(item) { Id = text });
        }
        BulkResponse bulkResponse = _client.Bulk(bulkRequest);
        if (!bulkResponse.IsValid)
        {
            Console.WriteLine("Bulk indexing failed: " + bulkResponse.DebugInformation);
        }
    }

    /// <summary>
    /// Pagings the upload using the specified build data for one page
    /// </summary>
    /// <param name="buildDataForOnePage">The build data for one page</param>
    /// <param name="totalPages">The total pages</param>
    /// <param name="pOnCompletedCallback">The on completed callback</param>
    /// <exception cref="Exception">The PagingUpload method is running in background</exception>
    /// <exception cref="ArgumentException">The parameter 'totalPages' must be greater zero.</exception>
    public void PagingUpload(
        Func<int, List<T>> buildDataForOnePage,
        int totalPages,
        Action<long> pOnCompletedCallback = null
    )
    {
        if (totalPages < 0)
        {
            throw new ArgumentException("The parameter 'totalPages' must be greater zero.");
        }
        if (totalPages == 0)
        {
            return;
        }
        if (_IsPagingUploadRunning)
        {
            throw new Exception("The PagingUpload method is running in background");
        }
        _IsPagingUploadRunning = true;
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        SemaphoreSlim semaphore = new SemaphoreSlim(MaxDegreeOfParallelism);
        try
        {
            ConcurrentBag<Task> tasks = new ConcurrentBag<Task>();
            Task task = new Task(
                delegate
                {
                    Parallel.ForEach(
                        Enumerable.Range(1, totalPages),
                        new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
                        delegate(int page)
                        {
                            Task task2 = new Task(
                                delegate
                                {
                                    try
                                    {
                                        semaphore.Wait();
                                        List<T> items = buildDataForOnePage(page);
                                        Upload(items);
                                    }
                                    finally
                                    {
                                        semaphore.Release();
                                    }
                                }
                            );
                            tasks.Add(task2);
                            task2.ContinueWith(
                                delegate(Task t)
                                {
                                    t.Dispose();
                                }
                            );
                            task2.Start();
                        }
                    );
                }
            );
            task.ContinueWith(
                delegate(Task t)
                {
                    Task.WhenAll(tasks).GetAwaiter().GetResult();
                    t.Dispose();
                    _IsPagingUploadRunning = false;
                    stopWatch.Stop();
                    pOnCompletedCallback?.Invoke(stopWatch.ElapsedMilliseconds);
                }
            );
            task.Start();
        }
        finally
        {
            if (semaphore != null)
            {
                ((IDisposable)semaphore).Dispose();
            }
        }
    }

    /// <summary>
    /// Deletes the by document id using the specified  id
    /// </summary>
    /// <param name="_id">The id</param>
    /// <returns>The bool</returns>
    public bool DeleteByDocumentId(string _id)
    {
        string indexName = IndexName;
        DeleteResponse deleteResponse = _client.Delete(
            _id,
            (DeleteDescriptor<T> d) => d.Index(indexName)
        );
        if (!deleteResponse.IsValid)
        {
            Console.WriteLine("Delete failed: " + deleteResponse.DebugInformation);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Deletes the by query container using the specified condition
    /// </summary>
    /// <param name="condition">The condition</param>
    /// <returns>The bool</returns>
    private bool __DeleteByQueryContainer(QueryContainer condition)
    {
        DeleteByQueryRequest<T> request = new DeleteByQueryRequest<T>(IndexName)
        {
            Query = condition,
        };
        DeleteByQueryResponse deleteByQueryResponse = _client.DeleteByQuery(request);
        if (!deleteByQueryResponse.IsValid)
        {
            Console.WriteLine("Delete by query failed: " + deleteByQueryResponse.DebugInformation);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Deletes the by date range query using the specified condition
    /// </summary>
    /// <param name="condition">The condition</param>
    /// <returns>The bool</returns>
    public bool DeleteByDateRangeQuery(DateRangeQuery condition)
    {
        return __DeleteByQueryContainer(condition);
    }

    /// <summary>
    /// Deletes the by match query using the specified condition
    /// </summary>
    /// <param name="condition">The condition</param>
    /// <returns>The bool</returns>
    public bool DeleteByMatchQuery(MatchQuery condition)
    {
        return __DeleteByQueryContainer(condition);
    }

    /// <summary>
    /// Searches the by query using the specified json query
    /// </summary>
    /// <param name="jsonQuery">The json query</param>
    /// <exception cref="Exception"></exception>
    /// <returns>A list of t</returns>
    public List<T> SearchByQuery(string jsonQuery)
    {
        BytesResponse bytesResponse = _client.LowLevel.Search<BytesResponse>("casa_0", jsonQuery);
        if (bytesResponse.Success && bytesResponse.Body != null)
        {
            using (MemoryStream stream = new MemoryStream(bytesResponse.Body))
            {
                return _client
                    .RequestResponseSerializer.Deserialize<SearchResponse<T>>(stream)
                    .Documents.ToList();
            }
        }
        throw new Exception(
            "Error with DSL [" + jsonQuery + "]: " + bytesResponse.DebugInformation
        );
    }

    /// <summary>
    /// Aggregates the by query using the specified json query
    /// </summary>
    /// <param name="jsonQuery">The json query</param>
    /// <returns>A dictionary of string and object</returns>
    public Dictionary<string, object> AggregateByQuery(string jsonQuery)
    {
        BytesResponse bytesResponse = _client.LowLevel.Search<BytesResponse>("casa_0", jsonQuery);
        if (bytesResponse.Success && bytesResponse.Body != null)
        {
            MemoryStream stream = new MemoryStream(bytesResponse.Body);
            AggregateDictionary aggregations = _client
                .RequestResponseSerializer.Deserialize<SearchResponse<T>>(stream)
                .Aggregations;
            aggregations.TryGetValue(aggregations.Keys.First(), out var value);
            List<KeyValuePair<string, ESAggregationQueryResult>> list = JsonConvert
                .DeserializeObject<Dictionary<string, ESAggregationQueryResult>>(
                    JsonConvert.SerializeObject(value)
                )
                .ToList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            {
                foreach (KeyValuePair<string, ESAggregationQueryResult> item in list)
                {
                    dictionary.Add(item.Key, item.Value.Value);
                }
                return dictionary;
            }
        }
        Console.WriteLine("Search failed: " + bytesResponse.DebugInformation);
        return new Dictionary<string, object>();
    }

    /// <summary>
    /// Raises the change using the specified  id
    /// </summary>
    /// <param name="_id">The id</param>
    public void RaiseChange(string _id)
    {
        if (_GetSingleDocumentFunc != null)
        {
            _reloadDocumentQueue.Enqueue(_id);
        }
    }

    /// <summary>
    /// Disposes this instance
    /// </summary>
    public void Dispose()
    {
        _reloadDocumentCancellationTokenSource.Cancel();
        _reloadDocumentCancellationTokenSource.Dispose();
        _ReloadDocumentTaskQueue.Dispose();
    }
}
