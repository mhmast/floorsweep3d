using FloorSweep.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Web.Resource;
using System.Linq;

namespace FloorSweep.PathFinding.Api
{
    internal class AuthenticationFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attributes = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true).OfType<ScopeAttribute>();

            var scopes = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "scope")?.Value ??"";
            foreach (var a in attributes)
            {
                if(!a.Scopes.All(scopes.Contains))
                {
                    context.Result = new StatusCodeResult(403);
                }
            }

        }
    }
}