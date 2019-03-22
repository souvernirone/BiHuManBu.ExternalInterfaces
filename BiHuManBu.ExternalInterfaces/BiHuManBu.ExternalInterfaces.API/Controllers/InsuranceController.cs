using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using log4net;
using WebApiThrottle;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    /// <summary>
    /// 车险
    /// </summary>
    [DisableThrotting()]
    public class InsuranceController:ApiController
    {
        private ILog logInfo;
        private ILog logError;
        private ICarInsuranceService _carInsuranceService;
        public InsuranceController(ICarInsuranceService carInsuranceService)
        {
            logInfo = LogManager.GetLogger("INFO");
            logError = LogManager.GetLogger("ERROR");
            _carInsuranceService = carInsuranceService;
        }

         //续保接口
        [HttpGet]
        [EnableThrottling()]
        public async Task<HttpResponseMessage> FetchCarInfo([FromUri] GetReInfoRequest request)
        {
            return null;
        }

    }
}