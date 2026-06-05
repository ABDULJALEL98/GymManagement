using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Trainers.Queries.GetAllTrainers;

public class GetAllTrainersQueryHandler
    : IRequestHandler<GetAllTrainersQuery, Result<PagedResult<TrainerDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTrainersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PagedResult<TrainerDto>>> Handle(
        GetAllTrainersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Trainers
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

        if (!string.IsNullOrWhiteSpace(request.Specialization))
        {
            var specialization = request.Specialization.Trim();

            query = query.Where(x => x.Specialization.Contains(specialization));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var trainers = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new TrainerDto
            {
                Id = x.Id,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                Specialization = x.Specialization,
                YearsOfExperience = x.YearsOfExperience,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);

        var pagedResult = PagedResult<TrainerDto>.Create(
            trainers,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PagedResult<TrainerDto>>.Success(pagedResult);
    }
}