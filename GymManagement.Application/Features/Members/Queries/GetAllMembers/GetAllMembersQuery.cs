using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Members.Queries.GetAllMembers;

public class GetAllMembersQuery : IRequest<Result<List<MemberDto>>>
{
}