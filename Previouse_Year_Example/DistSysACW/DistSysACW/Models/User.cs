using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Models
{
    public class User
    {
        #region Task2
        // TODO: Create a User Class for use with Entity Framework
        // Note that you can use the [key] attribute to set your ApiKey Guid as the primary key 
        #endregion
       [Key]
        public int api_key { get; set; }
        public string user_name { get; set; }
        public ICollection<Log> Logs { get; set; }

        public string role { get; set; }

        public User() { }

        //public int log_data { get; set; }
        //should be on same table...will fix this later
    }

    #region Task13?
    // TODO: You may find it useful to add code here for Logging
    #endregion
    public class Log
    {
        [Key]
        public int LogID { get; set; }
        public string Log_string { get; set; }  //describes what user did
        public DateTime LogDateTime { get; set; }

        public Log() { }
    }

    public static class UserDatabaseAccess
    {
        #region Task3 
        // TODO: Make methods which allow us to read from/write to the database 
        #endregion
    }


}