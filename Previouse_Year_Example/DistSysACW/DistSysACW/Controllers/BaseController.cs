using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistSysACW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Threading;

namespace DistSysACW.Controllers
{

    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        public static dynamic Dynamic;
        //private UserController fooManager = new UserController();
        protected readonly Models.UserContext _context;
        //store admin api-key and user api-key?
        //---------------------------------------------api key storage in client for authentication----------------------------------------------------------------------------------------//
        public List<int> admin_api = new List<int>();  //STORE ADMIN API KEY to client 
        public List<int> user_api = new List<int>();   //STORE USER API KEY  to client 
        public UserContext context;

        protected BaseController(UserContext context)
        {
            this.context = context;
        }
        //--------------------------------------------------APIKEYS STORED TO CLIENT.NEED TO PLUG INTO HEADER--------------------------------------------//
        public void obtain_keys()
        {
            var _keys = context.Users;

            foreach (Models.User user in _keys)
            {
                //I just wanna obtain admin and user api key
                if ("Admin" == user.role)
                    admin_api.Add(user.api_key);
                else if ("user" == user.role)
                    user_api.Add(user.api_key);
            }
        }
        //-----------------------------------searches database for user(TASK 3 DONE)---------------------------------------------------//
        public string search_username(string name)
        {
            string _bool = "";
            string str = "";
            var _names = context.Users;
            int exist = 0;
            string api = null;
            //I just wanna know if user exists or nah
            foreach (Models.User user in _names)
            {
                str = name;
                if (str == user.user_name)
                {
                    exist += 1;
                    api = Convert.ToString(user.api_key);
                }
                else
                {
                    _bool = "False - User Does Not Exist! Did you mean to do a POST to create a new user?";
                }
            }
            if (exist > 0)
            {
                _bool = "True - User Does Exist! Did you mean to do a POST to create a new user?";
            }
            else
            {
                _bool = "False - User Does Not Exist! Did you mean to do a POST to create a new user?";
            }

            return _bool;
        }
        //-----------------------------------------protected hello function-----------------------------------------------//
        public string encrypt_SHA1(string message)
        {
            DecryptorClass.Decrptor decrptor = new DecryptorClass.Decrptor();
            byte[] asciiByteMessage = decrptor.string_to_ascii(message);
            byte[] sha1ByteMessage = decrptor.SHA1_Encrypt(asciiByteMessage);
            return decrptor.ByteArrayToHexString(sha1ByteMessage);
        }
        //--------------------------------------encrypt 256----------------------------------------------------------------//
        public string encrypt_SHA256(string message)
        {
            DecryptorClass.Decrptor decrptor = new DecryptorClass.Decrptor();
            byte[] asciiByteMessage = decrptor.string_to_ascii(message);
            byte[] sha256ByteMessage = decrptor.SHA256_Encrypt(asciiByteMessage);
            return decrptor.ByteArrayToHexString(sha256ByteMessage);
        }

        //-------------------------------------TASK 4 ADD USERNAME TO DATABASE--------------------------------------------//

