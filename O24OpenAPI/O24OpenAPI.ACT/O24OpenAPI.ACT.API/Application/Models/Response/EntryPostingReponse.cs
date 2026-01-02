using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.ACT.Common;
using O24OpenAPI.ACT.API.Application.Models;

namespace O24OpenAPI.ACT.API.Application.Models.Response;

public class EntryPostingReponse : BaseO24OpenAPIModel
{
    public EntryPostingReponse()
    {
        this.EntryJournals = new List<TemporaryPosting>() { };
        this.ErrorEntryJournals = new List<TemporaryPosting>() { };
    }

    public List<TemporaryPosting> ErrorEntryJournals { get; set; }
    public List<TemporaryPosting> EntryJournals { get; set; }

    /// <summary>
    /// ErrorCode
    /// </summary>
    public string ErrorCode { get; set; } = Constants.PostingErrorCode.New;

    /// <summary>
    /// IsReverse
    /// </summary>
    public bool IsReverse { get; set; } = false;

    /// <summary>
    /// ErrorMessage
    /// </summary>
    public string ErrorMessage { get; set; }
}
