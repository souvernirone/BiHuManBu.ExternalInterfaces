using System.Web;
using System.Web.Mvc;

namespace BihuManBu.MonitorPlatform
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}