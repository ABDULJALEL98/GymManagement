using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Trainers.Commands.CreateTrainer;

public class CreateTrainerCommand : IRequest<Result<Guid>>
{
    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string Specialization { get; set; } = string.Empty;

    public int YearsOfExperience { get; set; }

    public bool IsActive { get; set; } = true;
}