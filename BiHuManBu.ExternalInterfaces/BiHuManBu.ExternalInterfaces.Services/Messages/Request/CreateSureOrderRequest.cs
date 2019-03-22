using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class CreateSureOrderRequest : BaseRequest
    {
        /// <summary>
        /// 报价ID
        /// </summary>
        [Range(1, 21000000000)]
        public long Buid { get; set; }

        /// <summary>
        /// 保险公司
        /// </summary>
        [Range(0, 9223372036854775807)]
        public long Source { get; set; }

        [Range(1, 2100000000)]
        public int CurAgent { get; set; }

        public decimal? BizRate { get; set; }

        /// <summary>
        /// 报价单传OrderId，判断是0就没有预约单，否则就直接执行更新状态操作
        /// </summary>
        [Range(0, 21000000000)]
        public long OrderId { get; set; }

        /// <summary>
        /// 数据来源 目前未使用 微信传7
        /// </summary>
        public int Fountain { get; set; }

        /// <summary>
        /// 当前代理人Id。微信新增字段
        /// 20170228账号统一，openid查询改为childagent查询
        /// </summary>
        [Range(1, 1000000)]
        public int ChildAgent { get; set; }

        public int _orderStatus = -3;
        /// <summary>
        /// 预约单状态，默认已出单。兼容陈龙的-2 出单（故将此字段参数化）
        /// </summary>
        public int OrderStatus { get { return _orderStatus; } set { _orderStatus = value; } }

        /// <summary>
        /// 出单时间
        /// </summary>
        public string SingleTime { get; set; }

        private decimal _bizCost = 0;
        /// <summary>
        /// 商业险费率 20181024addto非车
        /// </summary>
        public decimal BizCost { get { return _bizCost; } set { _bizCost = value; } }

        private decimal _forceCost = 0;
        /// <summary>
        /// 交强险费率 20181024addto非车
        /// </summary>
        public decimal ForceCost { get { return _forceCost; } set { _forceCost = value; } }
        /// <summary>
        /// 客户类别Id 20181024addto非车
        /// </summary>
        public int CategoryInfoId { get; set; }

        /// <summary>
        /// 坐席Id
        /// </summary>
        public int ZuoxiId { get; set; }
    }
}
