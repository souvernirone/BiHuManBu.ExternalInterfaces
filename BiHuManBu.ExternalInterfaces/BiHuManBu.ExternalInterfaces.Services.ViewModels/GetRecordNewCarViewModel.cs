namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetRecordNewCarViewModel : BaseViewModel
    {
        /// <summary>
        /// 是否新车备案过1是0否
        /// </summary>
        public int HasRecord { get; set; }
        /// <summary>
        /// 新车备案内容，如果HasRecord=1，里面有内容；为0为空对象
        /// </summary>
        public RecordNewCarModel RecordNewCarModel { get; set; }
    }
    public class RecordNewCarModel
    {
        /// <summary>
        /// 车架号
        /// </summary>
        public string CarVin { get; set; }
        /// <summary>
        /// 发动机号
        /// </summary>
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
        public string LicenseOwner { get; set; }
        /// <summary>
        /// 车主证件类型
        /// </summary>
        public int LicenseOwnerIdType { get; set; }
        /// <summary>
        /// 车主证件类型Value
        /// </summary>
        public string LicenseOwnerIdTypeValue { get; set; }
        /// <summary>
        /// 车主身份证号
        /// </summary>
        public string LicenseOwnerIdNo { get; set; }
        /// <summary>
        /// 核定载客量
        /// </summary>
        public int TonCountflag { get; set; }
        /// <summary>
        /// 核定载质量 (非必填)
        /// </summary>
        public string CarTonCount { get; set; }
        /// <summary>
        /// 整备质量kg
        /// </summary>
        public string CarLotEquQuality { get; set; }
        /// <summary>
        /// 排量/功率(升)
        /// </summary>
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
        public string ProofNo { get; set; }
        /// <summary>
        /// 车辆来历凭证所载日期
        /// </summary>
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
        //public long Source { get; set; }
    }
}
