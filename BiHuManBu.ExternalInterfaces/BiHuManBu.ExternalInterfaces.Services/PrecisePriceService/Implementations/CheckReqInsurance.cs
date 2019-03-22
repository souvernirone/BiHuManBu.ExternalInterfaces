using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using ServiceStack.Text;
using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class CheckReqInsurance : ICheckReqInsurance
    {
        private readonly ICheckShebei _checkShebei;
        private readonly ICheckXianZhong _checkXianZhong;
        public CheckReqInsurance(ICheckShebei checkShebei, ICheckXianZhong checkXianZhong)
        {
            _checkShebei = checkShebei;
            _checkXianZhong = checkXianZhong;
        }
        public BaseViewModel CheckInsurance(PostPrecisePriceRequest request)
        {
            BaseViewModel viewModel = new BaseViewModel();

            #region 短时起保业务
            if (!string.IsNullOrWhiteSpace(request.BizShortEndDate))
            {
                DateTime bizenddate = request.BizShortEndDate.UnixTimeToDateTime();
                if (bizenddate > DateTime.Now.AddYears(1) || bizenddate < DateTime.Now.AddDays(1))
                {
                    viewModel.StatusMessage = "输入参数错误，如果有商业险短时起保业务，请确保商业险截止日期在明天到一年后的今天范围之内";
                    return FailedViewModel(viewModel, -10000);
                }
            }
            #endregion
            #region 实时起保的时间需要大于当前时间1小时，
            if (!string.IsNullOrWhiteSpace(request.BizTimeStamp) && request.ForceTax != 2)
            {//!=2是单交强
                if (DateTime.Parse(request.BizTimeStamp.UnixTimeToDateTime().ToString("yyyy-MM-dd HH") + ":00:00") < DateTime.Now.AddMinutes(60))
                {
                    viewModel.StatusMessage = "输入参数错误，如果选择实时起保，商业起保时间需晚于系统时间60分钟";
                    return FailedViewModel(viewModel, -10000);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.ForceTimeStamp) && request.ForceTax != 0)
            {//!=0是单商业
                if (DateTime.Parse(request.ForceTimeStamp.UnixTimeToDateTime().ToString("yyyy-MM-dd HH") + ":00:00") < DateTime.Now.AddMinutes(60))
                {
                    viewModel.StatusMessage = "输入参数错误，如果选择实时起保，交强起保时间需晚于系统时间60分钟";
                    return FailedViewModel(viewModel, -10000);
                }
            }
            #endregion

            #region 折扣价格
            if (request.NegotiatePrice > 0)
            {
                if (string.IsNullOrWhiteSpace(request.BizStartDate) && string.IsNullOrWhiteSpace(request.BizTimeStamp))
                {
                    viewModel.StatusMessage = "输入参数错误，如果有浮动价格，请完善商业险起保日期、初登日期和购置价格（接口6获得的价格）";
                    return FailedViewModel(viewModel, -10000);
                }
                if (string.IsNullOrWhiteSpace(request.RegisterDate))
                {
                    viewModel.StatusMessage = "输入参数错误，如果有浮动价格，请完善商业险起保日期、初登日期和购置价格（接口6获得的价格）";
                    return FailedViewModel(viewModel, -10000);
                }
                if (request.PurchasePrice <= 0 && request.PurchasePrice > 7000000)
                {
                    viewModel.StatusMessage = "输入参数错误，如果有浮动价格，请完善商业险起保日期、初登日期和购置价格（接口6获得的价格）";
                    return FailedViewModel(viewModel, -10000);
                }
                string bizdate = string.Empty;
                //时间戳支持
                if (!string.IsNullOrWhiteSpace(request.BizTimeStamp))
                {
                    bizdate = request.BizTimeStamp.UnixTimeToDateTime().ToString("yyyy-MM-dd");
                }
                else if (!string.IsNullOrWhiteSpace(request.BizStartDate))
                {
                    bizdate = request.BizStartDate;
                }
                var ticks = DateTime.Parse(bizdate).Ticks - DateTime.Parse(request.RegisterDate).Ticks;
                if (ticks < 0)
                {
                    viewModel.StatusMessage = "输入参数错误，请确定您的商业险起保日期和初等日期的先后关系";
                    return FailedViewModel(viewModel, -10000);
                }
                //DepreciationPriceViewModel model = await GetPrice(request.BizTimeStamp, request.RegisterDate, request.PurchasePrice,request.CarType);
                //if (model.BusinessStatus == 1)
                //{
                //    if (request.NegotiatePrice < model.Item.DownPrice || request.NegotiatePrice > model.Item.UpPrice)
                //    {
                //        viewModel.StatusMessage = "请检查您的协商价格，该值应该在"+model.Item.DownPrice+"~"+model.Item.UpPrice+"范围内";
                //        return viewModel;
                //    }
                //}
                //else
                //{
                //    viewModel.StatusMessage = "调用保险中心折扣价格出现异常";
                //    return viewModel;
                //}
            }
            #endregion
            #region //单商业或者 双险的时候 ，商业险必须上
            if (request.ForceTax == 0 || request.ForceTax == 1)
            {
                if (request.CheSun == 0 && request.SanZhe == 0 && request.DaoQiang == 0 && request.SiJi == 0 && request.ChengKe == 0)
                {
                    var s = string.Empty;
                    if (request.ForceTax == 0)
                    {
                        s = "您选择的是单商业，请选择相关的商业险险种组合去报价";
                    }
                    if (request.ForceTax == 1)
                    {
                        s = "您选择的是交强+商业险，请选择相关的商业险险种组合去报价";
                    }
                    viewModel.StatusMessage = "输入参数错误，" + s;
                    return FailedViewModel(viewModel, -10000);
                }
            }
            #endregion
            //三责节假日险特殊判断，只有私家车和上三者的才能报
            //if (!(request.SanZhe > 0 && request.CarUsedType == 1))
            //{
            //    if (request.SanZheJieJiaRi > 0)
            //    {
            //        viewModel.StatusMessage = "输入参数错误，只有家庭自用车选择了第三者责任险，才可以选择三责险附加法定节假日限额翻倍险";
            //        return FailedViewModel(viewModel, -10000);
            //    }
            //}
            #region 修理期间费用补偿险
            if ((request.FybcDays < 0 || request.FybcDays > 90) || (request.Fybc < 0 || request.Fybc > 300))
            {
                viewModel.StatusMessage = "输入参数错误，" + "修理期间费用补偿险，费用只能是 100,200,300，天数必须在(1-90)范围内";
                return FailedViewModel(viewModel, -10000);
            }

            if ((request.Fybc > 0 && request.FybcDays <= 0) || (request.Fybc <= 0 && request.FybcDays > 0))
            {
                viewModel.StatusMessage = "输入参数错误，" + "修理期间费用补偿险，费用和金额都必须大于0";
                return FailedViewModel(viewModel, -10000);
            }
            if (request.Fybc > 0)
            {
                List<double> rge = new List<double>() { 100, 200, 300 };
                if (!rge.Contains(request.Fybc))
                {
                    viewModel.StatusMessage = "输入参数错误，" + "修理期间费用补偿险，费用只能是 100,200,300";
                    return FailedViewModel(viewModel, -10000);
                }
            }
            #endregion
            #region 新增设备校验
            if (request.SheBeiSunshi == 1)
            {
                var shebeimsg = _checkShebei.CheckRequestShebei(request);
                if (!string.IsNullOrWhiteSpace(shebeimsg))
                {
                    viewModel.StatusMessage = shebeimsg;
                    return FailedViewModel(viewModel, -10000);
                }
            }
            #endregion
            #region 其他一般险种校验
            var checkMsg = _checkXianZhong.CheckRequestXianZhong(request);
            if (!string.IsNullOrWhiteSpace(checkMsg))
            {
                string msg = checkMsg;
                viewModel.StatusMessage = "输入参数错误，" + msg;
                //靠，不知道这里谁改成-10001的，导致校验直接跳过去报价了。
                return FailedViewModel(viewModel, -10000);
            }
            #endregion

            //特约险判断
            if (!string.IsNullOrWhiteSpace(request.SpecialOption))
            {
                List<long> sources = SourceGroupAlgorithm.ParseSource(request.QuoteGroup);
                List<bx_specialoption> specialOptionList = new List<bx_specialoption>();
                List<SpecialOption> requestList = request.SpecialOption.FromJson<List<SpecialOption>>();
                foreach (var item in requestList)
                {
                    if (!sources.Contains(item.Source))
                    {
                        viewModel.StatusMessage = "输入参数错误，有部分特约不属于所指定的报价公司";
                        return FailedViewModel(viewModel, -10000);
                    }
                }
            }
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
    }
}
