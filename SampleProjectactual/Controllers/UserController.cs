// Controller/UserController.cs
using Microsoft.AspNetCore.Mvc;
using SampleProjectactual.Models;
using System.Linq;

namespace SampleProjectactual.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action to display all users
        public IActionResult GetUser()
        {
            // Retrieve all users from the database
            var users = _context.Users.ToList(); // Ensure this is not null
            return View(users); // Pass the list to the view
        }
    }
}
