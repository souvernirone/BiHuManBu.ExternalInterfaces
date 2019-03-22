using System.Collections.Generic;
using System.Net;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public  class GetAgentIdentityAndRateResponse
    {
        /// <summary>
        /// 是否是经纪人 1:经纪人 0：直客
        /// </summary>
        public int IsAgent { get; set; }

        public Rate AgentRate { get; set; }

        public List<Rate> ZhiKeRate { get; set; }


        public HttpStatusCode Status { get; set; }

    }
}
