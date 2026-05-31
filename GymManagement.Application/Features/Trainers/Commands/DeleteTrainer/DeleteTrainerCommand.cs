using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Trainers.Commands.DeleteTrainer;

public class DeleteTrainerCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public DeleteTrainerCommand(Guid id)
    {
        Id = id;
    }
}