using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace SampleProjectactual.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userid { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string email { get; set;}

        [Required]
        public string passwordhash { get; set;}

        public Address address { get; set;} 
    }
}
