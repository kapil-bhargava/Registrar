// Filters/AuthFilter.cs
using System.Web.Mvc;

namespace Regis.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["RegistrarUser"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}