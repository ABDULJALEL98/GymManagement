namespace GymManagement.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<string> Errors { get; set; } = new();

    public static Result Success(string message = "Operation completed successfully")
    {
        return new Result
        {
            IsSuccess = true,
            Message = message
        };
    }

    public static Result Failure(string message, List<string>? errors = null)
    {
        return new Result
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}