using O24OpenAPI.CMS.API.Application.Models.Media;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface IFileStorageService
{
    public Task<FileUploadResult> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string category = "general",
        string customerCode = null,
        CancellationToken cancellationToken = default
    );

    Task DeleteAsync(string key, CancellationToken cancellationToken = default);

    string GetFileUrl(string key, TimeSpan? expiresIn = null);
    Task<(Stream Stream, string ContentType)> GetAsync(
        string key,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// List categories
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<string>> ListCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Browse files and folders
    /// </summary>
    /// <param name="listTrackerCodeMedia"></param>
    /// <param name="category"></param>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MediaBrowseResultModel> BrowseAsync(
        Dictionary<string, string> listTrackerCodeMedia,
        string category,
        string path = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Build category tree
    /// </summary>
    /// <param name="listTrackerCodeMedia"></param>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MediaFolderNode> BuildCategoryTreeAsync(
        Dictionary<string, string> listTrackerCodeMedia,
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
