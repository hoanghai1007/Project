using Microsoft.AspNetCore.Mvc;
//nhìn thấy các file .cs bên trong folder Attributes
using BT1_project.Areas.Admin.Attributes;

namespace BT1_project.Areas.Admin.Controllers
{
    //bắt buộc phải có 
    [Area("Admin")]
    //kiểm tra xem User đã đăng nhập chưa
    [CheckLogin]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
