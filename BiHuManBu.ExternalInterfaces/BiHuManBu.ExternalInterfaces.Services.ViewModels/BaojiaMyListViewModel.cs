
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class BjdListViewModel : BaseViewModel
    {
        public int TotalCount { get; set; }
        public List<MyBaoJiaViewModel> MyBaojiaList { get; set; }
    }

    public class MyBaoJiaViewModel : BaseViewModel
    {
        /// <summary>
        /// userinfo中的UpdateTime//前端要这个字段为最新报价时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 最近(最新)续保时间
        /// </summary>
        public string LatestRenewalTime { get; set; }

        /// <summary>
        /// 默认0无效值，1是当天，2不是当天
        /// </summary>
        public int IsTheDay { get; set; }

        /// <summary>
        /// 0未下单1已下单
        /// </summary>
        public int HasOrder { get; set; }
        public long OrderId { get; set; }
        public int OrderStatus { get; set; }

        /// <summary>
        /// 核保类型组合
        /// </summary>
        public int SubmitGroup { get; set; }
        /// <summary>
        /// 报价类型组合
        /// </summary>
        public int QuoteGroup { get; set; }

        /// <summary>
        /// 0未报价1已报价
        /// </summary>
        public int HasBaojia { get; set; }
        public int? CarUsedType { get; set; }
        public string LicenseOwner { get; set; }
        //被保险人姓名
        public string InsuredName { get; set; }
        public string InsuredMobile { get; set; }
        public string InsuredAddress { get; set; }
        public int InsuredIdType { get; set; }
        /// <summary>
        /// 新增的被保险人证件号码
        /// </summary>
        public string InsuredIdCard { get; set; }
        /// <summary>
        /// 以前用的被保险人证件号码->车主
        /// </summary>
        public string CredentislasNum { get; set; }

        public string PostedName { get; set; }
        public int? IdType { get; set; }
        /// <summary>
        /// 新增的车主证件号码
        /// </summary>
        public string IdCard { get; set; }
        public string CityCode { get; set; }
        public string EngineNo { get; set; }
        public string CarVin { get; set; }
        public string PurchasePrice { get; set; }
        /// <summary>
        /// 报价请求座位数
        /// </summary>
        public int? SeatCount { get; set; }

        public string UserName { get; set; }
        public string LicenseNo { get; set; }
        public string MoldName { get; set; }
        public string RegisterDate { get; set; }
        /// <summary>
        /// 商业险到期时间
        /// </summary>
        public string LastBusinessEndDdate { get; set; }
        /// <summary>
        /// 交强险到期时间
        /// </summary>
        public string LastEndDate { get; set; }
        /// <summary>
        /// 商业险注册时间
        /// </summary>
        public string BizStartDate { get; set; }
        /// <summary>
        /// 交强险注册时间
        /// </summary>
        public string ForceStartDate { get; set; }
        /// <summary>
        /// 请求的交强商业起保时间
        /// </summary>
        public PostStartDateTime PostStartDate { get; set; }
        public long? Buid { get; set; }
        public List<MyPrecisePriceItemViewModel> PrecisePriceItem { get; set; }
        /// <summary>
        /// 每个保司返回的信息有可能是不同的，这个对象用来容纳这些东西
        /// </summary>
        public List<DiffCarInfo> CarInfos { get; set; }
        /// <summary>
        /// 出险次数20181106修改取值位置
        /// </summary>
        public int ClaimCount { get; set; }
        /// <summary>
        /// 交强出险次数20181106add
        /// </summary>
        public int ForceCliamCount { get; set; }
        /// <summary>
        /// 商业出险次数20181106add
        /// </summary>
        public int BizClaimCount { get; set; }
        /// <summary>
        /// 老的出险模型
        /// </summary>
        public List<ClaimDetailViewModel> ClaimDetail { get; set; }
        /// <summary>
        /// 新的出险模型
        /// </summary>
        public List<UserClaimDetailViewModel> ClaimItem { get; set; }

        public List<bx_images> Images { get; set; }

        /// <summary>
        /// 优惠活动
        /// </summary>
        public List<PreActivity> Activity { get; set; }

        /// <summary>
        /// 是否显示费率/计算器 0 展示，1不展示
        /// </summary>
        public int IsShowCalc { get; set; }

        /// <summary>
        /// 公车私车 0：续保失败，无法获取该属性 1：公车 2：私车
        /// </summary>
        public int IsPublic { get; set; }
        /// <summary>
        /// 当前记录的OpenId
        /// </summary>
        public string CurOpenId { get; set; }
        /// <summary>
        /// 当前记录的Agent
        /// </summary>
        public string CurAgent { get; set; }
        /// <summary>
        /// 获取电子保单的邮箱地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 是否是被保险人 0不是 1是 2是正常值 通过续保过来的
        /// </summary>
        public int IsTempInsured { get; set; }

        public string CurAgentName { get; set; }
        public string CurAgentMobile { get; set; }

        /// <summary>
        /// 报价请求 是否是新车 1：新车2：旧车（默认）
        /// </summary>
        public int IsNewCar { get; set; }

        /// <summary>
        /// 报价请求 精友码
        /// </summary>
        public string AutoMoldCode { get; set; }

        /// <summary>
        /// 协商价
        /// </summary>
        public decimal CoRealValue { get; set; }

        private List<UrlAndType> imgs = new List<UrlAndType>();
        /// <summary>
        /// 用户上传图片，废弃
        /// </summary>
        public List<UrlAndType> Imgs { get { return imgs; } set { imgs = value; } }

        private List<IsUploadImg> _isUpload = new List<IsUploadImg>();
        /// <summary>
        /// 新的用户上传的图片
        /// </summary>
        public List<IsUploadImg> IsUploadImg { get { return _isUpload; } set { _isUpload = value; } }

        #region 投保人
        /// <summary>
        /// 是否投保人
        /// </summary>
        public int IsHolder { get; set; }
        public Holder Holder { get; set; }
        #endregion

        /// <summary>
        /// 报价时间
        /// </summary>
        public string QuoteTime { get; set; }

        public RelationPerson CarOwnerPerson { get; set; }
        public RelationPerson HolderPerson { get; set; }
        public RelationPerson InsuredPerson { get; set; }
        public QuoteReqCarInfoViewModel ReqInfo { get; set; }

        /// <summary>
        /// 身份证后六位
        /// </summary>
        public string SixDigitsAfterIdCard { get; set; }
        /// <summary>
        /// 新折扣系数
        /// </summary>
        public string NewRate { get; set; }
        /// <summary>
        /// 是否贷款车  1贷款  0 不贷款  -1不知道是否贷款
        /// </summary>
        public int? IsLoans { get; set; }

        /// <summary>
        /// 过户时间  具体时间: 过户   "": 未过户    "-1": 不知道是否过户
        /// </summary>
        public string TransferDate { get; set; }
    }

    /// <summary>
    /// 投保人
    /// </summary>
    public class Holder
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string HolderName { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public int HolderIdType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string HolderIdCard { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string HolderMobile { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string HolderAddress { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string HolderEmail { get; set; }
    }

    public class PreActivity
    {
        public string ActivityName { get; set; }
        public string ActivityContent { get; set; }
    }

    /// <summary>
    /// 每个保司返回的信息有可能是不同的，这个对象用来容纳这些东西
    /// </summary>
    public class DiffCarInfo
    {
        /// <summary>
        /// 精友编码
        /// </summary>
        public string AutoMoldCode { get; set; }
        public string VehicleInfo { get; set; }
        /// <summary>
        /// 报价返回座位数
        /// </summary>
        public int CarSeat { get; set; }
        public long Source { get; set; }

        public string Risk { get; set; }
        public string XinZhuanXu { get; set; }
        public string SyVehicleClaimType { get; set; }
        public string JqVehicleClaimType { get; set; }
        public string VehicleStyle { get; set; }
        /// <summary>
        /// 车型别名
        /// </summary>
        public string VehicleAlias { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public string VehicleYear { get; set; }
    }



    public class BaojiaMyListViewModel
    {
        public long b_uid { get; set; }
        public string UserName { get; set; }
        public string LicenseNo { get; set; }
        public string MoldName { get; set; }
        public string CarRegisterDate { get; set; }
        public string last_business_end_date { get; set; }
        public string InsuredIdCard { get; set; }

        //public List<QuoteTotal> quoteresult { get; set; }
        public List<PrecisePriceItemViewModel> precisePriceItem { get; set; }
    }
    public class QuoteTotal
    {
        public int source { get; set; }
        public string BizTotal { get; set; }
        public string ForceTotal { get; set; }
        public string TaxTotal { get; set; }
        public string Total { get; set; }
        public int? submit_status { get; set; }
    }

    /// <summary>
    /// 上传图片的类型和地址
    /// </summary>
    public class UrlAndType
    {
        public string Type { get; set; }

        public string Url { get; set; }
    }

    public class ImgUrl
    {
        public List<UrlAndType> Urls { get; set; }

        /// <summary>
        /// 哪家保司 新的source值
        /// </summary>
        public long Source { get; set; }
    }
    public class IsUploadImg
    {
        /// <summary>
        /// 1上传0未上传
        /// </summary>
        public int IsUpload { get; set; }

        /// <summary>
        /// 哪家保司 新的source值
        /// </summary>
        public long Source { get; set; }
    }
}
