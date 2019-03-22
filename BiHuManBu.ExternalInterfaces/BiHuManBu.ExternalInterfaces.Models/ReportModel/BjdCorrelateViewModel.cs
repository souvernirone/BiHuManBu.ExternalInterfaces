using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models.ReportModel
{
    public class BjdCorrelateViewModel
    {
        public bj_baodanxinxi Baodanxinxi { get; set; }

        public bj_baodanxianzhong Baodanxianzhong { get; set; }

        public bx_bj_union BjUnion { get; set; }


        public List<bx_claim_detail> ClaimDetail { get; set; }

        public bx_savequote Savequote { get; set; }

        /// <summary>
        /// 优惠活动详情列表
        /// </summary>
        public List<bx_preferential_activity> Activitys { get; set; }

    }
}
