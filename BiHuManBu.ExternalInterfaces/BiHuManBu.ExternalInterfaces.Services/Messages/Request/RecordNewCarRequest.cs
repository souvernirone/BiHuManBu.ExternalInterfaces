using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class RecordNewCarRequest:BaseRequest
    {
        /// <summary>
        /// 车架号
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(20, MinimumLength = 5)]
        public string CarVin { get; set; }
        /// <summary>
        /// 发动机号
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(20, MinimumLength = 5)]
        public string EngineNo { get; set; }
        /// <summary>
        /// 行驶证车辆类型名称
        /// </summary>
        public string DriveLicenseCarTypeName { get; set; }
        /// <summary>
        /// 行驶证车辆类型值
        /// </summary>
        public string DriveLicenseCarTypeValue { get; set; }
        /// <summary>
        /// 车主名
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(50, MinimumLength = 2)]
        public string LicenseOwner { get; set; }
        /// <summary>
        /// 车主证件类型
        /// </summary>
        [Range(1,20)]
        public int LicenseOwnerIdType { get; set; }
        /// <summary>
        /// 车主证件类型Value
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(50, MinimumLength = 2)]
        public string LicenseOwnerIdTypeValue { get; set; }
        /// <summary>
        /// 车主身份证号
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(50, MinimumLength = 5)]
        public string LicenseOwnerIdNo { get; set; }
        /// <summary>
        /// 核定载客量
        /// </summary>
        [Range(1,100)]
        public int TonCountflag { get; set; }
        /// <summary>
        /// 核定载质量 (非必填)
        /// </summary>
        public string CarTonCount { get; set; }
        /// <summary>
        /// 整备质量kg
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(50, MinimumLength = 1)]
        public string CarLotEquQuality { get; set; }
        /// <summary>
        /// 排量/功率(升)
        /// </summary>
        [DataMember(IsRequired = true)]
        public decimal VehicleExhaust { get; set; }
        /// <summary>
        /// 燃料种类
        /// </summary>
        public string FuelType { get; set; }
        /// <summary>
        /// 燃料种类值
        /// </summary>
        public string FuelTypeValue { get; set; }
        /// <summary>
        /// 车辆来历凭证种类
        /// </summary>
        public string ProofType { get; set; }
        /// <summary>
        /// 车辆来历凭证种类 Value
        /// </summary>
        public string ProofTypeValue { get; set; }
        /// <summary>
        /// 车辆来历凭证编号
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(100, MinimumLength = 1)]
        public string ProofNo { get; set; }
        /// <summary>
        /// 车辆来历凭证所载日期
        /// </summary>
        [DataMember(IsRequired = true)]
        [StringLength(10, MinimumLength = 8)]
        public string ProofTime { get; set; }
        /// <summary>
        /// 新车购置价
        /// </summary>
        public decimal CarPrice { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public string VehicleYear { get; set; }
        /// <summary>
        /// 车型名称
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 精友码
        /// </summary>
        public string ModelCode { get; set; }
        /// <summary>
        /// 哪家保司备案
        /// </summary>
        public long Source { get; set; }
        /// <summary>
        /// bx_agent_config的id 取可以报价的那条
        /// </summary>
        public int ChannelId { get; set; }
    }
}
