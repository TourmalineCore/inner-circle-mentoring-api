using System.Security.Claims;

namespace Api;

public static class UserExtensions
{
    private const string TenantIdClaimType = "tenantId";
    private const string EmployeeIdClaimType = "employeeId";

    public static long GetTenantId(this ClaimsPrincipal context)
    {
        return long.Parse(context.FindFirstValue(TenantIdClaimType));
    }

    public static long GetEmployeeId(this ClaimsPrincipal context)
    {
        return long.Parse(context.FindFirstValue(EmployeeIdClaimType));
    }
}
