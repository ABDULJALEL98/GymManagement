using GymManagement.Domain.Common;

namespace GymManagement.Domain.Entities;

public class Trainer : AuditableEntity
{
    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string Specialization { get; set; } = string.Empty;

    public int YearsOfExperience { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<GymClass> GymClasses { get; set; } = new List<GymClass>();
}