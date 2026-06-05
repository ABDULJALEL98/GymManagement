using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Members.Queries.GetAllMembers;

public class GetAllMembersQueryHandler
    : IRequestHandler<GetAllMembersQuery, Result<PagedResult<MemberDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllMembersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PagedResult<MemberDto>>> Handle(
        GetAllMembersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Members
            .Query()
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.Trim();

            query = query.Where(x =>
                x.FullName.Contains(searchTerm) ||
                x.PhoneNumber.Contains(searchTerm) ||
                (x.Email != null && x.Email.Contains(searchTerm)));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var members = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
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
            .ToListAsync(cancellationToken);

        var pagedResult = PagedResult<MemberDto>.Create(
            members,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PagedResult<MemberDto>>.Success(pagedResult);
    }
}