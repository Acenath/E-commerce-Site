using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SampleProjectactual.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }

    public class Cart
    {
        public int CartId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }


}
