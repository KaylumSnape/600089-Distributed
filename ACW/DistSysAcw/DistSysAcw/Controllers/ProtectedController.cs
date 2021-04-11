using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DistSysAcw.Models;
using Microsoft.AspNetCore.Authorization;
using static DistSysAcw.Cryptography.ShaCryptography;

namespace DistSysAcw.Controllers
{
    public class ProtectedController : BaseController
    {
        private readonly UserContext _dbContext; // Not to be changed by the controller.

        // Pass in UserContext through dependency injection.
        public ProtectedController(Models.UserContext dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        #region TASK9
        // api/protected/hello
        [HttpGet]
        [ActionName("Hello")]
        [Authorize(Roles = "Admin, User")]
        public ActionResult Get()
        {
            var apiKey = Request.Headers["ApiKey"];

            var user = UserDatabaseAccess.GetUser(_dbContext, apiKey, null);

            UserDatabaseAccess.LogAction(_dbContext, user,
                $"{user.UserName} requested /protected/hello.");

            return Ok($"Hello {user.UserName}");
        }

        // api/protected/sha1
        [HttpGet]
        [ActionName("SHA1")]
        [Authorize(Roles = "Admin, User")]
        public ActionResult GetSha1([FromQuery] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Bad Request");
            }

            var apiKey = Request.Headers["ApiKey"];

            var user = UserDatabaseAccess.GetUser(_dbContext, apiKey, null);

            UserDatabaseAccess.LogAction(_dbContext, user,
                $"{user.UserName} requested /protected/sha1.");
            
            return Ok(Sha1Encrypt(message));
        }

        // api/protected/sha256
        [HttpGet]
        [ActionName("SHA256")]
        [Authorize(Roles = "Admin, User")]
        public ActionResult GetSha256([FromQuery] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Bad Request");
            }

            var apiKey = Request.Headers["ApiKey"];

            var user = UserDatabaseAccess.GetUser(_dbContext, apiKey, null);

            UserDatabaseAccess.LogAction(_dbContext, user,
                $"{user.UserName} requested /protected/sha256.");

            return Ok(Sha256Encrypt(message));
        }
        #endregion
    }
}
