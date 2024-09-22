using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleProjectactual.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; } // Primary Key

        [ForeignKey("Order")]
        public int OrderId { get; set; } // Foreign Key (from the Order entity)

        [ForeignKey("Product")]
        public int ProductId { get; set; } // Foreign Key (from the Product entity)

        public int Quantity { get; set; } // Quantity of the product ordered
        public decimal Price { get; set; } // Price of the product at the time of order

        // Navigation properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
