using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using log4net;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class SysCommController : ApiController
    {
        private ILog _logInfo;
        //
        // GET: /SysComm/
        public SysCommController()
        {
            _logInfo = LogManager.GetLogger("INFO");
        }

        private static readonly string _isUpdate = System.Configuration.ConfigurationManager.AppSettings["IsUpdate"];
        public HttpResponseMessage GetUpdate()
        {
            int intUpdate = !string.IsNullOrWhiteSpace(_isUpdate)?int.Parse(_isUpdate):0;//0不更新1更新
            return intUpdate.ResponseToJson();
        }

    }
}
