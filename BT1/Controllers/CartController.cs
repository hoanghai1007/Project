using Microsoft.AspNetCore.Mvc;
using System.IO; //thao tac voi file, thu muc
using Newtonsoft.Json;//thao tac voi file json
using System.Data;//su dung DataTalbe, DataRow
using System.Data.SqlClient;//su dung SqlConnection, DataAdapter...
using X.PagedList;//su dung cac ham phan trang
using BC = BCrypt.Net;//doi tuong ma hoa csdl, gan doi tuong nay thanh BC
using BT1_project.Models;//nhan dien cac file trong thu muc Models
using System.Security.Cryptography;


namespace BT1_project.Controllers
{
	public class CartController : Controller
	{
		public MyDbContext db = new MyDbContext();
		public IActionResult Index()
		{
			//khai báo giỏ hàng
			List<Item> shopping_cart = new List<Item>();
			//lấy chuỗi json lưu ở session
			string json_cart = HttpContext.Session.GetString("cart");
			if (!string.IsNullOrEmpty(json_cart))
			{
				//convert chuỗi json thành list
				shopping_cart = JsonConvert.DeserializeObject<List<Item>>(json_cart);
			}
           
            return View(shopping_cart);
		}
		//thêm sản phẩm vaog giỏ hàng
		public IActionResult Buy(int id)
		{
			//gọi hàm CartAdd để thêm sản phẩm vào giỏ hngf
			Cart.CartAdd(HttpContext.Session, id);
			
			return Redirect("/Cart");

		}
		//xóa sp khỏi giỏ hàng
		public IActionResult Remove(int id)
		{
			//gọi hàm CartRemove để xóa sản phẩm khỏi giỏ hngf
			Cart.CartRemove(HttpContext.Session, id);
			return Redirect("/Cart");
		}
		//xóa toàn bộ giỏ hàng
		public IActionResult Destroy(int id)
		{
			//gọi hàm CartDestroy để thêm sản phẩm vào giỏ hngf
			Cart.CartDestroy(HttpContext.Session);
			return Redirect("/Cart");
		}
		//update giỏ hàng
		public IActionResult Update()
		{
			//khai báo giỏ hàng
			List<Item> shopping_cart = new List<Item>();
            //lấy chuỗi json lưu ở session
            string json_cart = HttpContext.Session.GetString("cart");
			if (!string.IsNullOrEmpty(json_cart))
			{
				//convert chuỗi json thành list
				shopping_cart = JsonConvert.DeserializeObject<List<Item>>(json_cart);
				
			}
			foreach (Item item_cart in shopping_cart)
			{
				string form_name = "product_" + item_cart.ProductRecord.Id;
				int new_quantity = Convert.ToInt32(Request.Form[form_name]);
				//goi hàm để update lại số lượng
				Cart.CartUpdate(HttpContext.Session, item_cart.ProductRecord.Id, new_quantity);
			}
            
            return Redirect("/Cart");
		}
		//mua hàng
		public IActionResult Checkout()
		{
			//kiểm tra nếu chưa đăng nhập thì di chuyển đến url đăng nhập
			if (String.IsNullOrEmpty(HttpContext.Session.GetString("customer_email")))
				return Redirect("/Account/Login");
			else
			{
				Cart.CartCheckOut(HttpContext.Session, Convert.ToInt32(HttpContext.Session.GetString("customer_id")));
				return Redirect("/Cart");
			}
		}
	}
}
