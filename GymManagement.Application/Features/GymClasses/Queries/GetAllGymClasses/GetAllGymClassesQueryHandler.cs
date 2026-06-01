using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.GymClasses.Queries.GetAllGymClasses;

public class GetAllGymClassesQueryHandler : IRequestHandler<GetAllGymClassesQuery, Result<List<GymClassDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllGymClassesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<GymClassDto>>> Handle(
        GetAllGymClassesQuery request,
        CancellationToken cancellationToken)
    {
        var classes = await _unitOfWork.GymClasses
            .Query()
            .AsNoTracking()
            .Include(x => x.Trainer)
            .OrderByDescending(x => x.StartTime)
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

        return Result<List<GymClassDto>>.Success(classes);
    }
}