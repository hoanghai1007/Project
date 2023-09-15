//để kế thừa class DbContext
using Microsoft.EntityFrameworkCore;
using BT1_project.Models;
//để thao tác với file,thư mục
using System.IO;

namespace BT1_project.Models
{
    public class MyDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string strConnectionString = config.GetConnectionString("MyConnectionString").ToString();
            //kết nối csdl
            optionsBuilder.UseSqlServer(strConnectionString);
        }
        public DbSet<ItemUser> Users { get; set; }// <=> table users
        public DbSet<ItemCategory> Categories { get; set; }// <=> table Categories
        public DbSet<ItemAdv> Adv { get; set; }
        public DbSet<ItemCustomer> Customers { get; set; }
        public DbSet<ItemNews> News { get; set; }
        public DbSet<ItemOrder> Orders { get; set; }
        public DbSet<ItemOrderDetail> OrdersDetail { get; set; }
        public DbSet<ItemProduct> Products { get; set; }
        public DbSet<ItemRating> Rating { get; set; }
        public DbSet<ItemSlide> Slides { get; set; }
        public DbSet<ItemTag> Tags { get; set; }
        public DbSet<ItemTagProduct> TagsProducts { get; set; }
        public DbSet<ItemCategoryProduct> CategoriesProducts { get; set; }
        public object ColorsProducts { get; internal set; }
        public object Colors { get; internal set; }
    }
}
