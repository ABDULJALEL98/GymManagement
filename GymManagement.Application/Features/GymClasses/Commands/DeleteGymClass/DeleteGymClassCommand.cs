using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.GymClasses.Commands.DeleteGymClass;

public class DeleteGymClassCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public DeleteGymClassCommand(Guid id)
    {
        Id = id;
    }
}