using System.Net;
using Backend.Core.Enums;
using Backend.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Core.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AdminRoleInterceptorAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.HasRole(Role.Admin))
        {
            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            return;
        }

        base.OnActionExecuting(context);
    }
}