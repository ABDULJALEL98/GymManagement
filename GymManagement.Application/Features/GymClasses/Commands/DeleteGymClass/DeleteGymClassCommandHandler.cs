using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.GymClasses.Commands.DeleteGymClass;

public class DeleteGymClassCommandHandler : IRequestHandler<DeleteGymClassCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGymClassCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteGymClassCommand request,
        CancellationToken cancellationToken)
    {
        var gymClass = await _unitOfWork.GymClasses.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (gymClass is null)
        {
            return Result.Failure("Gym class not found");
        }

        _unitOfWork.GymClasses.Delete(gymClass);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Gym class deleted successfully");
    }
}