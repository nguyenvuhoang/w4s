using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.CMS.API.Application.Models.Media;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.API.Application.Services.Interfaces.Media;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.CMS.Infrastructure.Configurations;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Models.JwtModels;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Framework.Utils;
using System.Security.Cryptography;

namespace O24OpenAPI.CMS.API.Controllers;

public class MediaController(IMediaService mediaService, CMSSetting cmsSetting) : BaseController
{
    private readonly IMediaService _mediaService = mediaService;
    private readonly CMSSetting _cmsSetting = cmsSetting;

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        [FromQuery] string folder = null,
        string customercode = null
    )
    {
        ConsoleUtil.WriteInfo(
            $"UploadFile called with folder: {folder}, customercode: {customercode}"
        );
        Dictionary<string, string> header = Application.Utils.Utils.GetHeaders(HttpContext);

        string createdBy = null;
        bool isTokenValid = false;

        if (header.TryGetValue("uid", out string token))
        {
            IJwtTokenService? jwtTokenService = EngineContext.Current.Resolve<IJwtTokenService>();
            ValidateTokenResponseModel validateTokenResponse = jwtTokenService.ValidateToken(token);

            if (validateTokenResponse.IsValid)
            {
                isTokenValid = true;
                createdBy = validateTokenResponse.UserId;
            }
        }

        if (file == null || file.Length == 0)
            return BadRequest("File is empty.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp" };
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            return BadRequest("Invalid file format.");

        if (file.Length > 30 * 1024 * 1024)
            return BadRequest("File is too large.");

        // Read file to byte[]
        byte[] fileBytes;
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            fileBytes = ms.ToArray();
        }

        // Compute hash
        var hashBytes = SHA256.HashData(fileBytes);
        var fileHash = Convert.ToHexStringLower(hashBytes);

        // Check duplicate
        MediaModel existedFile = await _mediaService.GetMediaByHashId(fileHash);

        if (existedFile != null)
        {
            var existedTrackerCode = existedFile.TrackerCode;
            var existedS3Key = existedFile.FileUrl;
            var shortUrlExisting = $"{_cmsSetting.CMSURL?.TrimEnd('/')}/m/{existedTrackerCode}";

            return Ok(
                new
                {
                    s3Key = existedS3Key,
                    trackerCode = existedTrackerCode,
                    expiredOnUtc = existedFile.ExpiredOnUtc,
                    temp = false,
                    folderUsed = existedFile.FolderName,
                    reused = true,
                    fileUrl = shortUrlExisting,
                }
            );
        }

        // ============================
        //  UPLOAD TO S3
        // ============================

        IFileStorageService? storage = EngineContext.Current.Resolve<IFileStorageService>();
        var category = string.IsNullOrWhiteSpace(folder) ? "general" : folder.Trim();

        using var stream = new MemoryStream(fileBytes);

        FileUploadResult uploadResult = await storage.UploadAsync(
            stream,
            file.FileName,
            file.ContentType ?? "application/octet-stream",
            category,
            customercode
        );

        var s3Key = uploadResult.Key;

        var trackerCode = Guid.NewGuid().ToString("N");
        DateTime expiredOn = isTokenValid ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddMinutes(5);

        if (isTokenValid)
        {
            await _mediaService.InserMediaFileAsync(
                new MediaFile
                {
                    TrackerCode = trackerCode,
                    FileUrl = s3Key,
                    FileHash = fileHash,
                    FolderName = folder,
                    FileName = file.FileName,
                    FileExtension = extension,
                    FileSize = file.Length,
                    Status = "ACTIVE",
                    CreatedOnUtc = DateTime.UtcNow,
                    ExpiredOnUtc = expiredOn,
                    CreatedBy = createdBy,
                }
            );
        }
        else
        {
            await _mediaService.InsertMediaStagingAsync(
                new MediaStaging
                {
                    TrackerCode = trackerCode,
                    FileUrl = s3Key,
                    FileHash = fileHash,
                    FolderName = folder,
                    FileName = file.FileName,
                    FileExtension = extension,
                    FileSize = file.Length,
                    Status = "PENDING",
                    CreatedOnUtc = DateTime.UtcNow,
                    ExpiredOnUtc = expiredOn,
                    CreatedBy = createdBy,
                }
            );
        }

        var shortUrl = $"{_cmsSetting.CMSURL?.TrimEnd('/')}/m/{trackerCode}";

        return Ok(
            new
            {
                fileUrl = shortUrl,
                trackerCode,
                s3Key,
                expiredOnUtc = expiredOn,
                temp = !isTokenValid,
                folderUsed = folder,
            }
        );
    }

    [HttpGet("/m/{trackerCode}")]
    public async Task<IActionResult> ShortLink(
        string trackerCode,
        CancellationToken cancellationToken
    )
    {
        (Stream? stream, string? contentType) = await _mediaService.ViewMedia(trackerCode);

        if (stream == Stream.Null || contentType == null)
            return NotFound();

        return File(stream, contentType);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        IReadOnlyList<string> categories = await _mediaService.ListCategoriesAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpGet]
    public async Task<IActionResult> Browse(
        [FromQuery] string category,
        [FromQuery] string path,
        CancellationToken cancellationToken
    )
    {
        MediaBrowseResultModel result = await _mediaService.BrowseAsync(category, path, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetMediaTree(
        [FromQuery] string category,
        CancellationToken cancellationToken
    )
    {
        MediaFolderNode tree = await _mediaService.BuildCategoryTreeAsync(category, cancellationToken);
        return Ok(tree);
    }
}
