using Regis.Filters;
using System.Web;
using System.Web.Mvc;

namespace Regis
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // Yeh line important hai — ab har controller pe auth check apne aap lagega
            filters.Add(new GlobalRoleAuthFilter());
        }
    }
}
