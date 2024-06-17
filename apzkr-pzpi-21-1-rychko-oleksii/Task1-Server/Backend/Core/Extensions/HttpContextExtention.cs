using Backend.Core.Entities;
using Backend.Core.Enums;

namespace Backend.Core.Extensions;

public static class HttpContextExtensions
{
    public static bool HasRole(this HttpContext httpContext, Role role)
    {
        var identity = httpContext.Items["User"] as User;
        return identity?.Role == role || identity?.Role == Role.Admin;
    }
}