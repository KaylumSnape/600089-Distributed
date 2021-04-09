using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DistSysAcw.Models
{
    // Every database table must have a key so that the database can identify each record as a unique data item.
    public class User
    {
        #region Task2
        public enum Roles
        {
            Admin,
            User
        }
        [Key]
        public string ApiKey { get; set; } // Primary Key, signified by [key] attribute.
        public string UserName { get; set; }
        public Roles Role { get; set; }
        public User() { }
        #endregion
    }

    #region Task13?
    // TODO: You may find it useful to add code here for Logging
    #endregion

    // Methods which allow us to read from/write to the database.
    public static class UserDatabaseAccess
    {
        #region Task3
        // Create a new user in the database, return user object.
        public static User PostUser(UserContext dbContext, string userName)
        {
            var newUser = new User
            {
                ApiKey =  new Guid().ToString(),
                UserName = userName,
                Role = User.Roles.User
            };

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

        // Check if a user with a given ApiKey string exists in the database, returning true or false.
        public static User GetUser(UserContext dbContext, string apiKey)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.ApiKey == apiKey);
            return user ?? new User(); // If the user is null return a new empty user.
        }
        #endregion
    }


}