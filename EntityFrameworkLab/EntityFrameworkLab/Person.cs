using System;

namespace EntityFrameworkLab
{
    public class Person
    {
        // Every database table must have a key so that the database can identify
        // each record as a unique data item. This property is a convention (ID),
        // so EF natively understands to make this the key for the people table.
        public int PersonID { get; set; } // Primary Key, signified by ID.
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
        public Person() { }
    }
}