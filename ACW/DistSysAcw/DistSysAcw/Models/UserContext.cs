using Microsoft.EntityFrameworkCore;

namespace DistSysAcw.Models
{
    /* Context class is responsible for mapping entities (our objects)
    * to database tables, allowing us to perform queries like inserting,
    * updating, deleting in the BD, as well as managing data relationships. */
    public class UserContext : DbContext
    {
        public UserContext() : base() { }

        // DbSets to map.
        // LINQ queries against this object can be translated to a query ran on the DB.
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogArchive> LogArchives { get; set; }

        // Override the OnConfiguring method to set a connection string.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DistSysAcw;");
        }
    }
}