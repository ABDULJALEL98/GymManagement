using System.Text.Json.Serialization;

namespace GymManagement.Application.Common.Models;

public class Result<T> : Result
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; set; }

    public static Result<T> Success(T data, string message = "Operation completed successfully")
    {
        return new Result<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }

    public static new Result<T> Failure(string message, List<string>? errors = null)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}