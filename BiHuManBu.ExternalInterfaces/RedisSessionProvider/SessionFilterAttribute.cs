using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RedisSessionProvider
{
    public class SessionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 每次请求都续期
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            new Session(filterContext.HttpContext).Postpone();
        }
        //第一步
        // GlobalFilters.Filters.Add(new SessionFilterAttribute()); 全局文件
        //第二步
        //设置 登录filter

    }
}
