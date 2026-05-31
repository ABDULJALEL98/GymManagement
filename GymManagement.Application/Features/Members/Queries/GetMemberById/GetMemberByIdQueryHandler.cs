using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Members.Queries.GetMemberById;

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, Result<MemberDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMemberByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MemberDto>> Handle(
        GetMemberByIdQuery request,
        CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(request.Id, cancellationToken);

        if (member is null)
        {
            return Result<MemberDto>.Failure("Member not found");
        }

        var dto = new MemberDto
        {
            Id = member.Id,
            FullName = member.FullName,
            PhoneNumber = member.PhoneNumber,
            Email = member.Email,
            DateOfBirth = member.DateOfBirth,
            Address = member.Address,
            IsActive = member.IsActive
        };

        return Result<MemberDto>.Success(dto);
    }
}