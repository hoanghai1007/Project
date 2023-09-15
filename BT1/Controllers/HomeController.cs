using Microsoft.AspNetCore.Mvc;

namespace BT1_project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
