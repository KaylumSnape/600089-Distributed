using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DistSysAcw.Models
{
    // Every database table must have a key so that the database can identify each record as a unique data item.
    #region Task2
    public class User
    {
        
        public enum Roles
        {
            Admin,
            User
        }

        [Key] // Primary Key.
        public string ApiKey { get; set; }

        [Required] // Makes the column not nullable in sql.
        [MaxLength(50)] // Limit length of field, increases performance in SQL, more scalable.
        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public Roles Role { get; set; }

        public ICollection<Log> Logs { get; set; } // Collection of Logs.

        public User() { }
    }
    #endregion

    #region Task13
    public class Log
    {
        [Key] 
        public int LogId { get; set; } // Auto incremented by DB.

        [Required]
        [MaxLength(300)]
        public string LogString { get; set; } // Describes what happened.

        [Required]
        public DateTime LogDateTime { get; set; }

        public Log() { }

        public Log(string logString, DateTime logDateTime)
        {
            LogString = logString;
            LogDateTime = logDateTime;
        }
    }

    // When a user is deleted, copy logs over into this table with their ApiKey to identify them.
    public class LogArchive
    {
        [Key] 
        public int LogId { get; set; } 

        [Required]
        public string ApiKey { get; set; } // Link the log entry to a user.

        [Required]
        [MaxLength(300)]
        public string LogString { get; set; }

        [Required]
        public DateTime LogDateTime { get; set; }

        // The only way to create a LogArchive is with an existing log and apiKey.
        public LogArchive(int logId, string apiKey, string logString, DateTime logDateTime)
        {
            LogId = logId;
            ApiKey = apiKey;
            LogString = logString; 
            LogDateTime = logDateTime;
        }
    }
    #endregion

    // Methods which allow us to read from/write to the database.
    public static class UserDatabaseAccess
    {
        #region Task3
        // Create a new user in the database, return user object.
        public static User PostUser(UserContext dbContext, string userName)
        {
            var role = User.Roles.User;

            // If this is the first user in the user table, set role to Admin.
            if (!dbContext.Users.Any())
            {
                role = User.Roles.Admin;
            }

            var newUser = new User
            {
                ApiKey =  new Guid().ToString(),
                UserName = userName,
                Role = role,
                Logs = new Collection<Log>()
            };
            
            newUser.Logs.Add(LogAction(dbContext, $"PostUser: New {role} added to the system."));
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();

            return newUser;
        }

        // Check if a user with a given ApiKey string exists in the database, returning true or false.
        public static bool UserApiKeyExists(UserContext dbContext, string apiKey)
        {
            return dbContext.Users.FirstOrDefault(x => x.ApiKey == apiKey) != null;
        }

        // Check if a user with a given user name exists in the database, returning true or false.
        public static bool UserNameExists(UserContext dbContext, string userName)
        {
            return dbContext.Users.FirstOrDefault(x => x.UserName == userName) != null;
        }

        // Check if a user with a given ApiKey and UserName exists in the database, returning true or false.
        public static bool UserExists(UserContext dbContext, string apiKey, string userName)
        {
            return dbContext.Users.FirstOrDefault(x => x.ApiKey == apiKey && x.UserName == userName) != null;
        }

        // Return a user with a given ApiKey
        public static User GetUser(UserContext dbContext, string apiKey)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.ApiKey == apiKey);
            return user; // Returns null if no user found, so always null check when calling.
        }
        #endregion

        #region Task13
        // Create log of user activity.
        public static Log LogAction(UserContext dbContext, string logString)
        {
            var log = new Log
            {
                LogString = logString,
                LogDateTime = DateTime.Now
            };

            //dbContext.Logs.Add(log);
            // Maybe don't need this here, worried about saving while in the middle of other actions.
            //dbContext.SaveChanges();

            return log;
        }

        // Retrieve logs.
        //public static Collection<Log> GetLogs(UserContext dbContext, string apiKey)
        //{
        //    var logs = new Collection<Log>();
        //    var user = GetUser(dbContext, apiKey);

        //    foreach (var logId in user.Logs)
        //    {
        //        logs.Add(dbContext.Logs.FirstOrDefault(x => x.LogId == logId));
        //    }

        //    return logs;
        //}

        // When deleting a user, archive their logs in the LogArchive table.
        public static bool ArchiveUserLogs(UserContext dbContext, string apiKey)
        {

            var user = dbContext.Users
                .Include(l => l.Logs)
                .FirstOrDefault(u => u.ApiKey == apiKey);

            if (user == null) return false;

            foreach (var log in user.Logs)
            {
                dbContext.LogArchives.Add(new LogArchive(log.LogId, apiKey, log.LogString, log.LogDateTime));
            }
            dbContext.SaveChanges();
            return true;
        }
        #endregion
    }


}