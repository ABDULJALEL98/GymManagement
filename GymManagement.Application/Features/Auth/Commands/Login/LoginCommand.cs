using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<Result<string>>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}