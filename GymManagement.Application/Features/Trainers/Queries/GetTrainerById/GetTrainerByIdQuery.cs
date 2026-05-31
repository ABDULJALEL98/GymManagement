using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Trainers.Queries.GetTrainerById;

public class GetTrainerByIdQuery : IRequest<Result<TrainerDto>>
{
    public Guid Id { get; set; }

    public GetTrainerByIdQuery(Guid id)
    {
        Id = id;
    }
}