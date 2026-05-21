using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace GymManagement.Infrastructure.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
    {
        Policy = $"{PermissionPolicyProvider.PolicyPrefix}{permission}";
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}