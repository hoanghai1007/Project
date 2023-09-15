using Microsoft.AspNetCore.Mvc;

namespace BT1_project.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
