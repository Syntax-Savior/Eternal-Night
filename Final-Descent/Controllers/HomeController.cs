using Microsoft.AspNetCore.Mvc;

namespace Final_Descent.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";

            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");

            return View();
        }
    }
}
