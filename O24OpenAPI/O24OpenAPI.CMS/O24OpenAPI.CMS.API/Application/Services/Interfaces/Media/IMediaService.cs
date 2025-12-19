using O24OpenAPI.CMS.API.Application.Models.Media;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces.Media;

public interface IMediaService
{
    /// <summary>
    /// Get ByTracker Code
    /// </summary>
    /// <param name="trackerCode"></param>
    /// <returns></returns>
    Task<MediaStaging> GetByTrackerCode(string trackerCode);

    /// <summary>
    /// Get Media File Async
    /// </summary>
    /// <param name="trackerCode"></param>
    /// <returns></returns>
    Task<MediaModel> GetMediaAsync(string trackerCode);

    /// <summary>
    /// Update Async
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<MediaStaging> UpdateAsync(MediaStaging entity);

    /// <summary>
    /// Sync Media Async
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> SyncMediaAsync(MediaSyncModel model);

    /// <summary>
    /// Promote Media
    /// </summary>
    /// <returns></returns>
    Task PromoteMedia();

    /// <summary>
    /// Insert Media Staging Async
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<MediaStaging> InsertMediaStagingAsync(MediaStaging entity);

    /// <summary>
    /// Insert Media File Async
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<MediaFile> InserMediaFileAsync(MediaFile entity);

    /// <summary>
    /// Get Media By HashId
    /// </summary>
    /// <param name="hashid"></param>
    /// <returns></returns>
    Task<MediaModel> GetMediaByHashId(string hashid);

    /// <summary>
    /// Get Media By File URL
    /// </summary>
    /// <param name="fileUrls"></param>
    /// <returns></returns>
    Task<Dictionary<string, string>> GetMediaByFileURL(List<string> fileUrls);

    /// <summary>
    /// Get Media
    /// </summary>
    /// <param name="trackerCode"></param>
    /// <returns></returns>
    Task<(Stream, string)> ViewMedia(string trackerCode);

    /// <summary>
    /// Get List Categories Async
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<string>> ListCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Browse files and folders in a category and path
    /// </summary>
    /// <param name="category"></param>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MediaBrowseResultModel> BrowseAsync(
        string category,
        string path = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Build category tree
    /// </summary>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MediaFolderNode> BuildCategoryTreeAsync(
        string category,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Delete file by key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task DeleteFileAsync(string key);

    /// <summary>
    /// Delete multiple files by keys
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    Task DeleteFilesAsync(List<string> keys);
}
