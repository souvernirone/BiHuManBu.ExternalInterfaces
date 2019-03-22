using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using WebApiThrottle;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class ChargeController:ApiController
    {
        private ILog logInfo;
        private ILog logError;

        private IChargeService _chargeService;

        public ChargeController(IChargeService chargeService)
        {
            _chargeService = chargeService;
            logInfo = LogManager.GetLogger("INFO");
            logError = LogManager.GetLogger("ERROR");
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetCarInfo([FromUri]UpdateChargeRequest request)
        {
            logInfo.Info(string.Format("【收费服务】获取车辆信息接口请求串：{0}", Request.RequestUri));
            CarChargeViewModel viewModel = new CarChargeViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，"+msg;
                return viewModel.ResponseToJson();
            }
            if (!request.LicenseNo.IsValidLicenseno())
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "参数校验错误，请检查车牌号";
                return viewModel.ResponseToJson();
            }
            try
            {
                var response = await _chargeService.Update(request, Request.GetQueryNameValuePairs());
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    return viewModel.ResponseToJson();
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务发生异常";
                }
                else
                {
                    if (response.ErrCode == 0)
                    {
                        
                        viewModel.Item = response.ConvertToViewModel();
                        viewModel.BusinessStatus = 1;

                    }
                    else if (response.ErrCode == -2)
                    {
                        viewModel.BusinessStatus = 2;//余额不足
                    }
                    else if (response.ErrCode == -1)
                    {
                        viewModel.BusinessStatus = 3;//没有获取到
                    }
                }
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetClaimInfo([FromUri]UpdateChargeRequest request)
        {
            logInfo.Info(string.Format("【收费服务】获取车辆出险信息接口请求串：{0}", Request.RequestUri));
            CarClaimViewModel viewModel = new CarClaimViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            if (!request.LicenseNo.IsValidLicenseno())
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "参数校验错误，请检查车牌号";
                return viewModel.ResponseToJson();
            }
            try
            {
                var response = await _chargeService.UpdateClaim(request, Request.GetQueryNameValuePairs());
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    return viewModel.ResponseToJson();
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务发生异常";
                }
                else
                {
                    if (response.ErrCode == 0)
                    {
                        viewModel= response.List.ConvertToViewModel();
                        viewModel.LicenseNo = request.LicenseNo;
                        viewModel.TotalCount = response.TotalCount;
                        viewModel.UsedCount = response.UsedCount;
                        viewModel.BusinessStatus = 1;

                    }
                    else if (response.ErrCode == -2)
                    {
                        viewModel.BusinessStatus = 2;//余额不足
                    }
                    else if (response.ErrCode == -1)
                    {
                        viewModel.BusinessStatus = 3;//没有获取到
                    }
                }
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return viewModel.ResponseToJson();
        }


        [HttpGet]
        [EnableThrottling()]
        public async Task<HttpResponseMessage> GetReportClaim([FromUri] GetReportClaimRequest request)
        {
            logInfo.Info(string.Format("【收费服务】获取车辆【报备】信息接口请求串：{0}", Request.RequestUri));
            var viewModel = new ReportClaimHistoryViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10000;
                string msg = ModelState.Values.Where(item => item.Errors.Count == 1).Aggregate(string.Empty, (current, item) => current + (item.Errors[0].ErrorMessage + ";   "));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            if (!request.LicenseNo.IsValidLicenseno())
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "参数校验错误，请检查车牌号";
                return viewModel.ResponseToJson();
            }
            try
            {
                var response = await _chargeService.GetReportClaim(request, Request.GetQueryNameValuePairs());

                if (response.Status == HttpStatusCode.RequestTimeout)
                {
                    viewModel.BusinessStatus = -10004;
                    viewModel.StatusMessage = "请求超时 ，请稍候再试";
                    return viewModel.ResponseToJson();
                }
                if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
                {
                    viewModel.BusinessStatus = -10001;
                    viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                    return viewModel.ResponseToJson();
                }
                if (response.Status == HttpStatusCode.ExpectationFailed)
                {
                    viewModel.BusinessStatus = -10003;
                    viewModel.StatusMessage = "服务发生异常";
                }
                else
                {
                    viewModel.ReportClaims = new List<ReportClaim>();
                    viewModel.HistoryContracts = new List<HistoryContract>();
                    if (response.ErrCode == 0)
                    {
                        viewModel.ReportClaims = response.ClaimList.ConvertToReportClaimsViewModel();
                        viewModel.HistoryContracts = response.ContractList.ConvertToHistoryContract();
                        viewModel.LicenseNo = request.LicenseNo;
                        viewModel.TotalCount = response.TotalCount;
                        viewModel.UsedCount = response.UsedCount;
                        viewModel.BusinessStatus = 1;
                        viewModel.StatusMessage = "成功";

                    }
                    else if (response.ErrCode == -5)
                    {
                        viewModel.BusinessStatus = -5;//余额不足
                        viewModel.StatusMessage = "余额不足";
                    }
                    else if (response.ErrCode == -4)
                    {
                        viewModel.BusinessStatus = -4;//
                        viewModel.StatusMessage = "没有获取到理赔信息";
                    }
                    else if (response.ErrCode == -3)
                    {
                        viewModel.BusinessStatus = -3;//
                        viewModel.StatusMessage = "没有获取历史承保信息信息";
                    }
                    else if (response.ErrCode == -1202)
                    {
                        viewModel.BusinessStatus = -1;//请提供正确的车架号和发动机号
                        viewModel.StatusMessage = "请提供正确的车架号和发动机号";
                    }
                    else if (response.ErrCode == -2)
                    {
                        viewModel.BusinessStatus = -2;//没有获取到
                        viewModel.StatusMessage = "没有查询到车辆信息";
                    }
                }
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return viewModel.ResponseToJson();
        } 
    }
}