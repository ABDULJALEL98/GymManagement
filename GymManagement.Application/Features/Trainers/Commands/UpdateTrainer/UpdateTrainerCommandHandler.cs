using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Trainers.Commands.UpdateTrainer;

public class UpdateTrainerCommandHandler : IRequestHandler<UpdateTrainerCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTrainerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateTrainerCommand request,
        CancellationToken cancellationToken)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (trainer is null)
        {
            return Result.Failure("Trainer not found");
        }

        var phoneExists = await _unitOfWork.Trainers.AnyAsync(
            x => x.PhoneNumber == request.PhoneNumber && x.Id != request.Id,
            cancellationToken);

        if (phoneExists)
        {
            return Result.Failure("Phone number already exists");
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists = await _unitOfWork.Trainers.AnyAsync(
                x => x.Email == request.Email && x.Id != request.Id,
                cancellationToken);

            if (emailExists)
            {
                return Result.Failure("Email already exists");
            }
        }

        trainer.FullName = request.FullName;
        trainer.PhoneNumber = request.PhoneNumber;
        trainer.Email = request.Email;
        trainer.Specialization = request.Specialization;
        trainer.YearsOfExperience = request.YearsOfExperience;
        trainer.IsActive = request.IsActive;
        trainer.UpdatedAtUtc = DateTime.UtcNow;

        _unitOfWork.Trainers.Update(trainer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Trainer updated successfully");
    }
}