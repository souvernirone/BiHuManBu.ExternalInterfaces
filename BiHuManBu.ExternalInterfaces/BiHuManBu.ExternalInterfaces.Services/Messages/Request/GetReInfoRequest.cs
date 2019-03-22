using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class PointFloat
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
    /// <summary>
    /// 获取续保信息接口
    /// </summary>
    public class GetReInfoRequest
    {
        /// <summary>
        ///     非公车
        /// </summary>
        /// <summary>
        ///     客户端标识
        /// </summary>
        private string _custKey = string.Empty;

        private int _isLastYearNewCar = 1;
        private int _renewalType = 2;
        /// <summary>
        ///     车牌号
        /// </summary>
        //[Required]
        [StringLength(30, MinimumLength = 5)]
        public string LicenseNo { get; set; }

        /// <summary>
        ///     投保城市
        /// </summary>
        [Range(1, 10000)]
        public int CityCode { get; set; }

        /// <summary>
        ///     车主姓名
        /// </summary>
        public string CarOwnersName { get; set; }

        /// <summary>
        ///     车主行驶证
        /// </summary>
        [StringLength(18, MinimumLength = 15)]
        public string IdCard { get; set; }

        /// <summary>
        ///     经纪人
        /// </summary>
        [Range(1, 10000000)]
        public int Agent { get; set; }

        public int ChildAgent { get; set; }
        //[Range(0,1)]
        //public int IsPublic { get { return _isPublic; }set { _isPublic = value; }}
        [Required]
        [StringLength(32, MinimumLength = 10)]
        public string CustKey
        {
            get { return _custKey; }
            set { _custKey = value; }
        }

        /// <summary>
        ///     校验串
        /// </summary>
        [Required]
        [StringLength(32, MinimumLength = 32)]
        public string SecCode { get; set; }

        /// <summary>
        ///     续保类型
        /// </summary>
        public int RenewalType
        {
            get { return _renewalType; }
            set { _renewalType = value; }
        }

        /// <summary>
        ///     发动机号 和车架号是为了 兼容去年新车用这俩属性上险的情况进行续保
        /// </summary>
        [StringLength(50, MinimumLength = 0)]
        public string EngineNo { set; get; }

        /// <summary>
        ///     车架号
        /// </summary>
        [RegularExpression(@"^[A-Z_0-9-]{0,50}$")]
        public string CarVin { set; get; }

        /// <summary>
        ///     是否是去年新车 1：旧车 2：次新车（新车无续保）
        /// </summary>
        [Range(1, 2)]
        public int IsLastYearNewCar
        {
            get { return _isLastYearNewCar; }
            set { _isLastYearNewCar = value; }
        }

        /// <summary>
        ///     组合报价标示
        /// </summary>
        [Range(0, 1)]
        public int Group { get; set; }

        /// <summary>
        ///     addby20160909
        ///     目前只对app用
        ///     登陆状态
        /// </summary>
        public string BhToken { get; set; }

        /// <summary>
        ///     是否展示 商业险及交强都保单号
        /// </summary>
        public int CanShowNo { get; set; }

        /// <summary>
        ///     是否展示排量
        /// </summary>
        public int CanShowExhaustScale { get; set; }

        /// <summary>
        ///     是否展示修理厂类型 0:否 1：是
        /// </summary>
        public int ShowXiuLiChangType { get; set; }

        /// <summary>
        ///     续保相关日期 格式化 1:带有日期 时间格式  0：默认 没有时间格式
        /// </summary>
        public int TimeFormat { get; set; }

        /// <summary>
        ///     展示内部相关信息 0:否 1：是
        /// </summary>
        public int ShowInnerInfo { get; set; }

        /// <summary>
        ///     0小车，1大车，默认0
        /// </summary>
        public int RenewalCarType { get; set; }

        /// <summary>
        ///     0:不展示  1：展示
        /// </summary>
        public int ShowAutoMoldCode { get; set; }

        /// <summary>
        ///     补偿费用 0:不展示  1:展示
        /// </summary>
        public int ShowFybc { get; set; }

        /// <summary>
        ///     设备： 0:不展示  1:展示
        /// </summary>
        public int ShowSheBei { get; set; }

        /// <summary>
        ///     废弃
        /// </summary>
        public int ShowRenewalCarType { get; set; }

        public int ShowCarType { get; set; }
        public int ShowOrg { get; set; }

        /// <summary>
        ///     是否展示续保车型
        /// </summary>
        public int ShowRenewalCarModel { get; set; }

        public int ShowRelation { get; set; }

        /// <summary>
        ///     上年投保公司枚举
        /// </summary>
        public long RenewalSource { get; set; }

        #region 续保接口拆分用

        /// <summary>
        ///     是否调用续保信息
        /// </summary>
        public int IsCallNext { get; set; }

        /// <summary>
        ///     是否直接调用已有数据 1:是  0：否
        /// </summary>
        public int IsDirectRenewal { get; set; }

        #endregion

        /// <summary>
        /// 是否需要再次车型过滤 1是0否 光鹏洁 /2018-01-30
        /// 摄像头进店老数据需要第二次续保，如果需要则调用crm接口CarMoldFilter，否则调用crm接口Distribute
        /// </summary>
        public int NeedCarMoldFilter { get; set; }
        /// <summary>
        /// 摄像头绑定的代理人20180131启用
        /// </summary>
        public int CameraAgent { get; set; }

        /// <summary>
        /// 身份证后六位
        /// </summary>
        [StringLength(6, MinimumLength = 0)]
        public string SixDigitsAfterIdCard { get; set; }

        /// <summary>
        /// 是否展示商业交强到期时间
        /// </summary>
        public int ShowExpireDateNum { get; set; }
        /// <summary>
        /// 是否展示三者节假日险 1展示0不展示
        /// </summary>
        public int ShowSanZheJieJiaRi { get; set; }

        #region 平安续保输入验证码
        /// <summary>
        /// 参与平安校验码的输入。1参与0不参与。
        /// 壁虎内部系统默认=1参与，以下参数第一次可不填。
        /// 当=1时，请求会返回paukey、requestkey、checkpicture，如果需要平安验证码，则返回BusinessStatus=-10009
        /// 第二次请求输入YZMArea、PAUKey、RequestKey3个参数
        /// </summary>
        public int ShowPACheckCode { get; set; }
        /// <summary>
        /// 用户点击的验证码坐标，前端传来，用来转换 YZMArea
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// 用户点击验证码坐标
        /// </summary>
        public List<PointFloat> YZMArea { get; set; }
        /// <summary>
        /// 上次请求的渠道UKey信息（和RequestKey 保持原有会话）
        /// </summary>
        public int PAUKey { get; set; }
        /// <summary>
        /// 上次请求的渠道RequestKey信息（和UKey 保持原有会话）
        /// </summary>
        public string RequestKey { get; set; }
        #endregion

        /// <summary>
        /// 是否显示过户车模型
        /// </summary>
        public int ShowTransferModel { get; set; }

        /// <summary>
        /// 摄像头Id，是一串字符串 addby20180820.gpj
        /// </summary>
        public string CameraId { get; set; }

        /// <summary>
        /// 是否展示保费1是0否
        /// addby gpj20180926
        /// </summary>
        public int ShowBaoFei { get; set; }

        /// <summary>
        /// 车五项取大车数据
        /// </summary>
        public int GetAllCar { get; set; }

        /// <summary>
        /// 是否强制刷新核保 1是0否
        /// </summary>
        public int IsForceRenewal { get; set; }

        /// <summary>
        /// 车电联呼用zuoxiId，权限判断用
        /// </summary>
        public int ZuoXiId { get; set; }
    }
}