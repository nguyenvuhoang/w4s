namespace O24OpenAPI.CMS.API.Application.Models.ContextModels;

/// <summary>
///
/// </summary>
public class ErrorStatusModel
{
    int i;

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    public ErrorStatusModel(int s)
    {
        i = s;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public int GetCode()
    {
        return i;
    }
}

/// <summary>
///
/// </summary>
public enum ErrorStatus
{
    /// <summary>
    ///
    /// </summary>
    noError = 0, // 0 Không có lỗi

    /// <summary>
    ///
    /// </summary>
    successLoadData = 1, // thành công

    /// <summary>
    ///
    /// </summary>
    errorLoadData = 11, // Lỗi khi load

    // các mã chạy trong 2 và liền kề
    /// <summary>
    ///
    /// </summary>
    successSave = 2, // thành công

    /// <summary>
    ///
    /// </summary>
    errorSave = 21, // Lỗi khi lưu =thêm

    /// <summary>
    ///
    /// </summary>
    errorDuplicateData = 22,

    /// <summary>
    ///
    /// </summary>
    ///         // các mã chạy trong 2 và liền kề
    successUpdate = 3, // thành công

    /// <summary>
    ///
    /// </summary>
    errorUpdate = 31, // Lỗi khi sửa

    /// <summary>
    ///
    /// </summary>
    errorNotExistUpdate = 32, // Không tồn tại khi update

    // các mã chạy trong 2 và liền kề
    /// <summary>
    ///
    /// </summary>
    successDelete = 4, // thành công

    /// <summary>
    ///
    /// </summary>
    errorDelete = 41, // Lỗi khi xóa

    /// <summary>
    ///
    /// </summary>
    errorNotExistDelete = 42, // Lỗi khi xóa

    /// <summary>
    ///
    /// </summary>
    // các mã chạy trong 2 và liền kề
    successView = 5, // thành công

    /// <summary>
    ///
    /// </summary>
    errorView = 51, // Lỗi khi xem

    /// <summary>
    ///
    /// </summary>
    // các mã chạy trong 2 và liền kề
    // phòng khi có gọi lên server, nếu không có thì chỉ chạy fo thôi

    successClear = 6, // thành công

    /// <summary>
    ///
    /// </summary>
    errorClear = 61, // Lỗi khi xem

    /// <summary>
    ///
    /// </summary>
    successUploadFile = 7, // thành công

    /// <summary>
    ///
    /// </summary>
    errorUploadFile = 71, // Lỗi khi xem

    /// <summary>
    ///
    /// </summary>
    successDownloadFile = 8, // thành công

    /// <summary>
    ///
    /// </summary>
    errorDownloadFile = 81, // Lỗi khi xem
}
