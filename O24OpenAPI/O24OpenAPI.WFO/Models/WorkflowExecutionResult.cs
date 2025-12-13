namespace O24OpenAPI.WFO.Models;

public class WorkflowExecutionResult
{
    public bool IsSuccess { get; private set; }
    public string ExecutionId { get; private set; }
    public string ErrorMessage { get; private set; }
    public int StatusCode { get; private set; }

    private WorkflowExecutionResult() { }

    public static WorkflowExecutionResult Success(string executionId)
    {
        return new WorkflowExecutionResult
        {
            IsSuccess = true,
            ExecutionId = executionId,
            StatusCode = 200,
        };
    }

    public static WorkflowExecutionResult Failed(string error)
    {
        return new WorkflowExecutionResult
        {
            IsSuccess = false,
            ErrorMessage = error,
            StatusCode = 400,
        };
    }

    public static WorkflowExecutionResult NotFound(string error)
    {
        return new WorkflowExecutionResult
        {
            IsSuccess = false,
            ErrorMessage = error,
            StatusCode = 404,
        };
    }

    public static WorkflowExecutionResult Error(string error)
    {
        return new WorkflowExecutionResult
        {
            IsSuccess = false,
            ErrorMessage = error,
            StatusCode = 500,
        };
    }
}
