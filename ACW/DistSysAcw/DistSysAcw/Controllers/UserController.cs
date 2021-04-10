using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistSysAcw.Models;

namespace DistSysAcw.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserContext _dbContext;

        // Pass in UserContext through dependency injection.
        public UserController(Models.UserContext dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        #region TASK4
        // api/user/new?username=UserOne
        [ActionName("New")]
        [HttpGet]
        public ActionResult Get([FromQuery] string username)
        {
            // If the user exists, respond true, if not and also if string is empty, respond false.
            return Ok(UserDatabaseAccess.UserNameExists(_dbContext, username) ? "True - User Does Exist! Did you mean to do a POST to create a new user?" : "False - User Does Not Exist! Did you mean to do a POST to create a new user?");
        }

        // api/user/new
        [ActionName("New")]
        [HttpPost]
        public ActionResult Post([FromBody] string jsonUsername)
        {
            // If there is no string submitted in the body, from body fails. Otherwise, If the string is empty.
            if (string.IsNullOrWhiteSpace(jsonUsername))
            {
                return BadRequest("Oops. Make sure your body contains a string with your username and your Content-Type is Content-Type:application/json");
            }

            // If the username is already taken.
            if (UserDatabaseAccess.UserNameExists(_dbContext, jsonUsername))
            {
                return StatusCode(403,"Oops. This username is already in use. Please try again with a new username.");
            }

            // Create a new user and return the ApiKey as a string to the client.
            var newUser = UserDatabaseAccess.PostUser(_dbContext, jsonUsername);
            return Ok(newUser.ApiKey);
        }
        #endregion
    }
}
