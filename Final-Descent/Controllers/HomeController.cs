using Microsoft.AspNetCore.Mvc;

namespace Final_Descent.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
