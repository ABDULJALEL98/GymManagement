using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Trainers.Queries.GetAllTrainers;

public class GetAllTrainersQuery : IRequest<Result<List<TrainerDto>>>
{
}