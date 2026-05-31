using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Members.Queries.GetAllMembers;

public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, Result<List<MemberDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllMembersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<MemberDto>>> Handle(
        GetAllMembersQuery request,
        CancellationToken cancellationToken)
    {
        var members = await _unitOfWork.Members.GetAllAsync(cancellationToken);

        var result = members
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new MemberDto
            {
                Id = x.Id,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                DateOfBirth = x.DateOfBirth,
                Address = x.Address,
                IsActive = x.IsActive
            })
            .ToList();

        return Result<List<MemberDto>>.Success(result);
    }
}