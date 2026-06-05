using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Trainers.Queries.GetAllTrainers;

public class GetAllTrainersQuery : PagedRequest, IRequest<Result<PagedResult<TrainerDto>>>
{
    public bool? IsActive { get; set; }

    public string? Specialization { get; set; }
}