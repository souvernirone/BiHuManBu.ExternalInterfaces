using System;

namespace BiHuManBu.ExternalInterfaces.Models.ReportModel
{
    public class TransferModel
    {
        /// <summary>
        /// 初登日期
        /// </summary>
        public DateTime RegisterDate { set; get; }

        /// <summary>
        /// 车牌
        /// </summary>
        public string LicenseNo { get; set; }

        /// <summary>
        ///车架号
        /// </summary>
        public string CarVin { get; set; }

        /// <summary>
        ///发动机号
        /// </summary>
        public string EngineNo { get; set; }

        /// <summary>
        /// 保司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 保司枚举值，老的0,1,2,3这种形式，给前端传需要转换
        /// </summary>
        public int Source { get; set; }
    }
    public class TransferModelNew
    {
        /// <summary>
        /// 初登日期
        /// </summary>
        public string RegisterDate { set; get; }

        /// <summary>
        /// 车牌
        /// </summary>
        public string LicenseNo { get; set; }

        /// <summary>
        ///车架号
        /// </summary>
        public string CarVin { get; set; }

        /// <summary>
        ///发动机号
        /// </summary>
        public string EngineNo { get; set; }

        /// <summary>
        /// 保司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 保司枚举值，新值1，2,4,8这种形式
        /// </summary>
        public long Source { get; set; }
    }
}
