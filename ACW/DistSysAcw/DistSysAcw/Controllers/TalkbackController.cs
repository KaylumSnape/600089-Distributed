using System;
using DistSysAcw.Models;
using Microsoft.AspNetCore.Mvc;

namespace DistSysAcw.Controllers
{
    public class TalkbackController : BaseController
    {
        /// <summary>
        ///     Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="dbcontext">DbContext set as a service in Startup.cs and dependency injected</param>
        public TalkbackController(UserContext dbcontext) : base(dbcontext)
        {
        }

        #region TASK1

        // api/talkback/hello
        [ActionName("Hello")]
        [HttpGet]
        public ActionResult Get() // Use action result for one return type, Ok.
        {
            return Ok("Hello World"); // Ok creates a new OkObjectResult(200).
        }

        #endregion

        #region TASK2

        // api/talkback/sort?integers=2&integers=5&integers=8
        [ActionName("Sort")]
        [HttpGet]
        public ActionResult Get([FromQuery] string[] integers) // Accepting string[] so I can handle bad request.
        {
            if (integers == null) return Ok(new int[0]); // If no values are submitted.
            try // If submitted values are valid integers.
            {
                var integerArray = Array.ConvertAll(integers, int.Parse);
                Array.Sort(integerArray);
                return Ok(integerArray);
            }
            catch (Exception) // If submitted values are invalid, such as a char.
            {
                return BadRequest("BadRequest");
            }
        }

        #endregion
    }
}