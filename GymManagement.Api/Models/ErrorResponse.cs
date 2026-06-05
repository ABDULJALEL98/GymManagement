namespace GymManagement.Api.Models;

public class ErrorResponse
{
    public bool IsSuccess { get; set; } = false;

    public string Message { get; set; } = string.Empty;

    public List<string> Errors { get; set; } = new();
}