using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkLab
{
    /* Context class is responsible for mapping entities (our objects)
     * to database tables, allowing us to perform queries like inserting,
     * updating, deleting in the BD, as well as managing data relationships.
     */
    internal class MyContext : DbContext
    {
        public MyContext() : base() { }

        // DbSets to map.
        // LINQ queries against this object can be translated to a query ran on the DB.
        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }

        // Override the OnConfiguring method to set a connection string.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server = (localdb)\\mssqllocaldb; Database = EFLab;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}