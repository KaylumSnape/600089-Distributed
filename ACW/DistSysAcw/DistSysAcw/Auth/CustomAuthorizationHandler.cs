using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace DistSysAcw.Auth
{
    /// <summary>
    ///     Authorises clients by role.
    /// </summary>
    public class CustomAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        public CustomAuthorizationHandler(IHttpContextAccessor httpContext)
        {
            HttpContextAccessor = httpContext;
        }

        private IHttpContextAccessor HttpContextAccessor { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RolesAuthorizationRequirement requirement)
        {
            #region Task6

            // If the users identity has not been authenticated (from CustomAuthenticationHandler).
            if (context.User == null || !context.User.Identity.IsAuthenticated) return Task.CompletedTask;

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

            return Task.CompletedTask;

            #endregion
        }
    }
}