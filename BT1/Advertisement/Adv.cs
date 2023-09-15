using BT1_project.Models;
namespace BT1_project.Advertisement
{
    public class Adv
    {
        //lay danh sach cac ban ghi qua vi tri
        //tu khoa static co y nghia: TenClass.TenHam() ma khong can khoi tao doi tuong
        public static List<ItemAdv> GetAdv(int _Position)
        {
            MyDbContext db = new MyDbContext();
            var list_record = db.Adv.Where(item => item.Position == _Position).OrderByDescending(item => item.Id).ToList();
            return list_record;
        }
    }
}
