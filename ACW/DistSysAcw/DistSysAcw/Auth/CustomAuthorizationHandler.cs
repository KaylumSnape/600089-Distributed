using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistSysAcw.Auth
{
    /// <summary>
    /// Authorises clients by role.
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

                // If the user is not in the required role, they are not authorised.
                context.Fail();
                HttpContextAccessor.HttpContext.Response.StatusCode = 403;
                HttpContextAccessor.HttpContext.Response.WriteAsync("Forbidden. Admin access only.");
            }
            // TODO: Mention in report that I was setting 403 response outside of authentication check,
            // so I just moved it inside so it only sets the 403 response when the user is not in the
            // correct role (fails authorisation), leaving the response free to be set to 401 when
            // failing authentication.

            // Also mention that I was returning a JsonSerializer.Serialize("Forbidden. Admin access only.")
            // Meaning that the response included the "", I just returned the text instead.

            return Task.CompletedTask;
            #endregion
        }
    }
}