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
    public class ProductsController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Category(int? id, int? page)
        {

            int _CategoryId = id ?? 0;
            //tạo biens để truyền ra ngoài view
            ViewBag.CategoryId = _CategoryId;
            //khai bao so ban ghi tren mot trang
            int pageSize = 9;
            //tinh so trang
            int pageNumber = page ?? 1;
            List<ItemProduct> list_record = (from item in db.Products orderby item.Id descending select item).ToList();
            //nếu id > 0 thì thực hiện điều kiện where -> có thể truy vấn linhq tiếp ỏ biến list_record

            if (_CategoryId > 0)
            {
                list_record = (from item_product in list_record join item_category_product in db.CategoriesProducts on item_product.Id equals item_category_product.ProductId join item_category in db.Categories on item_category_product.CategoryId equals item_category.Id where item_category.Id == _CategoryId select item_product).ToList();
            }
            //lấy biến order truyền từ url
            if (!String.IsNullOrEmpty(Request.Query["order"]))
            {
                string order = Request.Query["order"];
                switch (order)
                {
                    case "name-asc":
                        list_record = list_record.OrderByDescending(item => item.Name).ToList();
                        break;
                    case "price-asc":
                        list_record = list_record.OrderBy(item => item.Price).ToList();
                        break;
                    case "price-desc":
                        list_record = list_record.OrderByDescending(item => item.Price).ToList();
                        break;
                }
            }
            return View(list_record.ToPagedList(pageNumber, pageSize));
        }
        //chi tiết sản phẩm
        public IActionResult Detail(int? id)
        {
            int _id = id ?? 0;
            //lấy một bản ghi
            ItemProduct item_product = db.Products.Where(item => item.Id == _id).FirstOrDefault();
            return View(item_product);
        }
        //danh so sao san pham
        public IActionResult Rating(int? id)
        {
            int _id = id ?? 0;
            int star = Convert.ToInt32(Request.Query["star"]);
            //insert ban ghi vao table Rating
            ItemRating record = new ItemRating();
            record.ProductId = _id;
            record.Star = star;
            //---
            db.Add(record);
            db.SaveChanges();
            //---
            return Redirect("/Products/Detail/" + _id);
        }
        //tinh tu trai qua phai, tren url co bao nhieu bien thi co the truyen vao ham theo cach sau
        //VD: Search?fromPrice=1000000&toPrice=10000000
        public IActionResult Search(int? page)
        {
            int fromPrice = Convert.ToInt32(Request.Query["fromPrice"]);
            int toPrice = Convert.ToInt32(Request.Query["toPrice"]);
            //khai bao so ban ghi tren mot trang
            int pageSize = 5;
            //tinh so trang
            int pageNumber = page ?? 1;
            //---
            ViewBag.fromPrice = fromPrice;
            ViewBag.toPrice = toPrice;
            //---
            List<ItemProduct> list_products = db.Products.Where(item => item.Price >= fromPrice && item.Price <= toPrice).OrderByDescending(item => item.Id).ToList();
            return View("SearchPrice", list_products.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Tag(int? page)
        {
            int tag_id = Convert.ToInt32(Request.Query["tag_id"]);
            //khai bao so ban ghi tren mot trang
            int pageSize = 50;
            //tinh so trang
            int pageNumber = page ?? 1;
            //---
            List<ItemProduct> list_products = (from product in db.Products join tag_product in db.TagsProducts on product.Id equals tag_product.ProductId join tag in db.Tags on tag_product.TagId equals tag.Id where tag.Id == tag_id select product).ToList();
            return View("Tag", list_products.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult SearchName(int? page)
        {
            string key = Request.Query["key"];
            //khai bao so ban ghi tren mot trang
            int pageSize = 5;
            //tinh so trang
            int pageNumber = page ?? 1;
            //---
            ViewBag.key = key;
            //---
            List<ItemProduct> list_products = db.Products.Where(item => item.Name.Contains(key)).OrderByDescending(item => item.Id).ToList();
            return View("SearchName", list_products.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult AjaxSearch()
        {
            string key = Request.Query["key"];
            //---
            List<ItemProduct> list_products = db.Products.Where(item => item.Name.Contains(key)).OrderByDescending(item => item.Id).ToList();
            string str = "";
            foreach (var item in list_products)
            {
                str += "<li><a href='/Products/Detail/" + item.Id + "'><div><img src='/Upload/Products/" + item.Photo + "'><span>" + item.Name + "</span><p>" + item.Price + "$</p></div></a></li>";
            }

            //str += "<li><img src='/Upload/Products/" + item.Photo + "'> <a href='/Products/Detail/" + item.Id + "'>" + item.Name + "</a></li>";
        
            return Content(str);
        }

    }
}





