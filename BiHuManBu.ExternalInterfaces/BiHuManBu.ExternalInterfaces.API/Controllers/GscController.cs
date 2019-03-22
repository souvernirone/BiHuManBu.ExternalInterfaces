using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class GscController : ApiController
    {
        private IGscService _gscService;

        public GscController(IGscService gscService)
        {
            _gscService = gscService;
        }
        public HttpResponseMessage GetUserInfo(string usercode, string pwd)
        {
            bx_gsc_userinfo userinfo=_gscService.FindUserInfoByGsc(usercode, pwd);
            if (userinfo != null)
            {
                return userinfo.ResponseToJson();
            }
            else
            {
                return null;
            }
            
        }
        //
        // GET: /Gsc/



    }
}
