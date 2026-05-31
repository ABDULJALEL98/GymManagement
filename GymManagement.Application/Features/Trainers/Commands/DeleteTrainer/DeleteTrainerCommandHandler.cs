using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Trainers.Commands.DeleteTrainer;

public class DeleteTrainerCommandHandler : IRequestHandler<DeleteTrainerCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTrainerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteTrainerCommand request,
        CancellationToken cancellationToken)
    {
        var trainer = await _unitOfWork.Trainers.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (trainer is null)
        {
            return Result.Failure("Trainer not found");
        }

        _unitOfWork.Trainers.Delete(trainer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Trainer deleted successfully");
    }
}