using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<Result<string>>
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}