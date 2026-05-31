using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Trainers.Queries.GetAllTrainers;

public class GetAllTrainersQueryHandler : IRequestHandler<GetAllTrainersQuery, Result<List<TrainerDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTrainersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<TrainerDto>>> Handle(
        GetAllTrainersQuery request,
        CancellationToken cancellationToken)
    {
        var trainers = await _unitOfWork.Trainers.GetAllAsync(cancellationToken);

        var result = trainers
            .OrderByDescending(x => x.CreatedAtUtc)
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
            .ToList();

        return Result<List<TrainerDto>>.Success(result);
    }
}