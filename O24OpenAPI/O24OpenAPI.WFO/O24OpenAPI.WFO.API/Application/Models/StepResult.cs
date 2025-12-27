namespace O24OpenAPI.WFO.API.Application.Models;

public class StepResult
{
    public bool IsSuccess { get; private set; }
    public object Data { get; private set; }
    public string ErrorMessage { get; private set; }

    private StepResult() { }

    public static StepResult Success(object data = null)
    {
        return new StepResult { IsSuccess = true, Data = data };
    }

    public static StepResult Failed(string error)
    {
        return new StepResult { IsSuccess = false, ErrorMessage = error };
    }
}
