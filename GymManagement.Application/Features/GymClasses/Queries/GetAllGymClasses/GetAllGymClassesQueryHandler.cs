using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.GymClasses.Queries.GetAllGymClasses;

public class GetAllGymClassesQueryHandler
    : IRequestHandler<GetAllGymClassesQuery, Result<PagedResult<GymClassDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllGymClassesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PagedResult<GymClassDto>>> Handle(
        GetAllGymClassesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.GymClasses
            .Query()
            .AsNoTracking()
            .Include(x => x.Trainer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.Trim();

            query = query.Where(x =>
                x.Name.Contains(searchTerm) ||
                (x.Description != null && x.Description.Contains(searchTerm)) ||
                x.Trainer.FullName.Contains(searchTerm));
        }

        if (request.TrainerId.HasValue)
        {
            query = query.Where(x => x.TrainerId == request.TrainerId.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(x => x.StartTime >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(x => x.StartTime <= request.ToDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var classes = await query
            .OrderByDescending(x => x.StartTime)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new GymClassDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                TrainerId = x.TrainerId,
                TrainerName = x.Trainer.FullName,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Capacity = x.Capacity,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);

        var pagedResult = PagedResult<GymClassDto>.Create(
            classes,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PagedResult<GymClassDto>>.Success(pagedResult);
    }
}