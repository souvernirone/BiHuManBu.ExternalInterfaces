using BiHuManBu.ExternalInterfaces.Models;
using System.Collections.Generic;
using System.Net;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetPrecisePriceReponse:BaseResponse
    {
        public bx_userinfo UserInfo { get; set; }
        public bx_lastinfo LastInfo { get; set; }
        public bx_savequote SaveQuote { get; set; }
        public bx_quoteresult QuoteResult { get; set; }

        public bx_submit_info SubmitInfo { get; set; }

        public bx_car_renewal Renewal { get; set; }
        public bx_agent_config AgentConifg { get; set; }

        public bx_quoteresult_carinfo CarInfo { get; set; }

        public bx_quotereq_carinfo ReqInfo { get; set; }
        public int BusinessStatus { get; set; }

        public string CheckCode { get; set; }
        public List<bx_ywxdetail> YwxDetails { get; set; }

    }
}
