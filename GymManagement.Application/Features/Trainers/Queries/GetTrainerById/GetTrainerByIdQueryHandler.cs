using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Trainers.Queries.GetTrainerById;

public class GetTrainerByIdQueryHandler : IRequestHandler<GetTrainerByIdQuery, Result<TrainerDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTrainerByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TrainerDto>> Handle(
        GetTrainerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (trainer is null)
        {
            return Result<TrainerDto>.Failure("Trainer not found");
        }

        var dto = new TrainerDto
        {
            Id = trainer.Id,
            FullName = trainer.FullName,
            PhoneNumber = trainer.PhoneNumber,
            Email = trainer.Email,
            Specialization = trainer.Specialization,
            YearsOfExperience = trainer.YearsOfExperience,
            IsActive = trainer.IsActive
        };

        return Result<TrainerDto>.Success(dto);
    }
}