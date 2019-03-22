using System.Collections.Generic;
using System.Configuration;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public  class GetBjdItemResponse
    {
        public bj_baodanxianzhong Baodanxianzhong { get; set; }
        public bj_baodanxinxi Baodanxinxi { get; set; }
        public List<bx_claim_detail> ClaimDetail { get; set; }
        public bx_savequote Savequote { get; set; }

        /// <summary>
        /// 优惠活动详情列表
        /// </summary>
        public List<bx_preferential_activity> Activitys { get; set; }

        /// <summary>
        /// 业务员信息
        /// </summary>
        public AgentViewModelByBJ AgentDetail { get; set; }
    }
}
