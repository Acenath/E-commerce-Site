using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SampleProjectactual.Models;
using System.Linq;

namespace SampleProjectactual.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(ApplicationDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Retrieves the cart from the session
        private Cart GetCart()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(cartJson) ? new Cart() : JsonConvert.DeserializeObject<Cart>(cartJson);
        }

        // Saves the cart to the session
        private void SaveCart(Cart cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            _logger.LogInformation("Saving cart to session: {CartJson}", cartJson);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }

        // Adds a product to the cart
        public IActionResult AddToCart(int productId)
        {
            _logger.LogInformation("AddToCart called with ProductId: {ProductId}", productId);

            var cart = GetCart();
            var product = _context.Products.SingleOrDefault(p => p.pid == productId);

            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found.", productId);
                return RedirectToAction("GetProducts", "Product");
            }

            if (product.quantity <= 0)
            {
                _logger.LogWarning("Product with ID {ProductId} has insufficient quantity.", productId);
                return RedirectToAction("GetProducts", "Product");
            }

            var cartItem = cart.Items.SingleOrDefault(item => item.ProductId == productId);
            if (cartItem == null)
            {
                cart.Items.Add(new CartItem { Product = product, ProductId = productId, Quantity = 1 });
            }
            else
            {
                cartItem.Quantity++;
            }

            product.quantity--;
            _context.Products.Update(product);
            _context.SaveChanges();

            SaveCart(cart);
            _logger.LogInformation("Product {ProductId} added to cart.", productId);

            return RedirectToAction("GetProducts", "Product");
        }

        // View the cart
        public IActionResult ViewCart()
        {
            var cart = GetCart();
            ViewBag.CartCount = cart.Items.Count;
            return View(cart);
        }

        // Remove an item from the cart
        public IActionResult RemoveFromCart(int productId)
        {
            _logger.LogInformation("RemoveFromCart called with ProductId: {ProductId}", productId);

            var cart = GetCart();
            var cartItem = cart.Items.SingleOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                var product = _context.Products.SingleOrDefault(p => p.pid == productId);
                if (product != null)
                {
                    product.quantity += cartItem.Quantity;
                    _context.Products.Update(product);
                    _context.SaveChanges();
                }

                cart.Items.Remove(cartItem);
                SaveCart(cart);
                _logger.LogInformation("Product {ProductId} removed from cart.", productId);
            }
            else
            {
                _logger.LogWarning("Product with ID {ProductId} not found in cart.", productId);
            }

            return RedirectToAction("ViewCart");
        }

        // Clear the cart
        [HttpPost]
        public IActionResult ClearCart()
        {
            _logger.LogInformation("ClearCart called.");

            var cart = GetCart();
            foreach (var cartItem in cart.Items)
            {
                var product = _context.Products.SingleOrDefault(p => p.pid == cartItem.ProductId);
                if (product != null)
                {
                    product.quantity += cartItem.Quantity;
                    _context.Products.Update(product);
                }
            }

            _context.SaveChanges();
            cart.Items.Clear(); // Remove all items from the cart
            SaveCart(cart);

            _logger.LogInformation("Cart cleared and product quantities restored.");
            return RedirectToAction("ViewCart");
        }

        // Checkout action
        public IActionResult Checkout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["RedirectUrl"] = Url.Action("Checkout", "Cart");
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("ProcessPayment", "Order");
        }
    }
}
