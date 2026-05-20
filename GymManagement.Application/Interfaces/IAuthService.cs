using GymManagement.Application.Common.Models;

namespace GymManagement.Application.Interfaces;

public interface IAuthService
{
    Task<Result<string>> RegisterAsync(
        string fullName,
        string email,
        string password,
        string role);

    Task<Result<string>> LoginAsync(
        string email,
        string password);
}