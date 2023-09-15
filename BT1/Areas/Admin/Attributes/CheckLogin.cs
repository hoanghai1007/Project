//để kế thừa class ActionFilterAttribute
using Microsoft.AspNetCore.Mvc.Filters;
namespace BT1_project.Areas.Admin.Attributes
{
    public class CheckLogin:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //lấy giá trị session email
            string email = context.HttpContext.Session.GetString("admin_email");
            if (string.IsNullOrEmpty(email))
            {
                context.HttpContext.Response.Redirect("/Admin/Account/Login");
            }
            base.OnActionExecuting(context);
        }
    }
}
