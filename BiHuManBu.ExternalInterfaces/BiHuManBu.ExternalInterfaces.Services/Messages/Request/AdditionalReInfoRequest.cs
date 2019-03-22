
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class AdditionalReInfoRequest:BaseRequest
    {
        [Range(1,2100000000)]
        public long Buid { get; set; }

        #region bx_car_renewal
        //车牌号
        public string LicenseNo { get; set; }
        //上年承保公司
        public int? LastYearSource { get; set; }
        //交强险到期
        public string LastForceEndDate { get; set; }
        //商业险到期
        public string LastBizEndDate { get; set; }
        #endregion

        //以下内容暂时没用到
        #region bx_userinfo_renewal_info
        //客户电话
        public string ClientMobile { get; set; }
        //客户姓名
        public string CilentName { get; set; }
        #endregion

        #region bx_carinfo
        //车牌号  放到bx_car_renewal里面了
        //车主姓名
        public string LicenseOwner { get; set; }
        //发动机号
        public string EngineNo { get; set; }
        //车架号
        public string VinNo { get; set; }
        //车辆注册日期
        public string RegisterDate { get; set; }
        //品牌型号
        public string MoldName { get; set; }
        #endregion
        
        #region bx_car_renewal
        //被保险人姓名
        public string InsuredName { get; set; }
        //被保险人证件类型
        public int? InsuredIdType { get; set; }
        //证件号码
        public string InsuredIdCard { get; set; }
        #endregion
    }
}
