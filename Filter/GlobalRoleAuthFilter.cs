// Filters/GlobalRoleAuthFilter.cs
// Globally register hota hai (FilterConfig) — har controller/action pe automatically chalega.
using System.Linq;
using System.Web.Mvc;

namespace Regis.Filters
{
    public class GlobalRoleAuthFilter : IAuthorizationFilter
    {
        // 👈 Controller ka actual class naam StudentLoginController hai — isliye yahan bhi wahi
        private static readonly string[] StudentControllers = { "StudentLogin" };

        // In par koi login check nahi (Login/Logout page, Home, Error)
        private static readonly string[] PublicControllers = { "Account", "Home", "Error" };

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool ignoreAuth =
                filterContext.ActionDescriptor.IsDefined(typeof(IgnoreAuthAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(IgnoreAuthAttribute), true);

            if (ignoreAuth) return;

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (PublicControllers.Contains(controllerName)) return;

            var session = filterContext.HttpContext.Session;

            if (StudentControllers.Contains(controllerName))
            {
                if (session == null || session["StudentUser"] == null || session["StudentApplicationId"] == null)
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
                return;
            }

            // Baaki SAARE controllers => Registrar-only
            if (session == null || session["RegistrarUser"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}