namespace GymManagement.Application.DTOs;

public class DashboardStatsDto
{
    public int TotalMembers { get; set; }

    public int ActiveMembers { get; set; }

    public int TotalTrainers { get; set; }

    public int ActiveTrainers { get; set; }

    public int UpcomingGymClasses { get; set; }

    public int ActiveSubscriptions { get; set; }

    public int TotalBookings { get; set; }

    public decimal TotalPaidAmount { get; set; }

    public List<RecentBookingDto> RecentBookings { get; set; } = new();

    public List<RecentPaymentDto> RecentPayments { get; set; } = new();
}