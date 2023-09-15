using BT1_project.Models;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt; // thư viện mã hoa password
namespace BT1_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        //khởi tạo đối tượng thao tác csdl
        public MyDbContext db = new MyDbContext();
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fa) 
        {
            string _Email = fa["Email"];
            string _Password = fa["Password"];
            //mã hóa password
            //_Password = BC.HashPassword(_Password);
            //return Content(_Password);
            //lấy một bản ghi tương ứng với email,password
            ItemUser record = db.Users.Where(item=>item.Email == _Email).FirstOrDefault();
            if (record != null) 
            {
                if(BC.Verify(_Password ,record.Password))
                {
                    //khởi tạo session Email
                    HttpContext.Session.SetString("admin_name", record.Name);
                    //khởi tạo session Email
                    HttpContext.Session.SetString("admin_email", record.Email);
                    //di chuyển đến url
                    return Redirect("/Admin/Home");
                }    
            }
            return Redirect("/Admin/Account/Login");
        }
        //Logout
        public IActionResult Logout()
        {
            //hủy session
            HttpContext.Session.Remove("admin_email");
            return Redirect("/Admin/Account/Login");
        }
    }
}
