using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        [Required] [MaxLength(20)] public Roles Role { get; set; }

        public ICollection<Log> Logs { get; set; } // Collection of Logs.
    }

    #endregion

    #region Task13

    public class Log
    {
        public Log()
        {
        }

        public Log(string logString, DateTime logDateTime)
        {
            LogString = logString;
            LogDateTime = logDateTime;
        }

        [Key] public int LogId { get; set; } // Auto incremented by DB.

        [Required] [MaxLength(300)] public string LogString { get; set; } // Describes what happened.

        [Required] public DateTime LogDateTime { get; set; }
    }

    // When a user is deleted, copy logs over into this table with their ApiKey to identify them.
    public class LogArchive
    {
        // Might need an empty constructor for EF.
        // The only way to create a LogArchive is with an existing log and apiKey.
        public LogArchive(int logId, string apiKey, string logString, DateTime logDateTime)
        {
            LogId = logId;
            ApiKey = apiKey;
            LogString = logString;
            LogDateTime = logDateTime;
        }

        [Key] public int LogArchiveId { get; set; } // Auto incremented by DB.

        [Required] public int LogId { get; set; } // Link the log entry to its original log.

        [Required] public string ApiKey { get; set; } // Link the log entry to a user.

        [Required] [MaxLength(300)] public string LogString { get; set; }

        [Required] public DateTime LogDateTime { get; set; }
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
            if (!dbContext.Users.Any()) role = User.Roles.Admin;

            var newUser = new User
            {
                ApiKey = Guid.NewGuid().ToString(),
                UserName = userName,
                Role = role,
                Logs = new List<Log>()
            };

            dbContext.Users.Add(newUser);

            // dbContext is saved when logging so no need to do it again.
            LogAction(dbContext, newUser,
                $"PostUser: New {role} {userName} added to the system.");

            return newUser;
        }

        // Check if a user with a given ApiKey string exists in the database, returning true or false.
        public static bool UserApiKeyExists(UserContext dbContext, string apiKey)
        {
            return dbContext.Users.FirstOrDefault(u => u.ApiKey == apiKey) != null;
        }

        // Check if a user with a given user name exists in the database, returning true or false.
        public static bool UserNameExists(UserContext dbContext, string userName)
        {
            return dbContext.Users.FirstOrDefault(u => u.UserName == userName) != null;
        }

        // Check if a user with a given ApiKey and UserName exists in the database, returning true or false.
        public static bool UserExists(UserContext dbContext, string apiKey, string userName)
        {
            return dbContext.Users.FirstOrDefault(u => u.ApiKey == apiKey && u.UserName == userName) != null;
        }

        // Return a user and their logs with a given ApiKey.
        // Returns null if no user found, so always null check when calling.
        public static User GetUser(UserContext dbContext, string? apiKey, string? username)
        {
            User user = null;

            // Just realised that when i get a user to add a log to them, this is why it's slow
            // I shouldn't get the logs for users when I just want to add a new log to it.
            // This is an easy performance improvement I'd have to make if scaling this.

            // When you query the DB, it will only return the table you ask for,
            // it won't return the linked tables unless you tell it too with .Include.
            if (apiKey != null)
                user = dbContext.Users
                    .Include(u => u.Logs)
                    .FirstOrDefault(u => u.ApiKey == apiKey);

            if (username != null)
                user = dbContext.Users
                    .Include(u => u.Logs)
                    .FirstOrDefault(u => u.UserName == username);

            return user;
        }

        #endregion

        #region Task 7

        // Remove a user.
        public static bool DeleteUser(UserContext dbContext, string apiKey, string username)
        {
            var user = GetUser(dbContext, apiKey, null);

            // If user does not exist or username does not match the supplied username.
            if (user == null || user?.UserName != username) return false;

            // Archive user logs, if unsuccessful return false.
            if (!ArchiveUserLogs(dbContext, user.ApiKey))
            {
                LogAction(dbContext, user,
                    $"DeleteUser: Failed to archive logs and remove user: {user.UserName}");

                return false;
            }

            // Remove user.
            dbContext.Users.Remove(user);

            dbContext.SaveChanges();
            return true;
        }

        #endregion

        #region TASK8

        // Change a users role.
        public static bool ChangeUserRole(UserContext dbContext, ChangeRole jsonChangeRole)
        {
            var user = GetUser(dbContext, null, jsonChangeRole.username);

            if (user == null) return false;

            // We know it's a valid enum because we check in the api route.
            // https://www.dotnetperls.com/enum-parse
            user.Role = Enum.Parse<User.Roles>(jsonChangeRole.role);

            LogAction(dbContext, user,
                $"ChangeUserRole: {jsonChangeRole.username} role has been changed to {jsonChangeRole.role}.");

            return true;
        }

        #endregion
        
        #region Task13

        // Create log of user activity.
        public static void LogAction(UserContext dbContext, User user, string logString)
        {
            var log = new Log
            {
                LogString = logString,
                LogDateTime = DateTime.Now
            };

            user.Logs.Add(log);
            dbContext.SaveChanges();
        }

        // When deleting a user, archive their logs in the LogArchive table.
        public static bool ArchiveUserLogs(UserContext dbContext, string apiKey)
        {
            var user = GetUser(dbContext, apiKey, null);

            // There should always be at least one log, PostUser.
            if (user?.Logs == null) return false; // Null check. 

            // Copy logs over from Log table to LogArchive, adding apiKey to identify the user.
            foreach (var log in user.Logs)
                dbContext.LogArchives.Add(new LogArchive(log.LogId, apiKey, log.LogString, log.LogDateTime));

            dbContext.SaveChanges();
            return true;
        }

        #endregion
    }

    // Model for change role json request.
    // https://andrewlock.net/model-binding-json-posts-in-asp-net-core/
    // The JSON object to change a users role.
    public class ChangeRole
    {
        public string username { get; set; }
        public string role { get; set; }
    }
}