        public string add_user(string name)
        {
            string admin_cap = null;                                                                      //if no one is in the database
            List<string> admin_cap_role = new List<string>();                                            //stores all roles
            string role_determiner = null;                                                                //will return "null" if no admin role found
            string rslt = null;
            if (name == "" || name == null)
                rslt = "Empty";
            var _names = context.Users;

            foreach (Models.User user in _names)
            {
                string str = name;
                admin_cap = user.user_name;
                admin_cap_role.Add(user.role);
                if (str == user.user_name)
                    rslt = "Taken";
            }
            role_determiner = admin_cap_role.Find(x => x.Contains("Admin"));                            //will return "null" if no admin role found
            //search admin cap role for admin

            if (admin_cap == null || role_determiner == null)                                             //if there are no people in the database or all users in the database are signed in as users
            {
                using (Models.UserContext _context = new Models.UserContext())
                {
                    var _logs = new List<Models.Log>
                    {
                        new Log{Log_string = "First System Signup", LogDateTime = DateTime.Now},
                        new Log{Log_string = "Test data", LogDateTime = DateTime.Now},
                        new Log{Log_string = "Test data 3", LogDateTime = DateTime.Now}
                    };
                    //for adding.....
                    Models.User user = new Models.User()
                    {
                        user_name = name, //grap from post func
                        Logs = _logs,
                        role = "Admin"


                    };
                    
                    rslt = Convert.ToString(user.api_key);
                    _context.Users.Add(user);
                    _context.logs.AddRange(_logs);
                    _context.SaveChanges();
                }
            }


            //condition for user
            if (rslt == null && admin_cap != null)                                                             //if result is empty and there are already people in the database
            {
                using (Models.UserContext _context = new Models.UserContext())
                {
                    var _logs = new List<Models.Log>
                    {
                        new Log{Log_string = "First System Signup", LogDateTime = DateTime.Now},
                      
                    };
                    //for adding.....
                    Models.User user = new Models.User()
                    {
                        user_name = name, //grap from post func
                        Logs = _logs,
                        role = "user"
                    };
                    rslt = Convert.ToString(user.api_key);
                    _context.Users.Add(user);
                    _context.logs.AddRange(_logs);
                    _context.SaveChanges();

                    foreach (Models.User user1 in _names)
                    {
                        //get stored api key
                        string str = name;
                        admin_cap_role.Add(user.role);
                        if (name == user.user_name)
                            rslt = user.api_key.ToString();
                    }
                }
                // rslt = "Saved";
            }
            return rslt;
        }

        //------------------------------------UPDATING ROLE FUNCTION. WORKS------------------------------------//
        public string update_role_to_admin(string name)
        {
            string rslt = "";
            var _names = context.Users;
            foreach (Models.User user in _names)
            {
                string str = name;
                if (str == user.user_name)
                    rslt = "Taken";
            }

            if (rslt != "")
            {
                var _cntxt = new Models.UserContext();
                var t_name = _cntxt.Users.Where(u => u.user_name == name).First();
                t_name.role = "Admin";
                _cntxt.SaveChanges();
                rslt = "Changed";
            }
            else
            {
                rslt = "Unavailable";
            }
            return rslt;


        }
        //------------------------------------------------UPDATE TO USER----------------------------------------------------//

        public string update_role_to_user(string name)
        {
            var _cntxt = new Models.UserContext();
            var t_name = _cntxt.Users.Where(u => u.user_name == name).First();
            t_name.role = "User";
            _cntxt.SaveChanges();
            return "Changed";
        }


        //----------------------------------DELETE ROLE FUNCTION. WORKS BUT PARTIALLY IMPLEMENTED------------------------------------------------------------------//
        public string delete_user(string name)
        {
            // db.Database.Log = Console.WriteLine;
            var _cntxt = new Models.UserContext();
            var t_name = _cntxt.Users.Where(u => u.user_name == name).First();
            _cntxt.Users.Remove(t_name);
            _cntxt.SaveChanges();
            return "deleted";
        }
        //---------------------------------GENERATE PUBLIC KEY-------------------//
        
        public dynamic generate_public_key()
        {
                var publicKeyXml =Middleware.AuthMiddleware.publicxml;
                return publicKeyXml;
        }
        //--------------------------------GENERATE PRIVATE KEY------------------//
        public dynamic generate_private_key()
        { 
            var privateKeyXml = CoreExtensions.RSACryptoExtensions.ToXmlStringCore22(Middleware.AuthMiddleware.rsaServer, true);
            return privateKeyXml;
        }
        //-------------------------------update log---------------------------------------
        public void update_log(string name, string action)
        {
           // var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
           // var name__ = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            var _logs = new List<Models.Log>
                    {
                        new Log{Log_string = action, LogDateTime = DateTime.Now},

                    };
          
            var _cntxt = new Models.UserContext();
            var t_name = _cntxt.Users.Where(u => u.user_name == name).First();
            t_name.Logs = _logs;
            _cntxt.SaveChanges();
        }
    }
}