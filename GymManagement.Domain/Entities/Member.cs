using GymManagement.Domain.Common;

namespace GymManagement.Domain.Entities;

public class Member : AuditableEntity
{
    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Address { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}