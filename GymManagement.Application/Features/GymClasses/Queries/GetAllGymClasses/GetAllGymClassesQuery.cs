using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.GymClasses.Queries.GetAllGymClasses;

public class GetAllGymClassesQuery : IRequest<Result<List<GymClassDto>>>
{
}