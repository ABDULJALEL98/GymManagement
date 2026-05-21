using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<string>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        return await _authService.RegisterAsync(
            request.FullName,
            request.Email,
            request.Password,
            request.Role);
    }
}