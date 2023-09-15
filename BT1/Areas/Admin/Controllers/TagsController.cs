using Microsoft.AspNetCore.Mvc;
using System.IO; //thao tac voi file, thu muc
using Newtonsoft.Json;//thao tac voi file json
using System.Data;//su dung DataTalbe, DataRow
using System.Data.SqlClient;//su dung SqlConnection, DataAdapter...
using X.PagedList;//su dung cac ham phan trang
using BC = BCrypt.Net;//doi tuong ma hoa csdl, gan doi tuong nay thanh BC
using BT1_project.Models;//nhan dien cac file trong thu muc Models
using System.Security.Cryptography;
using BT1_project.Areas.Admin.Attributes;

namespace BT1_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class TagsController : Controller
    {
        //khởi tạo csdl
        public MyDbContext db = new MyDbContext();
        public IActionResult Index(int? page)
        {
            //khai báo số bản ghi tteen một trang
            int pageSize = 6;
            //tính số trang
            int pageNumber = page ?? 1;//nếu page=null thì PageNumber = 1; nếu Page != null thì PageNumber = page
            List<ItemTag> list_record = (from item in db.Tags orderby item.Id descending select item).ToList();
            return View(list_record.ToPagedList(pageNumber, pageSize));
        }
        //update
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            //khai bao bien action de dua vao tham so action cua the form
            ViewBag.action = "/Admin/Tags/UpdatePost/" + _id;
            //lay mot ban ghi
            var record = db.Tags.Where(item => item.Id == _id).FirstOrDefault();
            return View("CreateUpdate", record);
        }
        //update Post
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            //lay mot ban ghi
            var record = db.Tags.Where(item => item.Id == _id).FirstOrDefault();
            //---
            //ham .Trim() de cat khoang trang ben trai va ben phai cua chuoi
            string _Name = fc["Name"].ToString().Trim();//cac dung IFormCollection
            string _Description = Request.Form["Description"].ToString().Trim();//cach dung Request.Form
            string _Content = Request.Form["Content"].ToString().Trim();
            int _Hot = !String.IsNullOrEmpty(Request.Form["Hot"]) ? 1 : 0;
            if (record != null)
            {
                //---
                record.Name = _Name;
                //lay thong tin cua file
                string _Photo = "";
                try
                {
                    _Photo = Request.Form.Files[0].FileName;
                }
                catch {; }
                if (!string.IsNullOrEmpty(_Photo))
                {
                    //upload anh moi
                    var timestamp = DateTime.Now.ToFileTime();//chuyen thoi gian ra thanh mot so nguyen
                    _Photo = timestamp + "_" + _Photo;//noi chuoi thoi gian vao bien _Photo
                    //lay duong dan cua file
                    //Path.Combine(duongdan1,duongdan2...) noi duongdan1 va duongdan2... thanh mot duong dan
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Tags", _Photo);
                    //upload file
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                }
                //cập nhật csdl
                db.SaveChanges();
            }
            //---
            return RedirectToAction("Index");
        }
        //create
        public IActionResult Create()
        {
            //khai báo biến action để đưa vào tham số action của thẻ form
            ViewBag.action = "/Admin/Tags/CreatePost";
            return View("CreateUpdate");
        }
        //create post
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            //tạo một bản ghi
            ItemTag record = new ItemTag();
            //---
            //ham .Trim() de cat khoang trang ben trai va ben phai cua chuoi
            string _Name = fc["Name"].ToString().Trim();//cac dung IFormCollection
            string _Description = Request.Form["Description"].ToString().Trim();//cach dung Request.Form
            string _Content = Request.Form["Content"].ToString().Trim();
            int _Hot = !String.IsNullOrEmpty(Request.Form["Hot"]) ? 1 : 0;
            if (record != null)
            {
                //---
                record.Name = _Name;
                //lay thong tin cua file
                string _Photo = "";
                try
                {
                    _Photo = Request.Form.Files[0].FileName;
                }
                catch {; }
                if (!string.IsNullOrEmpty(_Photo))
                {
                    //upload anh moi
                    var timestamp = DateTime.Now.ToFileTime();//chuyen thoi gian ra thanh mot so nguyen
                    _Photo = timestamp + "_" + _Photo;//noi chuoi thoi gian vao bien _Photo
                    //lay duong dan cua file
                    //Path.Combine(duongdan1,duongdan2...) noi duongdan1 va duongdan2... thanh mot duong dan
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Tags", _Photo);
                    //upload file
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream)
;
                    }
                }
                //thêm bản ghi vào table
                db.Tags.Add(record);
                //cập nhật csdl
                db.SaveChanges();
            }
            //---
            return RedirectToAction("Index");
        }
        //delete
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //Lấy một bản ghi
            ItemTag record = db.Tags.Where(item => item.Id == _id).FirstOrDefault();
            // xoá bản ghi này
            db.Tags.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

