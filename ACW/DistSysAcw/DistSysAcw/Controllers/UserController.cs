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
        #endregion
    }
}
