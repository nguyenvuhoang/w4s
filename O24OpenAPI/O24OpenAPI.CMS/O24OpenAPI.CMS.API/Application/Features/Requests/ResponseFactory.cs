using O24OpenAPI.CMS.API.Application.Models.Response;

namespace O24OpenAPI.CMS.API.Application.Features.Requests;

public static class ResponseFactory
{
    public static ResponseModel Success(
        object data = null,
        string message = "Success",
        string executionId = null
    )
    {
        return new ResponseModel
        {
            Success = true,
            Code = "success",
            Message = message,
            Data = data,
            ExecutionId = executionId ?? EngineContext.Current.Resolve<WorkContext>().ExecutionId,
        };
    }

    public static ResponseModel Error(
        string message,
        string code = "failed",
        List<ErrorInfoModel> errors = null,
        string executionId = null
    )
    {
        return new ResponseModel
        {
            Success = false,
            Code = code,
            Message = message,
            Errors = errors,
            ExecutionId = executionId ?? EngineContext.Current.Resolve<WorkContext>().ExecutionId,
        };
    }

    public static ResponseModel Error(List<ErrorInfoModel> errors, string executionId = null)
    {
        return new ResponseModel
        {
            Success = false,
            Code = "failed",
            Message = "Error occurred",
            Errors = errors,
            ExecutionId = executionId ?? EngineContext.Current.Resolve<WorkContext>().ExecutionId,
        };
    }

    public static ResponseModel Error(Exception exception, string executionId = null)
    {
        return new ResponseModel
        {
            Success = false,
            Code = "failed",
            Message = exception.Message,
            Errors = new List<ErrorInfoModel>
            {
                new ErrorInfoModel
                {
                    Type = "Exception",
                    TypeError = exception.GetType().ToString(),
                    Info = exception.Message,
                    Code = "exception",
                    Key = "exception",
                },
            },
            ExecutionId = executionId ?? EngineContext.Current.Resolve<WorkContext>().ExecutionId,
        };
    }
}
