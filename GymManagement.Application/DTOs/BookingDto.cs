using GymManagement.Domain.Enums;

namespace GymManagement.Application.DTOs;

public class BookingDto
{
    public Guid Id { get; set; }

    public Guid MemberId { get; set; }

    public string MemberName { get; set; } = string.Empty;

    public Guid GymClassId { get; set; }

    public string GymClassName { get; set; } = string.Empty;

    public DateTime BookingDate { get; set; }

    public BookingStatus Status { get; set; }
}