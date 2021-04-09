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
        /// <summary>
        /// This DbContext contains the database context defined in UserContext.cs
        /// You can use it inside your controllers to perform database CRUD functionality
        /// </summary>
        protected Models.UserContext DbContext { get; set; }
        public BaseController(Models.UserContext dbcontext)
        {
            DbContext = dbcontext;
        }
    }
}