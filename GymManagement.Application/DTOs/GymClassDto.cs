namespace GymManagement.Application.DTOs;

public class GymClassDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid TrainerId { get; set; }

    public string TrainerName { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int Capacity { get; set; }

    public bool IsActive { get; set; }
}