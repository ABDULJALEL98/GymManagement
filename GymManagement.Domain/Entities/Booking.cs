using GymManagement.Domain.Common;
using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Entities;

public class Booking : AuditableEntity
{
    public Guid MemberId { get; set; }

    public Member Member { get; set; } = null!;

    public Guid GymClassId { get; set; }

    public GymClass GymClass { get; set; } = null!;

    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    public BookingStatus Status { get; set; } = BookingStatus.Pending;
}