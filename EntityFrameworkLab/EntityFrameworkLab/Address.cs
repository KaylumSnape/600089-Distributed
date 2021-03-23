using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkLab
{
    public class Address
    {
        // You can also identify a key using the [key] attribute.
        // The conventions namespace is needed to do this.
        [Key]
        public int AddressIdentifier { get; set; }
        public string HouseNameOrNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public ICollection<Person> People { get; set; }
        public Address() { }

    }
}