using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DistSysAcw.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace DistSysAcw.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserContext _dbContext; // Not to be changed by the controller.

        // Pass in UserContext through dependency injection.
        public UserController(Models.UserContext dbcontext) : base(dbcontext)
        {
            _dbContext = dbcontext;
        }

        #region TASK4
        // api/user/new?username=UserOne
        [HttpGet]
        [ActionName("New")]
        public ActionResult Get([FromQuery] string username)
        {
            // If the user exists, respond true, if not and also if string is empty, respond false.
            return Ok(UserDatabaseAccess.UserNameExists(_dbContext, username) ? "True - User Does Exist! Did you mean to do a POST to create a new user?" : "False - User Does Not Exist! Did you mean to do a POST to create a new user?");
        }

        // api/user/new
        [HttpPost]
        [ActionName("New")]
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

        #region TASK7
        // api/user/removeuser?username=UserOne
        [HttpDelete]
        [ActionName("RemoveUser")]
        // Authenticate who we are talking to in CustomAuthenticationHandler.
        // Verify the authentic user is authorised to carry out this action in CustomAuthorizationHandler.
        [Authorize(Roles = "Admin, User")] 
        public ActionResult Delete([FromHeader] string apiKey, [FromQuery] string username)
        {
            var user = UserDatabaseAccess.GetUser(_dbContext, apiKey, null);
            UserDatabaseAccess.LogAction(_dbContext, user,
                $"/user/removeuser called for {user.UserName}.");

            return Ok(UserDatabaseAccess.DeleteUser(_dbContext, apiKey, username));
        }
        #endregion

        #region TASK8
        // api/user/changerole
        [HttpPut] // Idempotent action
        [ActionName("ChangeRole")]
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRole([FromHeader] string apiKey, [FromBody] JsonChangeRole jsonRequest)
        {
            // User is already authenticated and authorised if they reach this point,
            // Still need their apiKey to add a log.

            var user = UserDatabaseAccess.GetUser(_dbContext, apiKey, null);
            UserDatabaseAccess.LogAction(_dbContext, user,
                $"/user/changerole called for {jsonRequest.username} by {user.UserName}");

            JsonChangeRole jsonChangeRole;

            try
            {
                jsonChangeRole = jsonRequest;
            }
            catch (Exception)
            {
                return BadRequest("NOT DONE: An error occurred");
            }
            
            // If username does not exist.
            if (!UserDatabaseAccess.UserNameExists(_dbContext, jsonChangeRole.username))
            {
                return BadRequest("NOT DONE: Username does not exist");
            }

            // If role does not exist.
            if (!Enum.IsDefined(typeof(User.Roles), jsonChangeRole.role))
            {
                return BadRequest("NOT DONE: Role does not exist");
            }

            // If success.
            if (UserDatabaseAccess.ChangeUserRole(_dbContext, jsonChangeRole))
            {
                return Ok("DONE");
            }

            return BadRequest("NOT DONE: An error occurred");
        }
        #endregion
    }

    // Model for change role json request.
    // https://andrewlock.net/model-binding-json-posts-in-asp-net-core/
    public class JsonChangeRole
    {
        public string username { get; set; }
        public string role { get; set; }
    }
}
