using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.GymClasses.Queries.GetAllGymClasses;

public class GetAllGymClassesQuery : PagedRequest, IRequest<Result<PagedResult<GymClassDto>>>
{
    public Guid? TrainerId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}