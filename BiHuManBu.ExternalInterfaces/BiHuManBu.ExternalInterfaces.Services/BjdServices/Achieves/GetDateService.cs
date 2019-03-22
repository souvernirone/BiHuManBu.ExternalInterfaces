using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class GetDateService : IGetDateService
    {
        private IQuoteResultRepository _quoteResultRepository;
        private ILog logErr;
        public GetDateService(IQuoteResultRepository quoteResultRepository)
        {
            _quoteResultRepository = quoteResultRepository;
            logErr = LogManager.GetLogger("ERROR");
        }
        public string GetDate(bx_userinfo userinfo, out string postBizStartDate)
        {
            var quoteresult = userinfo.QuoteStatus > -1 ? _quoteResultRepository.GetStartDate(userinfo.Id) : new InsuranceStartDate();//,userinfo.Source.Value
            string postForceStartDate = string.Empty;//交强险起始时间
            postBizStartDate = string.Empty;//商业险起始时间
            //报价成功取quoteresult表起保时间
            if (userinfo.QuoteStatus > 0)
            {
                postForceStartDate = quoteresult != null ? (quoteresult.ForceStartDate.HasValue
                        ? quoteresult.ForceStartDate.Value.ToString("yyyy-MM-dd HH:mm") : "") : "";
                postBizStartDate = quoteresult != null ? (quoteresult.BizStartDate.HasValue
                        ? quoteresult.BizStartDate.Value.ToString("yyyy-MM-dd HH:mm") : "") : "";
            }
            return postForceStartDate;
        }
    }
}
