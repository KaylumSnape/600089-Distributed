using DistSysAcw.Models;
using Microsoft.AspNetCore.Mvc;

namespace DistSysAcw.Controllers
{
    [ApiController] // Tells the framework this is a controller containing endpoints.
    // Route is the path to these endpoints.
    // [controller] by convention is the controller class name minus the "Controller" suffix.
    // [action] attribute tells route to pay attention to action names.
    [Route("api/[Controller]/[Action]")]
    public abstract class BaseController : ControllerBase
    {
        public BaseController(UserContext dbcontext)
        {
            DbContext = dbcontext;
        }

        /// <summary>
        ///     This DbContext contains the database context defined in UserContext.cs
        ///     You can use it inside your controllers to perform database CRUD functionality
        /// </summary>
        protected UserContext DbContext { get; set; }
    }
}