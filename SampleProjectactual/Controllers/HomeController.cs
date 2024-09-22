using Microsoft.AspNetCore.Mvc;

namespace SampleProjectactual.Controllers
{
    public class HomeController : Controller
    {
        //ViewResult
        public IActionResult Index() // Name can be changed instead Index you can write anything
        {
            return View("Index");
            // You can also use var result = View();
            // return result
        }

    }
}
