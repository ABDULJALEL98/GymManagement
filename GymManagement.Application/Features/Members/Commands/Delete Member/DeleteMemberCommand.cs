using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Members.Commands.DeleteMember;

public class DeleteMemberCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public DeleteMemberCommand(Guid id)
    {
        Id = id;
    }
}