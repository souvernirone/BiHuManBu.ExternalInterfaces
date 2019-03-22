using System.Collections.Generic;
using System.Net;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public  class GetReInfoResponse
    {
        public bx_userinfo UserInfo { get; set; }
        public bx_car_renewal SaveQuote { get; set; }
        public bx_carinfo CarInfo { get; set; }

        public bx_quotereq_carinfo ReqCarinfo { get; set; }

        public bx_lastinfo LastInfo { get; set; }
        public HttpStatusCode Status { get; set; }
        public bx_carmodel CarModel { get; set; }
        public int BusinessStatus { get; set; }
        public string BusinessMessage { get; set; }
        public RemoteMessage.CenterPicCodeCacheModel CenterPicCodeCacheModel { get; set; }
        /// <summary>
        /// 续保过户信息
        /// </summary>
        public List<TransferModel> TransferModelList { get; set; }
        /// <summary>
        /// 续保保费对象
        /// addbygpj20180926
        /// </summary>
        public bx_car_renewal_premium RenewalPremium { get; set; }

    }
    public class AppGetReInfoResponse
    {
        public bx_userinfo UserInfo { get; set; }
        public bx_carinfo CarInfo { get; set; }
        public bx_quotereq_carinfo ReqCarinfo { get; set; }
        public bx_lastinfo LastInfo { get; set; }

        public bx_car_renewal SaveQuote { get; set; }

        public bx_userinfo_renewal_info WorkOrder { get; set; }
        public List<bx_consumer_review> DetailList { get; set; }

        public HttpStatusCode Status { get; set; }
        /// <summary>
        /// 当前记录拥有者，bx_userinfo的anget
        /// </summary>
        public int Agent { get; set; }
        public string AgentName { get; set; }
        /// <summary>
        /// 车牌录入者
        /// </summary>
        public int SaAgent { get; set; }
        public string SaAgentName { get; set; }
        /// <summary>
        /// 是否已分配
        /// </summary>
        public int IsDistrib { get; set; }

        public long? Buid { get; set; }
        public int BusinessStatus { get; set; }
        public string BusinessMessage { get; set; }

    }
}
