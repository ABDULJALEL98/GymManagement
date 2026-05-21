namespace GymManagement.Application.DTOs;

public class UpdateRolePermissionsRequest
{
    public List<string> Permissions { get; set; } = new();
}