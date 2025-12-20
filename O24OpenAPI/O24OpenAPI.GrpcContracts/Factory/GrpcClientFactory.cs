using System.Collections.Concurrent;
using System.Linq.Expressions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.GrpcContracts.Configuration;
using O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;
using O24OpenAPI.Logging.Interceptors;

namespace O24OpenAPI.GrpcContracts.Factory;

public sealed class GrpcClientFactory(GrpcClientLoggingInterceptor loggingInterceptor)
    : IGrpcClientFactory,
        IDisposable
{
    private readonly GrpcClientsConfig _configs = Singleton<GrpcClientsConfig>.Instance;
    private readonly ConcurrentDictionary<string, ChannelWrapper> _channels = new();
    private static readonly ConcurrentDictionary<Type, Lazy<Delegate>> _clientFactoryCache = new();

    private GrpcClientLoggingInterceptor _loggingInterceptor = loggingInterceptor;

    private readonly SemaphoreSlim _channelLock = new(1, 1);

    private const int MaxRetryAttempts = 3;
    private const int RetryDelayMs = 1000;

    private const int StaleChannelCleanupMs = 5 * 60 * 1000;

    public TClient GetClient<TClient>()
        where TClient : ClientBase
    {
        return GetClientAsync<TClient>().GetAwaiter().GetResult();
    }

    public async Task<TClient> GetClientAsync<TClient>()
        where TClient : ClientBase
    {
        var (address, clientTypeName) = GetServiceAddress<TClient>();
        var channel = await GetOrCreateChannelAsync(clientTypeName, address).ConfigureAwait(false);
        return CreateClientFromChannel<TClient>(channel);
    }

    public async Task<AsyncServerStreamingCall<TResponse>> GetServerStreamAsync<
        TClient,
        TRequest,
        TResponse
    >(
        Func<TClient, TRequest, CallOptions, AsyncServerStreamingCall<TResponse>> streamMethod,
        TRequest request,
        CallOptions? callOptions = null
    )
        where TClient : ClientBase
    {
        var client = await GetClientAsync<TClient>().ConfigureAwait(false);
        var options = callOptions ?? new CallOptions();

        for (int attempt = 0; attempt < MaxRetryAttempts; attempt++)
        {
            try
            {
                return streamMethod(client, request, options);
            }
            catch (RpcException ex) when (IsConnectionError(ex) && attempt < MaxRetryAttempts - 1)
            {
                var (address, clientTypeName) = GetServiceAddress<TClient>();
                await RecreateChannelAsync(clientTypeName, address).ConfigureAwait(false);
                client = await GetClientAsync<TClient>().ConfigureAwait(false);
                await Task.Delay(RetryDelayMs * (attempt + 1)).ConfigureAwait(false);
            }
        }

        throw new InvalidOperationException(
            $"Failed to establish stream after {MaxRetryAttempts} attempts"
        );
    }

    private (string address, string clientTypeName) GetServiceAddress<TClient>()
    {
        var clientTypeName =
            typeof(TClient).DeclaringType?.Name
            ?? throw new InvalidOperationException(
                $"Could not determine the service name for gRPC client type '{typeof(TClient).FullName}'."
            );

        if (!_configs.TryGetValue(clientTypeName, out var address) || string.IsNullOrEmpty(address))
        {
            var wfoGrpcClient = EngineContext.Current.Resolve<IWFOGrpcClientService>();
            var serviceInfo = wfoGrpcClient
                .GetServiceInstanceByServiceHandleNameAsync(clientTypeName)
                .GetAwaiter()
                .GetResult();

            if (serviceInfo != null)
            {
                address = serviceInfo.service_grpc_url;
                Singleton<GrpcClientsConfig>.Instance[clientTypeName] = address;

                if (string.IsNullOrEmpty(address))
                {
                    throw new InvalidOperationException(
                        $"gRPC client address for service '{clientTypeName}' is not configured."
                    );
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"No gRPC client address configured for service '{clientTypeName}'."
                );
            }
        }

        return (address, clientTypeName);
    }

    private async Task<GrpcChannel> GetOrCreateChannelAsync(string serviceName, string address)
    {
        if (_channels.TryGetValue(serviceName, out var existing) && existing.Address == address)
        {
            if (await IsChannelHealthyAsync(existing.Channel).ConfigureAwait(false))
            {
                return existing.Channel;
            }
        }

        await _channelLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (_channels.TryGetValue(serviceName, out existing) && existing.Address == address)
            {
                if (await IsChannelHealthyAsync(existing.Channel).ConfigureAwait(false))
                {
                    return existing.Channel;
                }
            }

            var newWrapper = CreateChannelWrapper(address);
            _channels.AddOrUpdate(serviceName, newWrapper, (_, __) => newWrapper);
            _ = CleanupStaleWrappersAsync();

            return newWrapper.Channel;
        }
        finally
        {
            _channelLock.Release();
        }
    }

    private async Task RecreateChannelAsync(string serviceName, string address)
    {
        await _channelLock.WaitAsync().ConfigureAwait(false);
        try
        {
            var newWrapper = CreateChannelWrapper(address);
            _channels.AddOrUpdate(serviceName, newWrapper, (_, __) => newWrapper);

            _ = CleanupStaleWrappersAsync();
        }
        finally
        {
            _channelLock.Release();
        }
    }

    private static async Task<bool> IsChannelHealthyAsync(GrpcChannel channel)
    {
        try
        {
            var state = channel.State;

            if (state == ConnectivityState.Shutdown || state == ConnectivityState.TransientFailure)
            {
                return false;
            }

            if (state == ConnectivityState.Idle || state == ConnectivityState.Connecting)
            {
                var connectTask = channel.ConnectAsync();
                var timeoutTask = Task.Delay(5000);
                var completedTask = await Task.WhenAny(connectTask, timeoutTask)
                    .ConfigureAwait(false);

                return completedTask == connectTask;
            }

            return state == ConnectivityState.Ready;
        }
        catch
        {
            return false;
        }
    }

    private static ChannelWrapper CreateChannelWrapper(string address)
    {
        var channel = GrpcChannel.ForAddress(
            address,
            new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                },
                MaxRetryAttempts = 3,
                MaxRetryBufferSize = 16 * 1024 * 1024,
            }
        );

        return new ChannelWrapper(channel, address);
    }

    private async Task CleanupStaleWrappersAsync()
    {
        try
        {
            var threshold = DateTime.UtcNow - TimeSpan.FromMilliseconds(StaleChannelCleanupMs);
            foreach (var kvp in _channels)
            {
                if (kvp.Value.CreatedAt < threshold)
                {
                    _channels.TryRemove(
                        new KeyValuePair<string, ChannelWrapper>(kvp.Key, kvp.Value)
                    );
                }
            }
        }
        catch { }

        await Task.CompletedTask;
    }

    private TClient CreateClientFromChannel<TClient>(GrpcChannel channel)
        where TClient : ClientBase
    {
        _loggingInterceptor ??= new GrpcClientLoggingInterceptor();
        var interceptedInvoker = channel.Intercept(_loggingInterceptor);

        var factoryLazy = _clientFactoryCache.GetOrAdd(
            typeof(TClient),
            static type => new Lazy<Delegate>(() =>
            {
                var invokerParam = Expression.Parameter(typeof(CallInvoker), "invoker");
                var constructor =
                    type.GetConstructor(new[] { typeof(CallInvoker) })
                    ?? throw new InvalidOperationException(
                        $"gRPC client type '{type.FullName}' does not have a public constructor that accepts a CallInvoker."
                    );

                var newExpression = Expression.New(constructor, invokerParam);
                var lambda = Expression.Lambda<Func<CallInvoker, TClient>>(
                    newExpression,
                    invokerParam
                );

                return lambda.Compile();
            })
        );

        var factoryDelegate = (Func<CallInvoker, TClient>)factoryLazy.Value;
        return factoryDelegate(interceptedInvoker);
    }

    private static bool IsConnectionError(RpcException ex)
    {
        return ex.StatusCode == StatusCode.Unavailable
            || ex.StatusCode == StatusCode.Internal
            || ex.StatusCode == StatusCode.Unknown
            || ex.StatusCode == StatusCode.DeadlineExceeded;
    }

    public void Dispose()
    {
        _channels.Clear();
        _channelLock.Dispose();
    }

    private class ChannelWrapper
    {
        public GrpcChannel Channel { get; }
        public string Address { get; }
        public DateTime CreatedAt { get; }

        public ChannelWrapper(GrpcChannel channel, string address)
        {
            Channel = channel;
            Address = address;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
