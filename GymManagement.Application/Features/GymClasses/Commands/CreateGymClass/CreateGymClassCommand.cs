using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.GymClasses.Commands.CreateGymClass;

public class CreateGymClassCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid TrainerId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int Capacity { get; set; }

    public bool IsActive { get; set; } = true;
}