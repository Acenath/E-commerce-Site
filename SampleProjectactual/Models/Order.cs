using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SampleProjectactual.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; } // Primary Key

        public string UserId { get; set; } // Foreign Key (from the User entity)
        public decimal TotalAmount { get; set; } // Total amount for the order
        public DateTime OrderDate { get; set; } = DateTime.Now; // Date when the order was placed

        // Navigation properties
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
