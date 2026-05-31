using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Members.Commands.UpdateMember;

public class UpdateMemberCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; }
}