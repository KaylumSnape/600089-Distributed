using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading;
using System.Security.Cryptography;

namespace DistSysACW.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProtectedController : BaseController
    {
        public ProtectedController(Models.UserContext context) : base(context)
        {

        }
        
        // GET: api/Protected/5
        [HttpGet]
        [ActionName("hello")]
        [Authorize(Roles = "Admin,user")]
        public ActionResult Get()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            var role_ = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
            string message = name;
            string msg = "";
            if (message == null || message == "")
            {
                this.Response.StatusCode = 400;
                msg = "BAD REQUEST";
            }
            else
            {
                this.Response.StatusCode = 200;
                msg = "Hello " + name;
                update_log(name, "User Requested Protected Hello");
            }
            return new ObjectResult(msg);
        }
        [HttpGet]
        [ActionName("sha1")]
        [Authorize(Roles = "Admin,user")]
        public ActionResult security1([FromQuery]string message)
        {
            string msg = "";
            if (message == null || message == "")
            {
                this.Response.StatusCode = 400;
                msg = "BAD REQUEST";
            }
            else
            {

                msg = encrypt_SHA1(message);
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                update_log(name, "User Requested Protected SHA1");
            }
            return new ObjectResult(msg);
        }
        [HttpGet]
        [ActionName("sha256")]
        [Authorize(Roles = "Admin,user")]
        public ActionResult security2([FromQuery]string message)
        {
            string msg = "";
            if(message == null || message == "")
            {
                this.Response.StatusCode = 400;
                msg = "BAD REQUEST";
            }
            else
            {
                msg = encrypt_SHA256(message);
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                update_log(name, "User Requested Protected SHA256");
            }
            return new ObjectResult(msg);
        }
        [HttpGet]
        [ActionName("GetPublicey")]
        [Authorize(Roles = "Admin,user")]
        public ActionResult GetPublicKey()
        {
            //var rsaServer = new RSACryptoServiceProvider(1024);
            //var publicKeyXml = CoreExtensions.RSACryptoExtensions.ToXmlStringCore22(rsaServer, false);
            //var rsaClient = new RSACryptoServiceProvider(1024);
            //rsaClient.FromXmlString(publicKeyXml);
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            update_log(name, "User Requested Public Key");
            return new ObjectResult(generate_public_key());

        }
        [HttpGet]
        [ActionName("sign")]
        [Authorize(Roles = "Admin,user")]
        public ActionResult Signing([FromQuery]string message)
        {
            DecryptorClass.Decrptor decrptor = new DecryptorClass.Decrptor();
            byte[]data = decrptor.string_to_ascii(message);
            var pKey = generate_private_key();
            byte[] signed_data = decrptor.HashAndSignBytes(data, pKey);//sha1
            var hex_return = decrptor.ByteArrayToHexString(signed_data);
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            update_log(name, "User Signed a message using private key");
            return new ObjectResult(hex_return);
        }
    }
}
