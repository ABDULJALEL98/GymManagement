using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using MediatR;

namespace GymManagement.Application.Features.GymClasses.Commands.CreateGymClass;

public class CreateGymClassCommandHandler : IRequestHandler<CreateGymClassCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGymClassCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateGymClassCommand request,
        CancellationToken cancellationToken)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(
            request.TrainerId,
            cancellationToken);

        if (trainer is null)
        {
            return Result<Guid>.Failure("Trainer not found");
        }

        if (!trainer.IsActive)
        {
            return Result<Guid>.Failure("Trainer is not active");
        }

        var gymClass = new GymClass
        {
            Name = request.Name,
            Description = request.Description,
            TrainerId = request.TrainerId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Capacity = request.Capacity,
            IsActive = request.IsActive
        };

        await _unitOfWork.GymClasses.AddAsync(gymClass, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(gymClass.Id, "Gym class created successfully");
    }
}