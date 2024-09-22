using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SampleProjectactual.Models;
using Stripe;
using System;
using System.Linq;
using System.Threading.Tasks;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public OrderController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public IActionResult Payment()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var cart = GetCart();
        if (!cart.Items.Any())
        {
            return RedirectToAction("GetProducts", "Product");
        }

        var model = new PaymentViewModel
        {
            TotalAmount = cart.Items.Sum(item => item.Quantity * item.Product.price),
            StripePublishableKey = _configuration["Stripe:PublishableKey"] // Pass the key to the view
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var cart = GetCart();

        try
        {
            // Stripe setup
            var options = new ChargeCreateOptions
            {
                Amount = (long)(model.TotalAmount * 100), // Stripe expects amount in cents
                Currency = "usd",
                Description = "Order Payment",
                Source = model.CardToken // Token from Stripe.js
            };

            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            // Create an order in the database
            var order = new Order
            {
                UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value,
                TotalAmount = model.TotalAmount,
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Reduce stock quantities
            foreach (var item in cart.Items)
            {
                var product = _context.Products.Single(p => p.pid == item.ProductId);
                product.quantity -= item.Quantity;
                _context.Products.Update(product);
            }
            _context.SaveChanges();

            // Clear the cart
            cart.Items.Clear();
            SaveCart(cart);

            TempData["SuccessMessage"] = "Payment successful! Thank you for your order.";

            return RedirectToAction("Confirmation");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Payment failed: {ex.Message}");
            return View("Payment", model);
        }
    }


    public IActionResult Confirmation()
    {
        var successMessage = TempData["SuccessMessage"];
        return View(model: successMessage);
    }


    private Cart GetCart()
    {
        var cartJson = HttpContext.Session.GetString("Cart");
        return string.IsNullOrEmpty(cartJson) ? new Cart() : JsonConvert.DeserializeObject<Cart>(cartJson);
    }

    private void SaveCart(Cart cart)
    {
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
    }
}
