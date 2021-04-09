using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Threading;
using System.Security.Cryptography;
using System.Text;
using CoreExtensions;

namespace DistSysACW.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        public static int count = 0;  //public key init
        public static dynamic publicxml;
        //public static dynamic rsaServer = new RSACryptoServiceProvider(1024);
        public  static RSACryptoServiceProvider rsaServer = new RSACryptoServiceProvider();
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Models.UserContext dbContext)
        {
            //Checks if api-key from header is valid-----------------------------------------------------------------//
           
            string test = "";
            test = context.Request.Headers["apikey"].ToString();   //grabs specified header from request header
            if (test != "")
            {
                int _key = int.Parse(test);
                //Grab db model
                Models.UserContext userContext = new Models.UserContext();
                var _role = userContext.Users;
                string operator_ = "";             //username
                string designation_ = "";          //role
                foreach (Models.User user in _role)
                {
                    if (_key == user.api_key)
                    {
                        //obtain relevant user and role
                        operator_ = user.user_name;
                        designation_ = user.role;

                    }
                }
                // ClaimTypes.Name = operator_;
                if(operator_ != "" && designation_ != "")
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, operator_));
                    claims.Add(new Claim(ClaimTypes.Role, designation_));
                    claims.Add(new Claim(ClaimTypes.Hash, ""));
                    claims.Add(new Claim(ClaimTypes.Authentication, context.Request.Headers["apikey"]));
                    claims.Add(new Claim(ClaimTypes.Thumbprint, context.Request.Headers["apikey"]));

                    var identity = new ClaimsIdentity(claims);
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    Thread.CurrentPrincipal = claimsPrincipal;
                    context.User = claimsPrincipal; //Assigns principal to current HTTP context(I was just missing this lol)
                    //used to get current claims and principles
                    count += 1;
                }
                if (count < 2)
                {
                    
                    var publicKeyXml = CoreExtensions.RSACryptoExtensions.ToXmlStringCore22(rsaServer, false);
                    publicxml = publicKeyXml;
                }
            }
            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
