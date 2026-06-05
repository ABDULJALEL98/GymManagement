using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Members.Queries.GetAllMembers;

public class GetAllMembersQuery : PagedRequest, IRequest<Result<PagedResult<MemberDto>>>
{
    public bool? IsActive { get; set; }
}