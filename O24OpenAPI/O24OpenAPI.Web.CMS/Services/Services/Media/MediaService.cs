using Amazon.S3;
using LinqToDB;
using O24OpenAPI.Web.CMS.Models.Media;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.CMS.Services.Interfaces.Media;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.Web.CMS.Services.Services.Media;

public class MediaService(
    IRepository<MediaStaging> mediaStagingRepository,
    IRepository<MediaFile> mediaFileRepository,
    IWebHostEnvironment environment,
    IFileStorageService fileStorageService
) : IMediaService
{
    private readonly IRepository<MediaStaging> _mediaStagingRepository = mediaStagingRepository;
    private readonly IRepository<MediaFile> _mediaFileRepository = mediaFileRepository;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly IFileStorageService _fileStorageService = fileStorageService;



    public async Task<MediaStaging> GetByTrackerCode(string trackerCode)
    {
        return await _mediaStagingRepository
            .Table.Where(x => x.TrackerCode == trackerCode)
            .FirstOrDefaultAsync();
    }
    /// <summary>
    /// Get Media File by Tracker Code
    /// </summary>
    /// <param name="trackerCode"></param>
    /// <returns></returns>
    public async Task<MediaModel> GetMediaAsync(string trackerCode)
    {
        if (string.IsNullOrWhiteSpace(trackerCode))
            return null;

        // Query kho chính
        var mainQuery =
            from m in _mediaFileRepository.Table
            where m.TrackerCode == trackerCode
            select new MediaModel
            {
                TrackerCode = m.TrackerCode,
                FileUrl = m.FileUrl,
                FileHash = m.FileHash,
                FolderName = m.FolderName,
                FileName = m.FileName,
                FileExtension = m.FileExtension,
                FileSize = m.FileSize,
                CreatedOnUtc = m.CreatedOnUtc,
                ExpiredOnUtc = m.ExpiredOnUtc,
                CreatedBy = m.CreatedBy,
                IsTemp = false
            };

        // Query staging
        var stagingQuery =
            from s in _mediaStagingRepository.Table
            where s.TrackerCode == trackerCode
            select new MediaModel
            {
                TrackerCode = s.TrackerCode,
                FileUrl = s.FileUrl,
                FileHash = s.FileHash,
                FolderName = s.FolderName,
                FileName = s.FileName,
                FileExtension = s.FileExtension,
                FileSize = s.FileSize,
                CreatedOnUtc = s.CreatedOnUtc,
                ExpiredOnUtc = s.ExpiredOnUtc,
                CreatedBy = s.CreatedBy,
                IsTemp = true
            };

        var unionQuery = mainQuery.Concat(stagingQuery);

        var result = await unionQuery
            .OrderBy(x => x.IsTemp)
            .FirstOrDefaultAsync();

        return result;
    }

    /// <summary>
    /// Insert Media Staging
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<MediaStaging> InsertMediaStagingAsync(MediaStaging entity)
    {
        await _mediaStagingRepository.InsertAsync(entity);
        return entity;
    }
    /// <summary>
    /// Insert Media File
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<MediaFile> InserMediaFileAsync(MediaFile entity)
    {
        await _mediaFileRepository.InsertAsync(entity);
        return entity;
    }


    public async Task<MediaStaging> UpdateAsync(MediaStaging entity)
    {
        await _mediaStagingRepository.Update(entity);
        return entity;
    }

    public async Task<bool> SyncMediaAsync(MediaSyncModel model)
    {
        if (model.MediaListSync == null || model.MediaListSync.Count == 0)
        {
            return false;
        }

        foreach (var code in model.MediaListSync)
        {
            var media = await GetByTrackerCode(code);
            if (media != null)
            {
                media.Status = Constant.Action.ACTIVE;
                await UpdateAsync(media);
            }
        }

        return true;
    }

    public async Task PromoteMedia()
    {
        try
        {
            var activeList = await _mediaStagingRepository
                .Table.Where(x => x.Status == Constant.Action.ACTIVE)
                .ToListAsync();

            if (activeList != null && activeList.Count > 0)
            {
                foreach (var item in activeList)
                {
                    try
                    {
                        var mediaFile = new MediaFile
                        {
                            TrackerCode = item.TrackerCode,
                            FileUrl = item.FileUrl,
                            Base64String = item.Base64String,
                            FileHash = item.FileHash,
                            FolderName = item.FolderName,
                            FileName = item.FileName,
                            FileExtension = item.FileExtension,
                            FileSize = item.FileSize,
                            Status = Constant.Action.ACTIVE,
                            CreatedOnUtc = item.CreatedOnUtc,
                            ExpiredOnUtc = item.ExpiredOnUtc,
                            CreatedBy = item.CreatedBy,
                        };

                        await _mediaFileRepository.InsertAsync(mediaFile);
                        await _mediaStagingRepository.Delete(item);
                    }
                    catch (Exception itemEx)
                    {
                        Console.WriteLine($"[PromoteMedia] Promote error for TrackerCode={item?.TrackerCode}: {itemEx.Message}");
                    }
                }
            }

            var utcNow = DateTime.UtcNow;
            var expiredPendings = await _mediaStagingRepository.Table
                .Where(x => x.Status == Constant.Action.PENDING
                            && x.ExpiredOnUtc.HasValue
                            && x.ExpiredOnUtc.Value <= utcNow)
                .ToListAsync();

            if (expiredPendings != null && expiredPendings.Count > 0)
            {
                foreach (var item in expiredPendings)
                {
                    try
                    {
                        try
                        {
                            await DeleteFileAsync(item.FileUrl);
                        }
                        catch (AmazonS3Exception s3Ex)
                        {
                            await s3Ex.LogErrorAsync($"[PromoteMedia] S3 delete error for Key={item.FileUrl}: {s3Ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            await ex.LogErrorAsync($"[PromoteMedia] Unexpected S3 delete exception for Key={item.FileUrl}: {ex.Message}");
                        }

                        // ---- REMOVE FROM STAGING TABLE ----
                        await _mediaStagingRepository.Delete(item);
                    }
                    catch (Exception itemEx)
                    {
                        await itemEx.LogErrorAsync(
                            $"[PromoteMedia] Cleanup error for TrackerCode={item?.TrackerCode}: {itemEx.Message}"
                        );
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PromoteMedia] Error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Suy ra đường dẫn vật lý để xóa file.
    /// - Nếu FileUrl là đường dẫn tuyệt đối (C:\... hoặc /var/...): dùng luôn.
    /// - Nếu là URL (http/https), lấy phần path sau domain và ghép vào web root.
    /// - Nếu chỉ có FileName: ghép vào thư mục uploads mặc định.
    /// NOTE: Chỉnh _mediaRoot hoặc _webRoot cho phù hợp hệ thống của bạn.
    /// </summary>
    private string ResolvePhysicalPath(string fileUrl, string fileName, string foldername)
    {
        try
        {

            string _webRoot = _environment?.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot");
            string _mediaRoot = Path.Combine(_webRoot, "uploads", foldername);

            if (!string.IsNullOrWhiteSpace(fileUrl))
            {
                if (Path.IsPathRooted(fileUrl))
                {
                    return fileUrl;
                }

                if (Uri.TryCreate(fileUrl, UriKind.Absolute, out var uri))
                {
                    var localPath = uri.LocalPath.TrimStart('/', '\\');
                    return Path.Combine(_webRoot, localPath);
                }

                var relative = fileUrl.TrimStart('/', '\\');
                return Path.Combine(_webRoot, relative);
            }

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                return Path.Combine(_mediaRoot, fileName);
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PromoteMedia] ResolvePhysicalPath error: {ex.Message}");
            return string.Empty;
        }
    }

    public async Task<MediaModel> GetMediaByHashId(string hashid)
    {
        if (string.IsNullOrWhiteSpace(hashid))
            return null;

        // Query kho chính
        var mainQuery =
            from m in _mediaFileRepository.Table
            where m.FileHash == hashid
            select new MediaModel
            {
                TrackerCode = m.TrackerCode,
                FileUrl = m.FileUrl,
                FileHash = m.FileHash,
                FolderName = m.FolderName,
                FileName = m.FileName,
                FileExtension = m.FileExtension,
                FileSize = m.FileSize,
                CreatedOnUtc = m.CreatedOnUtc,
                ExpiredOnUtc = m.ExpiredOnUtc,
                CreatedBy = m.CreatedBy,
                IsTemp = false
            };

        // Query staging

        var stagingQuery =
            from s in _mediaStagingRepository.Table
            where s.FileHash == hashid
            select new MediaModel
            {
                TrackerCode = s.TrackerCode,
                FileUrl = s.FileUrl,
                FileHash = s.FileHash,
                FolderName = s.FolderName,
                FileName = s.FileName,
                FileExtension = s.FileExtension,
                FileSize = s.FileSize,
                CreatedOnUtc = s.CreatedOnUtc,
                ExpiredOnUtc = s.ExpiredOnUtc,
                CreatedBy = s.CreatedBy,
                IsTemp = true
            };

        var unionQuery = mainQuery.Concat(stagingQuery);

        var result = await unionQuery
            .OrderBy(x => x.IsTemp)
            .FirstOrDefaultAsync();

        return result;
    }

    /// <summary>
    /// Get Media File by File URL
    /// </summary>
    /// <param name="fileURL"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, string>> GetMediaByFileURL(List<string> fileUrls)
    {
        if (fileUrls == null || fileUrls.Count == 0)
            return [];

        var mainQuery =
            from m in _mediaFileRepository.Table
            where fileUrls.Contains(m.FileUrl)
            select new
            {
                m.FileUrl,
                m.TrackerCode,
                IsTemp = false
            };

        var stagingQuery =
            from s in _mediaStagingRepository.Table
            where fileUrls.Contains(s.FileUrl)
            select new
            {
                s.FileUrl,
                s.TrackerCode,
                IsTemp = true
            };

        var union = mainQuery.Concat(stagingQuery);

        var list = await union
            .ToListAsync();

        var dict = list
            .GroupBy(x => x.FileUrl)
            .ToDictionary(
                g => g.Key,
                g => g.OrderBy(x => x.IsTemp).First().TrackerCode
            );

        return dict;
    }

    public async Task<(Stream, string)> ViewMedia(string trackerCode)
    {
        if (string.IsNullOrWhiteSpace(trackerCode))
            return (Stream.Null, null);

        var media = await GetMediaAsync(trackerCode);
        if (media == null || string.IsNullOrEmpty(media.FileUrl))
            return (Stream.Null, null);

        var (stream, contentType) = await _fileStorageService.GetAsync(media.FileUrl);
        return (stream, contentType);
    }

    /// <summary>
    /// Get list categories
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<string>> ListCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _fileStorageService.ListCategoriesAsync(cancellationToken);
        return categories;
    }

    /// <summary>
    /// Browse media files and folders
    /// </summary>
    /// <param name="category"></param>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<MediaBrowseResultModel> BrowseAsync(string category, string path = null, CancellationToken cancellationToken = default)
    {
        var listTrackerCodeMedia = await GetListTrackerCodeMedia();

        var result = await _fileStorageService.BrowseAsync(listTrackerCodeMedia, category, path, cancellationToken);
        return result;
    }

    /// <summary>
    /// Build category tree
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<MediaFolderNode> BuildCategoryTreeAsync(string category, CancellationToken cancellationToken = default)
    {
        var listTrackerCodeMedia = await GetListTrackerCodeMedia();
        var tree = await _fileStorageService.BuildCategoryTreeAsync(listTrackerCodeMedia, category, cancellationToken);
        return tree;
    }

    /// <summary>
    /// Delete file by key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task DeleteFileAsync(string key)
    {
        await _fileStorageService.DeleteFileAsync(key);
    }

    /// <summary>
    /// Delete files by keys
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public async Task DeleteFilesAsync(List<string> keys)
    {
        await _fileStorageService.DeleteFilesAsync(keys);
    }
    /// <summary>
    /// Get Media File by Tracker Code
    /// </summary>
    /// <param name="fileUrls"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, string>> GetListTrackerCodeMedia()
    {
        var mainQuery =
            from m in _mediaFileRepository.Table
            select new
            {
                m.FileUrl,
                m.TrackerCode,
                IsTemp = false
            };

        var stagingQuery =
            from s in _mediaStagingRepository.Table
            select new
            {
                s.FileUrl,
                s.TrackerCode,
                IsTemp = true
            };

        var union = mainQuery.Concat(stagingQuery);

        var list = await union
            .ToListAsync();

        var dict = list
            .GroupBy(x => x.FileUrl)
            .ToDictionary(
                g => g.Key,
                g => g.OrderBy(x => x.IsTemp).First().TrackerCode
            );

        return dict;
    }
}
