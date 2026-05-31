namespace GymManagement.Application.DTOs;

public class MemberDto
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; }
}