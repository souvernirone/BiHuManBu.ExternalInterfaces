using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class CheckReqBaseInfo : ICheckReqBaseInfo
    {
        private readonly List<int> _idtypeList = new List<int>() { 1, 2, 5, 9, 14 };
        private readonly ICheckMultiChannels _checkMultiChannels;
        public CheckReqBaseInfo(ICheckMultiChannels checkMultiChannels)
        {
            _checkMultiChannels = checkMultiChannels;
        }
        public BaseViewModel CheckBaseInfo(PostPrecisePriceRequest request)
        {
            BaseViewModel viewModel = new BaseViewModel();
            string strMsg = string.Empty;//错误消息返回
            #region 车牌号校验
            List<int> _renewalTypeLicenseNo = new List<int>() { 4, 6, 7, 8 };//crm、app、微信
            if (!_renewalTypeLicenseNo.Contains(request.RenewalType) && !request.LicenseNo.Equals(request.CarVin))
            {
                //不是crm、app、微信，并且车牌！=车架，需要走车牌的校验。
                //modify20181124，此处考虑到车架号发动机号续保时，请求报价的车牌需要传车架号，故将此限制去掉。
                if (request.IsNewCar != 1 && !(request.LicenseNo.IsValidLicenseno() ||
                    (!string.IsNullOrWhiteSpace(request.UpdateLicenseNo) && !string.IsNullOrWhiteSpace(request.LicenseNo) && request.UpdateLicenseNo.IsValidLicenseno())))
                {
                    viewModel.StatusMessage = "输入参数错误，车牌号格式不正确";
                    return FailedViewModel(viewModel, -10000);
                }
            }
            if (!string.IsNullOrWhiteSpace(request.CarVin))
            {
                if (request.CarVin.Length < 2 || request.CarVin.Length > 17)
                {
                    viewModel.StatusMessage = "输入参数错误，车架号格式不正确";
                    return FailedViewModel(viewModel, -10000);
                }
            }
            #endregion
            #region 初登日期校验
            if (!string.IsNullOrWhiteSpace(request.RegisterDate))
            {
                if (!request.RegisterDate.IsValidDate())
                {
                    viewModel.StatusMessage = "输入参数错误，请检查初登日期";
                    return FailedViewModel(viewModel, -10000);
                }
                DateTime dtRegisterDate = DateTime.Parse(request.RegisterDate);
                if (dtRegisterDate > DateTime.Now || dtRegisterDate < DateTime.Parse("1900-01-01"))
                {
                    viewModel.StatusMessage = "输入参数错误，请检查初登日期";
                    return FailedViewModel(viewModel, -10000);
                }
            }
            #endregion
            #region 精友码、车型报价类型判断
            if (request.AutoMoldCodeSource > 4 || request.AutoMoldCodeSource < -1)
            {
                viewModel.StatusMessage = "输入参数错误，AutoModelCodeSource请参考" + "1用户选择续保续保车型报价，2用户选择自定义车型报价，3用户选择最低配置车型报价!";
                return FailedViewModel(viewModel, -10000);
            }
            else
            {
                if (request.AutoMoldCodeSource > 0 && string.IsNullOrWhiteSpace(request.AutoMoldCode) && request.QuoteGroup != 2)
                {
                    viewModel.StatusMessage = "输入参数错误,您选择了车型报价类型，请完善精友码才可以继续报价(调用接口6)";
                    return FailedViewModel(viewModel, -10000);
                }
            }
            #endregion
            #region Email判断
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                if (request.Email.Length > 50)
                {
                    viewModel.StatusMessage = "输入参数错误，" + "Email长度过长!";
                    return FailedViewModel(viewModel, -10000);
                }
            }
            #endregion
            #region 非平安报价带星判断：非平安的报价，关系人信息/车架号/发动机号不允许带星
            if (request.QuoteGroup != 2)
            {
                if (!string.IsNullOrWhiteSpace(request.CarOwnersName) && request.CarOwnersName.Contains("*")
                    || !string.IsNullOrWhiteSpace(request.HolderName) && request.HolderName.Contains("*")
                    || !string.IsNullOrWhiteSpace(request.InsuredName) && request.InsuredName.Contains("*"))
                {
                    viewModel.StatusMessage = "关系人信息加星只能投保平安，请输入完整的关系人信息或取消其他家的报价";
                    return FailedViewModel(viewModel, -10000);
                }
                if (!string.IsNullOrWhiteSpace(request.CarVin) && request.CarVin.Contains("*"))
                {
                    viewModel.StatusMessage = "车辆信息加星只能投保平安，请输入完整的车辆信息或取消其他家的报价";
                    return FailedViewModel(viewModel, -10000);
                }
            }
            #endregion
            #region 被保人/车主/投保人证件信息判断
            //车主
            strMsg = CheckRelation(request.QuoteGroup, "车主", request.CarOwnersName, request.OwnerIdCardType, request.IdCard);
            if (!string.IsNullOrEmpty(strMsg))
            {
                viewModel.StatusMessage = strMsg;
                return FailedViewModel(viewModel, -10000);
            }
            //被保人
            strMsg = CheckRelation(request.QuoteGroup, "被保险人", request.InsuredName, request.InsuredIdType, request.InsuredIdCard);
            if (!string.IsNullOrEmpty(strMsg))
            {
                viewModel.StatusMessage = strMsg;
                return FailedViewModel(viewModel, -10000);
            }
            //投保人
            strMsg = CheckRelation(request.QuoteGroup, "投保人", request.HolderName, request.HolderIdType, request.HolderIdCard);
            if (!string.IsNullOrEmpty(strMsg))
            {
                viewModel.StatusMessage = strMsg;
                return FailedViewModel(viewModel, -10000);
            }
            #endregion
            #region 车主/被保人证件有效期判断
            strMsg = CheckCertiStartdate("投保人", request.HolderIdType, request.HolderCertiStartdate, request.HolderCertiEnddate);
            if (!string.IsNullOrEmpty(strMsg))
            {
                viewModel.StatusMessage = strMsg;
                return FailedViewModel(viewModel, -10000);
            }
            strMsg = CheckCertiStartdate("投保人", request.InsuredIdType, request.InsuredCertiStartdate, request.InsuredCertiEnddate);
            if (!string.IsNullOrEmpty(strMsg))
            {
                viewModel.StatusMessage = strMsg;
                return FailedViewModel(viewModel, -10000);
            }
            #endregion
            #region 新车逻辑
            if (request.IsNewCar == 1)
            {
                if (string.IsNullOrWhiteSpace(request.CarVin) || string.IsNullOrWhiteSpace(request.EngineNo) ||
                    string.IsNullOrWhiteSpace(request.MoldName) || string.IsNullOrWhiteSpace(request.RegisterDate))
                {
                    viewModel.StatusMessage = "输入参数错误，" + "新车报价 carvin，engineno，moldname，registerdate不能为空";
                    return FailedViewModel(viewModel, -10000);
                }
                DateTime regdate;
                DateTime.TryParse(request.RegisterDate, out regdate);
                if (regdate > DateTime.Now.Date || regdate < DateTime.Now.AddDays(-90))
                {
                    viewModel.StatusMessage = "输入参数错误，" + "新车报价初登日期只能是之前90天内（包含当日）";
                    return FailedViewModel(viewModel, -10000);
                }
                //if (string.IsNullOrWhiteSpace(request.BizTimeStamp) && string.IsNullOrWhiteSpace(request.BizStartDate))
                //{
                //viewModel.StatusMessage = "输入参数错误，" + "商业险起保日期不能是空";
                //return FailedViewModel(viewModel, -10000);
                //}
                DateTime bizdate;
                if (!string.IsNullOrWhiteSpace(request.BizStartDate))
                {

                    DateTime.TryParse(request.BizStartDate, out bizdate);
                    if (bizdate < DateTime.Now.Date && bizdate > DateTime.Now.AddDays(90))
                    {
                        viewModel.StatusMessage = "输入参数错误，" + "商业险起保日期只能是90天内";
                        return FailedViewModel(viewModel, -10000);
                    }
                }
                if (!string.IsNullOrWhiteSpace(request.BizTimeStamp))
                {
                    bizdate = request.BizTimeStamp.UnixTimeToDateTime();
                    if (bizdate < DateTime.Now.Date && bizdate > DateTime.Now.AddDays(90))
                    {
                        viewModel.StatusMessage = "输入参数错误，" + "商业险起保日期只能是90天内";
                        return FailedViewModel(viewModel, -10000);
                    }
                }
            }
            #endregion

            #region 验证传进来的渠道能用20180123
            if (!string.IsNullOrWhiteSpace(request.MultiChannels))
            {
                string newMultiChannels = string.Empty;
                viewModel = _checkMultiChannels.CheckMultiChannelsUsed(request.MultiChannels, request.Agent, request.CityCode, request.QuoteGroup, out newMultiChannels);
                request.MultiChannels = newMultiChannels;
            }
            #endregion

            #region 车辆使用性质拦截  4,5,8 以及杭州之外的网约车（20）限制不支持报价

            if (request.CarUsedType == 4 || request.CarUsedType == 5 || request.CarUsedType == 8 ||
                (request.CarUsedType == 20 && request.CityCode != 9))
            {
                viewModel.StatusMessage = "输入参数错误，不支持使用性质是营业非营业、出租租赁、城市公交车辆报价";
                return FailedViewModel(viewModel, -10001);
            }
            #endregion
            #region 跟单费率和销售费率校验
            strMsg = CheckRatio("实际销售费用交强险比率", request.ActualSalesForceRatio);
            if (!string.IsNullOrEmpty(strMsg))
            {
                viewModel.StatusMessage = strMsg;
                return FailedViewModel(viewModel, -10000);
            }
            strMsg = CheckRatio("实际销售费用商业险比率", request.ActualSalesBizRatio);
            if (!string.IsNullOrEmpty(strMsg))
            {
                viewModel.StatusMessage = strMsg;
                return FailedViewModel(viewModel, -10000);
            }
            strMsg = CheckRatio("实际跟单费用交强险比率", request.ActualDtaryForceRatio);
            if (!string.IsNullOrEmpty(strMsg))
            {
                viewModel.StatusMessage = strMsg;
                return FailedViewModel(viewModel, -10000);
            }
            strMsg = CheckRatio("实际跟单费用商业险比率", request.ActualDtaryBizRatio);
            if (!string.IsNullOrEmpty(strMsg))
            {
                viewModel.StatusMessage = strMsg;
                return FailedViewModel(viewModel, -10000);
            }
            #endregion
            #region 修改座位数的校验
            if (request.SeatUpdated == 1 && request.SeatCount < 1) {
                viewModel.StatusMessage = "输入参数错误，如果修改座位数标识为1，请将座位数传大于0的值。";
                return FailedViewModel(viewModel, -10000);
            }
            #endregion

            return new BaseViewModel();
        }

        /// <summary>
        /// 重新封装viewmodel，把失败值返回
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="codeStatus"></param>
        /// <returns></returns>
        private BaseViewModel FailedViewModel(BaseViewModel viewModel, int codeStatus = 0)
        {
            viewModel.BusinessStatus = codeStatus;
            return viewModel;
        }

        private string CheckRelation(int quoteGroup, string strType, string name, int idType, string idCard)
        {
            if (!string.IsNullOrWhiteSpace(idCard) && idType == 0)
            {
                return string.Format("输入参数错误，" + "{0}证件类型不为空！", strType);
            }
            if (idType > 0)
            {
                if (!name.IsValidStar())
                {
                    //带星的
                    //只可以请求平安
                    if (quoteGroup != 2)
                    {
                        return string.Format("输入参数错误，{0}姓名格式错误！", strType);
                    }
                }
                else
                {
                    if (!name.IsValidName(idType))
                    {
                        return string.Format("输入参数错误，{0}姓名格式错误！", strType);
                    }
                }
                if (string.IsNullOrWhiteSpace(idCard))
                {
                    return string.Format("输入参数错误，{0}证件号不为空！", strType);
                }
                if (!_idtypeList.Contains(idType))
                {
                    return string.Format("输入参数错误，不支持输入的{0}证件类型!", strType);
                }
                if (!idCard.IsValidStar())
                {
                    //带星的
                    //只可以请求平安
                    if (quoteGroup != 2)
                    {
                        return string.Format("输入参数错误，{0}证件号格式错误！", strType);
                    }
                }
                else
                {
                    //不带星的
                    //直接校验证件号
                    if (idType == 1 && !idCard.IsValidIdCard())
                    {
                        return string.Format("输入参数错误，{0}身份证号格式错误！", strType);
                    }
                    else if (idType == 2 && !idCard.IsValidZZJG())
                    {
                        return string.Format("输入参数错误，{0}组织机构代码格式错误！", strType);
                    }
                    else if (idType == 9 && !idCard.IsValidYYZZ())
                    {
                        return string.Format("输入参数错误，{0}营业执照（社会统一信用代码）格式错误！", strType);
                    }
                    else if (idType == 5 && !idCard.IsValidTXZ())
                    {
                        return string.Format("输入参数错误，{0}港澳居民来往内地通行证格式错误！", strType);
                    }
                    else if (idType == 14 && !idCard.IsValidGASFZ())
                    {
                        return string.Format("输入参数错误，{0}港澳居民身份证格式错误！", strType);
                    }
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 证件有效期校验
        /// </summary>
        /// <param name="strType"></param>
        /// <param name="idType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private string CheckCertiStartdate(string strType, int idType, string startDate, string endDate)
        {
            if (!string.IsNullOrWhiteSpace(startDate))
            {
                if (idType < 1)
                {
                    return string.Format("输入参数错误，如果存在证件有效期，请输入正确的{0}证件类型!", strType);
                }
                if (!startDate.IsValidDate())
                {
                    return string.Format("输入参数错误，请确认{0}证件起始时间范围!", strType);
                }
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                if (idType < 1)
                {
                    return string.Format("输入参数错误，如果存在证件有效期，请输入正确的{0}证件类型!", strType);
                }
                if (!endDate.IsValidDate())
                {
                    return string.Format("输入参数错误，请确认{0}证件结束时间范围!", strType);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 校验跟单费率和销售费率
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        private string CheckRatio(string filename, decimal ratio)
        {
            if (ratio > 100 || ratio < 0)
            {
                return string.Format("输入参数错误，{0}范围为0-100。", filename);
            }
            return string.Empty;
        }
    }
}
