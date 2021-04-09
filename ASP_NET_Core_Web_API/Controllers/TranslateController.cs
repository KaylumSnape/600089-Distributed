using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects,
// visit https://go.microsoft.com/fwlink/?LinkID=397860 
// https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-5.0
// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio
// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-5.0
// Particularly useful for ACW.
// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client

namespace ASP_NET_Core_Web_API.Controllers
{
    // Tells the framework this is a controller containing endpoints.
    [ApiController]
    // The path to these endpoints.
    // [controller] by convention is the controller class name minus the "Controller" suffix.
    // [action] attribute tells route to pay attention to action names (GetString, GetName).
    [Route("[controller]/[action]")]
    public class TranslateController : ControllerBase
    {
        // Remember <TranslateController> is this class, the actual route is translate.
        // GET: api/<TranslateController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // Tells the framework that when there is a parameter in the path, it should route to 
        // this endpoint rather than the other ‘get’ method above that doesn’t expect a parameter.
        // Integer id mapped from the URI as a parameter.
        // Because we have two endpoints with the same name, we want to explicitly state that
        // ANYTHING that can be cast to an integer should go to this method
        // and everything else should go to the other.
        // GET api/<TranslateController>/5
        [HttpGet("{id:int}", Name = "getInt")]
        public string GetInt(int id)
        {
            id += 100;
            return $"Your number plus 100 is {id}";
        }

        // We can differentiate our endpoints further by giving them a name attribute.
        [HttpGet("{input}", Name = "getString")]
        public string GetString(string input)
        {
            return $"You sent the string {input}";
        }

        // To have multiple endpoints that take the same type, we have to use action names (GetName).
        // Remember HTTP action names/words are the GET/SET/POST/DELETE
        [HttpGet("{name}", Name = "getName")]
        public string GetName(string name)
        {
            return $"Your name is {name}";
        }

        // As long as there is no other member with this (Get) action name,
        // we can also specify the action name with its attribute.
        [ActionName("GetAge")]
        [HttpGet("{age:int}", Name = "getAge")]
        public string Get(int age)
        {
            return $"You are {age} years old.";
        }


        // [FromQuery] attribute explicitly tells the API that we are expecting a parameter in the URI.
        // This is specified with ?name=myname
        // ttp://domain.com/path/?paramName=value&secondParamName=secondValue
        // ttp://localhost:26850/translate/getnamefromquery/?name=BigDaddy
        [HttpGet]
        public string GetNameFromQuery([FromQuery]string name)
        {
            return $"Your name is {name}";
        }


        // HTTP request packet has a HEADER and a BODY, the [FromBody] attribute
        // tells us to expect a specified value type.

        // POST api/<TranslateController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TranslateController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TranslateController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
