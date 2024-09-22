using System.ComponentModel.DataAnnotations;

namespace SampleProjectactual.Models
{
    public class Product
    {
        [Key]
        public int pid { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public int quantity { get; set; }
        [Required]
        public int price { get; set; }
        public string description { get; set; }

    }


}
