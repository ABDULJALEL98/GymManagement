using GymManagement.Domain.Identity;

namespace GymManagement.Application.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}