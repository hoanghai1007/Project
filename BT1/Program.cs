var builder = WebApplication.CreateBuilder(args);
//Add MVC
builder.Services.AddControllersWithViews();
//khai báo session
builder.Services.AddSession();
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//điều phối url
app.UseRouting();
//sử dụng session
app.UseSession();
//khai báo thư mục wwwroot thành thư mục ảo
app.UseStaticFiles();
//câu hình url
app.MapControllerRoute(name: "area", pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name:"default",pattern:"{controller=Home}/{action=Index}/{id?}");

app.Run();
