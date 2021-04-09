using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace DistSysACW.Filters
{
    public class AuthFilter : AuthorizeAttribute, IAuthorizationFilter
    {
       
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                AuthorizeAttribute authAttribute = (AuthorizeAttribute)context.ActionDescriptor.EndpointMetadata.Where(e => e.GetType() == typeof(AuthorizeAttribute)).FirstOrDefault();
              
                if (authAttribute != null)
                {
                    //var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    //var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                    //var role_ = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();

                    string[] roles = authAttribute.Roles.Split(',');
                    foreach (string role in roles)
                    {
                        if (context.HttpContext.User.IsInRole(role) )
                        {
                             return;
                        }
                    }
                  throw new UnauthorizedAccessException();
                }
            }
            catch(Exception ex)
            {
                string val = ex.ToString();
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult("Unauthorized. Admin access only.");
            }
        }
    }
}
