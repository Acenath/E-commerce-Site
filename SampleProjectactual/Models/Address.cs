using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleProjectactual.Models
{
    public class Address
    {

        // Constructor with parameters
        public Address(string country, string city, string street_address)
        {
            this.country = country;
            this.city = city;
            this.street_address = street_address;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int aid { get; set; }

        public string country { get; set; }

        public string city { get; set; }

        public string street_address { get; set; }

        public override string ToString()
        {
            return "Street Address: " + this.street_address + "City: " + this.city + "Country: " + this.country;
        }


    }
}
