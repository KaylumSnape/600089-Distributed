using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistSysAcw.Auth
{
    /// <summary>
    /// Authorises clients by role
    /// </summary>
    public class CustomAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private IHttpContextAccessor HttpContextAccessor { get; set; }

        public CustomAuthorizationHandler(IHttpContextAccessor httpContext)
        {
            HttpContextAccessor = httpContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            #region Task6
            // TODO:  Modify the server's behaviour so that, when the action requires a user to be in Admin role ONLY 
            // (e.g. [Authorize(Roles = "Admin")]) and the user does not have the Admin role, you return a Forbidden status (403) 
            // with the message: "Forbidden. Admin access only."
            
            // If the user is not null and it's identity has been authenticated (from CustomAuthenticationHandler).
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                // If the user is in the required role to perform the action it's asking for.
                if (requirement.AllowedRoles.Any(role => context.User.IsInRole(role)))
                {
                    // Mark requirement as succeeded, they have been authorised.
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            
            // If the user is not in the required role, they are not authorised.
            context.Fail();
            HttpContextAccessor.HttpContext.Response.StatusCode = 403;
            HttpContextAccessor.HttpContext.Response.WriteAsync(JsonSerializer.Serialize("Forbidden. Admin access only."));
            
            return Task.CompletedTask;
            #endregion
        }
    }
}