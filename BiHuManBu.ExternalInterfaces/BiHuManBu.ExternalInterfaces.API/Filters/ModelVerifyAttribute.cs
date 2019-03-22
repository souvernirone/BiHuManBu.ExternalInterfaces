using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BiHuManBu.ExternalInterfaces.API.Filters
{
    public class ModelVerifyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                string msg = actionContext.ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";"));
                actionContext.Response = new HttpResponseMessage {
                    Content=new StringContent("{\"resultcode\":-100, \"message\":\"输入参数错误，"+ msg + "\" }"),
                };
            }
            base.OnActionExecuting(actionContext);
        }
    }
}