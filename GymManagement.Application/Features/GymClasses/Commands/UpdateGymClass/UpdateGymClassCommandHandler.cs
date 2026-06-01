using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.GymClasses.Commands.UpdateGymClass;

public class UpdateGymClassCommandHandler : IRequestHandler<UpdateGymClassCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGymClassCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateGymClassCommand request,
        CancellationToken cancellationToken)
    {
        var gymClass = await _unitOfWork.GymClasses.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (gymClass is null)
        {
            return Result.Failure("Gym class not found");
        }

        var trainer = await _unitOfWork.Trainers.GetByIdAsync(
            request.TrainerId,
            cancellationToken);

        if (trainer is null)
        {
            return Result.Failure("Trainer not found");
        }

        if (!trainer.IsActive)
        {
            return Result.Failure("Trainer is not active");
        }

        gymClass.Name = request.Name;
        gymClass.Description = request.Description;
        gymClass.TrainerId = request.TrainerId;
        gymClass.StartTime = request.StartTime;
        gymClass.EndTime = request.EndTime;
        gymClass.Capacity = request.Capacity;
        gymClass.IsActive = request.IsActive;
        gymClass.UpdatedAtUtc = DateTime.UtcNow;

        _unitOfWork.GymClasses.Update(gymClass);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Gym class updated successfully");
    }
}