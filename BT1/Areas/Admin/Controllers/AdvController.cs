using Microsoft.AspNetCore.Mvc;
using System.IO; //thao tac voi file, thu muc
using Newtonsoft.Json;//thao tac voi file json
using System.Data;//su dung DataTalbe, DataRow
using System.Data.SqlClient;//su dung SqlConnection, DataAdapter...
using X.PagedList;//su dung cac ham phan trang
using BC = BCrypt.Net;//doi tuong ma hoa csdl, gan doi tuong nay thanh BC
using BT1_project.Models;//nhan dien cac file trong thu muc Models
using System.Security.Cryptography;
using BT1_project.Areas.Admin.Attributes;//de nhin thay class CheckLogin.cs

namespace BT1_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[CheckLogin]
    public class AdvController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index(int? page)
        {
            //quy dinh so ban ghi tren mot trang
            int pageSize = 20;
            //lay bien page truyen tu url
            int pageNumber = page ?? 1;
            //lay tat ca cac ban ghi trong bang Adv
            List<ItemAdv> list_record = db.Adv.OrderByDescending(item => item.Id).ToList();
            //return Content(_ListRecord.Count.ToString());
            //truyen gia tri ra view co phan trang
            return View("Index", list_record.ToPagedList(pageNumber, pageSize));
        }
        #region Update
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            ViewBag.action = "/Admin/Adv/UpdatePost/" + _id;
            //lay mot ban ghi
            ItemAdv record = db.Adv.Where(item => item.Id == _id).FirstOrDefault();
            //goi view, truyen du lieu ra view
            return View("CreateUpdate", record);
        }

        // UpdatePosst
        [HttpPost]
        public IActionResult UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            string _Name = fc["Name"].ToString().Trim();
            int _Position = Convert.ToInt32(fc["Position"].ToString().Trim());
            //---
            //lay ban ghi tuong ung voi id truyen vao
            var record = db.Adv.Where(item => item.Id == _id).FirstOrDefault();
            if (record != null)
            {
                //---
                record.Name = _Name;
                record.Position = _Position;
                //---
                //lay thong tin o the file co type="file"
                string _FileName = "";
                try
                {
                    //lay ten file
                    _FileName = Request.Form.Files[0].FileName;
                }
                catch {; }
                if (!String.IsNullOrEmpty(_FileName))
                {
                    //xoa anh cu
                    if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Adv", record.Photo)))
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Adv", record.Photo));
                    }

                    //upload anh moi

                    //lay thoi gian gan vao ten file -> tranh cac file co ten trung ten voi file se upload
                    var timestap = DateTime.Now.ToFileTime();
                    _FileName = timestap + "_" + _FileName;
                    //lay duong dan cua file
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", _FileName);
                    //upload file
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                    //update gia tri vao cot Photo trong csdl
                    record.Photo = _FileName;
                }
                //---
                //cap nhat ban ghi
                db.SaveChanges();
                //---
            }
            return RedirectToAction("Index");
        }
        #endregion
        //create
        public IActionResult Create()
        {
            ViewBag.action = "/Admin/Adv/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            int _Position = Convert.ToInt32(fc["Position"].ToString().Trim());
            //---
            //Khởi tạo 1 bản ghi mới
            ItemAdv record = new ItemAdv();
            // gán giá trị cho bản ghi
            record.Name = _Name;
            record.Position = _Position;
            //---
            //lay thong tin o the file co type="file"
            string _FileName = "";
            try
            {
                //lay ten file
                _FileName = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!String.IsNullOrEmpty(_FileName))
            {
                //upload anh moi
                //lay thoi gian gan vao ten file -> tranh cac file co ten trung ten voi file se upload
                var timestap = DateTime.Now.ToFileTime();
                _FileName = timestap + "_" + _FileName;
                //lay duong dan cua file
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", _FileName);
                //upload file
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                record.Photo = _FileName;
            }
            //---
            //thêm ban ghi vao csdl
            db.Adv.Add(record);
            //cap nhat csdl
            db.SaveChanges();
            //---
            return RedirectToAction("Index");
        }
        //xoa ban ghi
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
            //lay ban ghi tuong ung voi id truyen vao
            var record = db.Adv.Where(item => item.Id == _id).FirstOrDefault();
            ////xoa anh
            if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Adv", record.Photo)))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Adv", record.Photo));
            }
            //xoa ban ghi
            db.Adv.Remove(record);
            //cap nhat csdl
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
