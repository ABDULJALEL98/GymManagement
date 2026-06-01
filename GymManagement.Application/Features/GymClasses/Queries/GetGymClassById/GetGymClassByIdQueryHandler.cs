using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.GymClasses.Queries.GetGymClassById;

public class GetGymClassByIdQueryHandler : IRequestHandler<GetGymClassByIdQuery, Result<GymClassDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetGymClassByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GymClassDto>> Handle(
        GetGymClassByIdQuery request,
        CancellationToken cancellationToken)
    {
        var gymClass = await _unitOfWork.GymClasses
            .Query()
            .AsNoTracking()
            .Include(x => x.Trainer)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (gymClass is null)
        {
            return Result<GymClassDto>.Failure("Gym class not found");
        }

        var dto = new GymClassDto
        {
            Id = gymClass.Id,
            Name = gymClass.Name,
            Description = gymClass.Description,
            TrainerId = gymClass.TrainerId,
            TrainerName = gymClass.Trainer.FullName,
            StartTime = gymClass.StartTime,
            EndTime = gymClass.EndTime,
            Capacity = gymClass.Capacity,
            IsActive = gymClass.IsActive
        };

        return Result<GymClassDto>.Success(dto);
    }
}