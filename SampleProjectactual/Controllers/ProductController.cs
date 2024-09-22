using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SampleProjectactual.Models;
using System.Collections.Generic;
using System.Linq;
using SampleProjectactual.Extensions;


namespace SampleProjectactual.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult GetProducts()
        {
            // Fetch products from the database
            var products = _context.Products.ToList();
            return View(products); // Pass the list of products to the view
        }

    }
    public class CartProduct
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
