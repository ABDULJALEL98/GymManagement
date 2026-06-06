using GymManagement.Domain.Enums;

namespace GymManagement.Application.DTOs;

public class BookingsReportDto
{
    public Guid BookingId { get; set; }

    public string MemberName { get; set; } = string.Empty;

    public string GymClassName { get; set; } = string.Empty;

    public string TrainerName { get; set; } = string.Empty;

    public DateTime BookingDate { get; set; }

    public BookingStatus Status { get; set; }
}