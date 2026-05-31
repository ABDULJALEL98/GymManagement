using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using MediatR;

namespace GymManagement.Application.Features.Trainers.Commands.CreateTrainer;

public class CreateTrainerCommandHandler : IRequestHandler<CreateTrainerCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTrainerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateTrainerCommand request,
        CancellationToken cancellationToken)
    {
        var phoneExists = await _unitOfWork.Trainers.AnyAsync(
            x => x.PhoneNumber == request.PhoneNumber,
            cancellationToken);

        if (phoneExists)
        {
            return Result<Guid>.Failure("Phone number already exists");
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists = await _unitOfWork.Trainers.AnyAsync(
                x => x.Email == request.Email,
                cancellationToken);

            if (emailExists)
            {
                return Result<Guid>.Failure("Email already exists");
            }
        }

        var trainer = new Trainer
        {
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Specialization = request.Specialization,
            YearsOfExperience = request.YearsOfExperience,
            IsActive = request.IsActive
        };

        await _unitOfWork.Trainers.AddAsync(trainer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(trainer.Id, "Trainer created successfully");
    }
}