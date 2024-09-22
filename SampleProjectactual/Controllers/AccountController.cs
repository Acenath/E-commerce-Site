using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleProjectactual.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AccountController> _logger;
    public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = _context.Users.SingleOrDefault(u => u.email == email);

        if (user == null || user.passwordhash != HashPassword(password))
        {
            ViewBag.ErrorMessage = "Invalid login attempt.";
            return View();
        }

        // Set authentication cookie
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.email),
            new Claim(ClaimTypes.NameIdentifier, user.userid.ToString())
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string name, string email, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ModelState.AddModelError("", "Passwords do not match.");
            return View();
        }

        var user_validate = _context.Users.SingleOrDefault(u => u.email == email || u.name == name);

        if (user_validate != null)
        {
            ModelState.AddModelError("", "User already exists.");
            return View();
        }

        var address = new Address("missing", "missing", "missing");
        var user = new User
        {
            name = name,
            email = email,
            passwordhash = HashPassword(password),
            address = address
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        // Log out the user from the Cookies authentication scheme
        await HttpContext.SignOutAsync("Cookies");

        // If using OpenIdConnect or other authentication schemes, sign out as well
        await HttpContext.SignOutAsync("OpenIdConnect");

        // Explicitly delete any authentication-related cookies
        Response.Cookies.Delete(".AspNetCore.Cookies");
        Response.Cookies.Delete("ExternalProviderToken");

        // Optional: Add logging to confirm actions
        _logger.LogInformation("User logged out and cookies cleared.");

        // Redirect to the home page after successful logout
        return RedirectToAction("Index", "Home");
    }




    [HttpGet]
    public IActionResult Profile()
    {
        // Get the email of the currently logged-in user
        var email = User.Identity.Name;

        // Retrieve the user from the database
        var user = _context.Users.SingleOrDefault(u => u.email == email);

        // Pass the user to the view
        return View(user);  
    }
    private string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
