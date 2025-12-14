using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Web.CMS.Configuration;
using O24OpenAPI.Web.CMS.Models.Media;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.Web.CMS.Services;

public class S3FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _baseUrl;

    public S3FileStorageService()
    {
        var storageConfig = Singleton<AppSettings>.Instance.Get<StorageConfiguration>();
        var accessKey = storageConfig.S3.AccessKey;
        var secretKey = storageConfig.S3.SecretKey;
        var region = storageConfig.S3.Region;
        _bucketName = storageConfig.S3.BucketName;
        _baseUrl = storageConfig.S3.BaseUrl?.TrimEnd('/') ?? string.Empty;
        var regionEndpoint = RegionEndpoint.GetBySystemName(region);
        _s3Client = new AmazonS3Client(accessKey, secretKey, regionEndpoint);
    }

    /// <summary>
    /// Uploads a file to S3 asynchronously
    /// </summary>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<FileUploadResult> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string category = "general",
        string customerCode = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (fileStream == null || fileStream.Length == 0)
                throw new ArgumentException("File stream is empty");

            var ext = Path.GetExtension(fileName);
            var key = BuildKey(ext, category, customerCode);

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = contentType
            };

            var response = await _s3Client.PutObjectAsync(request, cancellationToken);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Upload failed with status: {response.HttpStatusCode}");
            }

            var url = $"{_baseUrl}/{key}";

            return new FileUploadResult
            {
                Key = key,
                Url = url
            };
        }
        catch (AmazonS3Exception ex)
        {
            await ex.LogErrorAsync();
            throw new Exception($"AWS S3 error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw new Exception($"Upload to S3 failed: {ex.Message}", ex);
        }
    }


    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(request, cancellationToken);
    }

    public string GetFileUrl(string key, TimeSpan? expiresIn = null)
    {
        if (!string.IsNullOrEmpty(_baseUrl))
            return $"{_baseUrl}/{key}";

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = DateTime.UtcNow.Add(expiresIn ?? TimeSpan.FromMinutes(15))
        };

        return _s3Client.GetPreSignedURL(request);
    }


    /// <summary>
    /// Get file from S3 asynchronously
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<(Stream Stream, string ContentType)> GetAsync(
       string key,
       CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            var response = await _s3Client.GetObjectAsync(request, cancellationToken);

            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            var contentType = response.Headers.ContentType ?? "application/octet-stream";

            return (memoryStream, contentType);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return (Stream.Null, null);
        }
        catch (Exception ex)
        {
            throw new Exception($"Get file from S3 failed: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Build Key
    /// </summary>
    /// <param name="extension"></param>
    /// <param name="category"></param>
    /// <param name="customerCode"></param>
    /// <returns></returns>
    private static string BuildKey(string extension, string category, string customerCode = null)
    {
        var now = DateTime.UtcNow;

        category = string.IsNullOrWhiteSpace(category)
            ? "general"
            : category.Trim().ToLower();

        string folder;

        if (category == "kyc")
        {
            if (string.IsNullOrWhiteSpace(customerCode))
                throw new ArgumentException("customerCode is required when category = KYC");

            customerCode = customerCode.Trim().ToLower();

            folder = $"o24/cms/{category}/{customerCode}/{now:yyyy}/{now:MM}";
        }
        else
        {
            folder = $"o24/cms/{category}/{now:yyyy}/{now:MM}";
        }

        var fileId = Guid.NewGuid().ToString("N");

        return $"{folder}/{fileId}{extension}";
    }


    /// <summary>
    /// List categories (folders) under the CMS root
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<string>> ListCategoriesAsync(CancellationToken cancellationToken = default)
    {
        const string prefix = "o24/cms/";   // root folder của CMS
        var categories = new List<string>();
        string continuationToken = null;

        do
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = prefix,
                Delimiter = "/",
                ContinuationToken = continuationToken
            };

            var response = await _s3Client.ListObjectsV2Async(request, cancellationToken);
            var prefixes = response.CommonPrefixes ?? [];

            foreach (var cp in prefixes)
            {
                var name = cp[prefix.Length..].TrimEnd('/');
                if (!string.IsNullOrWhiteSpace(name))
                {
                    categories.Add(name);
                }
            }

            continuationToken = (bool)response.IsTruncated ? response.NextContinuationToken : null;

        } while (continuationToken != null);

        categories = [.. categories.Where(x => !int.TryParse(x, out _))];

        return categories;
    }

    /// <summary>
    /// Browse files and folders under a category and path
    /// </summary>
    /// <param name="category"></param>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<MediaBrowseResultModel> BrowseAsync(
        Dictionary<string, string> listTrackerCodeMedia,
        string category,
        string path = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category is required", nameof(category));

        category = category.Trim().Trim('/');

        var basePrefix = $"o24/cms/{category}/";

        string effectivePrefix = basePrefix;

        if (!string.IsNullOrWhiteSpace(path))
        {
            path = path.Trim().Trim('/');
            effectivePrefix = $"{basePrefix}{path}/";
        }

        var result = new MediaBrowseResultModel
        {
            Category = category,
            Path = path ?? string.Empty
        };

        string continuationToken = null;

        do
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = effectivePrefix,
                Delimiter = "/",
                ContinuationToken = continuationToken
            };

            var response = await _s3Client.ListObjectsV2Async(request, cancellationToken);

            var prefixes = response.CommonPrefixes ?? [];
            foreach (var cp in prefixes)
            {
                var rest = cp[effectivePrefix.Length..];
                var folderName = rest.TrimEnd('/');

                if (!string.IsNullOrWhiteSpace(folderName))
                {
                    if (!result.Folders.Any(f => f.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        result.Folders.Add(new MediaFolderItem
                        {
                            Name = folderName
                        });
                    }
                }
            }

            var objects = response.S3Objects ?? [];

            foreach (var obj in objects)
            {
                if (string.IsNullOrEmpty(obj.Key) || obj.Key.EndsWith("/"))
                    continue;

                if (!obj.Key.StartsWith(effectivePrefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                var fileName = obj.Key[(effectivePrefix.Length)..];

                result.Files.Add(new MediaFileItem
                {
                    Name = fileName,
                    S3Key = obj.Key,
                    Size = obj.Size,
                    LastModified = obj.LastModified
                });
            }


            continuationToken = (bool)response.IsTruncated ? response.NextContinuationToken : null;

        } while (continuationToken != null);

        if (result.Files.Count > 0 && listTrackerCodeMedia != null && listTrackerCodeMedia.Count > 0)
        {
            foreach (var file in result.Files)
            {
                if (listTrackerCodeMedia.TryGetValue(file.S3Key, out var trackerCode))
                    file.TrackerCode = trackerCode;
            }
        }

        return result;
    }

    /// <summary>
    /// Build category tree
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<MediaFolderNode> BuildCategoryTreeAsync(
        Dictionary<string, string> listTrackerCodeMedia,
        string category,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category is required", nameof(category));

        category = category.Trim().Trim('/');

        var root = new MediaFolderNode
        {
            Name = category,
            Path = string.Empty
        };

        string basePrefix = $"o24/cms/{category}/";

        await BuildFolderRecursiveAsync(root, basePrefix, cancellationToken);

        // Sau khi build xong, map TrackerCode cho tất cả file:
        AttachTrackerCodes(root, listTrackerCodeMedia);

        return root;
    }

    /// <summary>
    /// Build đệ quy cho folder
    /// </summary>
    /// <param name="current"></param>
    /// <param name="effectivePrefix"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task BuildFolderRecursiveAsync(
       MediaFolderNode current,
       string effectivePrefix,
       CancellationToken cancellationToken)
    {
        string continuationToken = null;

        do
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = effectivePrefix,
                Delimiter = "/",
                ContinuationToken = continuationToken
            };

            var response = await _s3Client.ListObjectsV2Async(request, cancellationToken);

            var prefixes = response.CommonPrefixes ?? new List<string>();
            var objects = response.S3Objects ?? new List<S3Object>();

            foreach (var cp in prefixes)
            {
                if (!cp.StartsWith(effectivePrefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                var rest = cp[effectivePrefix.Length..];
                var folderName = rest.TrimEnd('/');

                if (string.IsNullOrWhiteSpace(folderName))
                    continue;

                // build path mới
                var childPath = string.IsNullOrEmpty(current.Path)
                    ? folderName
                    : $"{current.Path}/{folderName}";

                // tránh trùng
                if (current.Folders.Any(f =>
                        f.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase)))
                    continue;

                var childNode = new MediaFolderNode
                {
                    Name = folderName,
                    Path = childPath
                };

                current.Folders.Add(childNode);

                var childPrefix = $"{effectivePrefix}{folderName}/";
                await BuildFolderRecursiveAsync(childNode, childPrefix, cancellationToken);
            }

            foreach (var obj in objects)
            {
                // Bỏ folder object
                if (string.IsNullOrEmpty(obj.Key) || obj.Key.EndsWith("/"))
                    continue;

                if (!obj.Key.StartsWith(effectivePrefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                var fileName = obj.Key.Substring(effectivePrefix.Length); // phần sau prefix

                current.Files.Add(new MediaFileItem
                {
                    Name = fileName,
                    S3Key = obj.Key,
                    Size = obj.Size,
                    LastModified = obj.LastModified
                });
            }

            continuationToken = (bool)response.IsTruncated ? response.NextContinuationToken : null;

        } while (continuationToken != null);
    }

    /// <summary>
    /// Attach TrackerCodes to all files in the folder tree
    /// </summary>
    /// <param name="root"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static void AttachTrackerCodes(
         MediaFolderNode root,
         Dictionary<string, string> listTrackerCodeMedia
     )
    {
        if (root == null || listTrackerCodeMedia == null || listTrackerCodeMedia.Count == 0)
            return;

        var allFiles = new List<MediaFileItem>();
        CollectFiles(root, allFiles);

        if (allFiles.Count == 0)
            return;

        foreach (var file in allFiles)
        {
            if (string.IsNullOrWhiteSpace(file?.S3Key))
                continue;

            if (listTrackerCodeMedia.TryGetValue(file.S3Key, out var trackerCode))
                file.TrackerCode = trackerCode;
        }
    }


    /// <summary>
    /// Collect all files in the folder tree
    /// </summary>
    /// <param name="node"></param>
    /// <param name="acc"></param>
    private static void CollectFiles(MediaFolderNode node, List<MediaFileItem> acc)
    {
        if (node.Files != null && node.Files.Count > 0)
            acc.AddRange(node.Files);

        if (node.Folders == null)
            return;

        foreach (var folder in node.Folders)
            CollectFiles(folder, acc);
    }


    /// <summary>
    /// Delete file by key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task DeleteFileAsync(string key)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(request);
    }

    /// <summary>
    /// Delete multiple files by keys
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public async Task DeleteFilesAsync(List<string> keys)
    {
        var request = new DeleteObjectsRequest
        {
            BucketName = _bucketName,
            Objects = [.. keys.Select(k => new KeyVersion { Key = k })]
        };

        await _s3Client.DeleteObjectsAsync(request);
    }

}
