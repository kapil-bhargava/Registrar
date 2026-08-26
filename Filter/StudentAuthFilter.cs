// Filters/StudentAuthFilter.cs
using System.Web.Mvc;

namespace Regis.Filters
{
    public class StudentAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            if (session["StudentUser"] == null || session["StudentApplicationId"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");   // ✅ same single login
            }
            base.OnActionExecuting(filterContext);
        }
    }
}