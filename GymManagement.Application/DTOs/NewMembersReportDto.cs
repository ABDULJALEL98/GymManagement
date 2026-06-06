namespace GymManagement.Application.DTOs;

public class NewMembersReportDto
{
    public Guid MemberId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public bool IsActive { get; set; }
}