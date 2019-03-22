using System.Web;
using System.Web.Mvc;

namespace BiHuManBu.CacheService.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}