using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.GymClasses.Queries.GetGymClassById;

public class GetGymClassByIdQuery : IRequest<Result<GymClassDto>>
{
    public Guid Id { get; set; }

    public GetGymClassByIdQuery(Guid id)
    {
        Id = id;
    }
}