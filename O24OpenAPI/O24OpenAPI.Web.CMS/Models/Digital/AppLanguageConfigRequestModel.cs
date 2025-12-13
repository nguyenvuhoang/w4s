namespace O24OpenAPI.Web.CMS.Models.Digital;

public class AppLanguageConfigRequestModel : BaseTransactionModel
{
    public string RequestChannel { get; set; } = string.Empty;
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public string Search { get; set; } = string.Empty;
    public string[] Languages { get; set; } = [];
}

public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int TotalItems { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => PageSize <= 0 ? 1 : (int)Math.Ceiling((double)TotalItems / PageSize);
    public bool HasPrev => PageIndex > 0;
    public bool HasNext => PageIndex + 1 < TotalPages;
}
public class LanguageRowDto
{
    public string KeyPath { get; set; } = "";
    public Dictionary<string, string> Values { get; set; } = [];
}
