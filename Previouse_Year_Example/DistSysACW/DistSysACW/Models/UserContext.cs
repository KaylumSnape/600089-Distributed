using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base()
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Log> logs { get; set; }
        //TODO: Task13

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer
                (
                @"Server=(localdb)\mssqllocaldb;Database=DistSysACW2;",
                options => options.EnableRetryOnFailure());
            base.OnConfiguring(optionsBuilder);
        }
    }
}