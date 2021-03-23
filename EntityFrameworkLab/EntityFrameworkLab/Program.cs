using System;
using System.Collections.Generic;

namespace EntityFrameworkLab
{
    class Program
    {
        // Need EF tool package. Look at packages in this project, I had to install 3.
        // Use Package Manager Console (View -> Other Windows -> Package Manager Console).
        // In that console, create migration file using the command Add-Migration <GiveAName>.
        // This is built from our Context class (It finds it for us based of naming conventions).
        // Acts like a commit, showing us what it plans to do to our database.
        // Then perform the changes using the command Update-Database.
        // -Verbose flag for more information.
        // View - SQL Server Object Explorer - To see our databases.
        // Right click > View data on a table. Hit refresh arrow to update view (or reopen).
        // When updating the objects/classes in code, we must get entity framework to update our
        // database, so that they are both in sync. This is done through Migrations.
        // In order to update the database to match our model we’ll have to make
        // a new migration that has the changes.        // .dbo MigrationsHistory table allows you to rollback & apply multiple migrations.
        // When renaming a property in your model/class, the column in the DB must also be updated.
        // Do a Migration, but edit the migration file before updating,
        // EF isn't smart enough and wants to drop the old column and data and then add a new one in place.
        // We don't want to lose our existing data.
        // New version of EF is smart enough, so this isn't an issue anymore.
        // It's still good to check the migration file before 'pushing' your changes to the DB.

        static void Main(string[] args)
        {
            // Remember using handles disposal of resources once we are finished with them.
            using (var ctx = new MyContext())
            {
                var addr = new Address()
                {
                    HouseNameOrNumber = "1077",
                    Street = "45 Worthing Street",
                    City = "Hull",
                    County = "Humberside",
                    Postcode = "HU51PD",
                    Country = "UK",
                    People = new List<Person>() };
                    var prsn = new Person()
                    {
                        FirstName = "Kaylum",
                        MiddleNames = "Jay",
                        LastName = "Snape",
                        DateOfBirth = new DateTime(2010, 10, 1),
                        Address = addr
                    };
                ctx.Addresses.Add(addr);
                ctx.People.Add(prsn);
                ctx.SaveChanges();

            }
        }
    }
}
