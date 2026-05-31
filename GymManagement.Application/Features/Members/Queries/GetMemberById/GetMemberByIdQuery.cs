using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Members.Queries.GetMemberById;

public class GetMemberByIdQuery : IRequest<Result<MemberDto>>
{
    public Guid Id { get; set; }

    public GetMemberByIdQuery(Guid id)
    {
        Id = id;
    }
}