using System.Web;
using System.Web.Mvc;
using WebApi.Providers;

namespace WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new CustomAuthorize());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
