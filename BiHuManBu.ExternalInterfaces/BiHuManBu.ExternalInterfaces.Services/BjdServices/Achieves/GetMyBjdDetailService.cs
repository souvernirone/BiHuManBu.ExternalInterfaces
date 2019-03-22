using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class GetMyBjdDetailService : IGetMyBjdDetailService
    {
        private ICheckRequestService _checkRequestService;
        private IGetDateService _getDateService;
        private ISetActivitiesService _setActivitiesService;
        private ISetAgentService _setAgentService;
        private ISetBaseInfoService _setBaseInfoService;
        private ISetCarInfoService _setCarInfoService;
        private ISetClaimsService _setClaimsService;
        private ISetDateService _setDateService;
        private ISetOrderService _setOrderService;
        private ISetPrecisePriceItemService _setPrecisePriceItemService;
        private ISetQuoteReqService _setQuoteReqService;
        private IImagesRepository _imagesRepository;
        private IUserInfoRepository _userInfoRepository;
        private ILog logErr;
        private ILog logInfo;
        public GetMyBjdDetailService(ICheckRequestService checkRequestService, IGetDateService getDateService, ISetActivitiesService setActivitiesService, ISetAgentService setAgentService, ISetBaseInfoService setBaseInfoService, ISetCarInfoService setCarInfoService, ISetClaimsService setClaimsService, ISetDateService setDateService, ISetOrderService setOrderService, ISetPrecisePriceItemService setPrecisePriceItemService, ISetQuoteReqService setQuoteReqService, IImagesRepository imagesRepository, IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
            _imagesRepository = imagesRepository;
            _checkRequestService = checkRequestService;
            _getDateService = getDateService;
            _setActivitiesService = setActivitiesService;
            _setAgentService = setAgentService;
            _setBaseInfoService = setBaseInfoService;
            _setCarInfoService = setCarInfoService;
            _setClaimsService = setClaimsService;
            _setDateService = setDateService;
            _setOrderService = setOrderService;
            _setPrecisePriceItemService = setPrecisePriceItemService;
            _setQuoteReqService = setQuoteReqService;
            logErr = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");
        }
        public MyBaoJiaViewModel GetMyBjdDetail(GetMyBjdDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            DateTime date1 = DateTime.Now;
            bx_userinfo userinfo = _userInfoRepository.FindByBuid(request.Buid);
            //校验
            var my = _checkRequestService.CheckRequest(userinfo, request);
            if (my.BusinessStatus != 1)
            {
                return my;
            }
            my.BusinessStatus = 1;
            //获取修改时间
            my.UpdateTime = userinfo.LatestQuoteTime.HasValue ? userinfo.LatestQuoteTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";

            //获取在该报价单在预约单中记录
            my = _setOrderService.SetOrder(my, userinfo);

            //从userinfo获取关系人 和一些其他配置
            my = _setBaseInfoService.SetBaseInfo(my, userinfo);
            List<bx_quoteresult_carinfo> quoteresultCarinfo;
            //获取车辆信息bx_quoteresult_carinfo
            my = _setCarInfoService.SetCarInfo(my, userinfo, out  quoteresultCarinfo);

            //取商业险和交强险开始时间
            string postForceStartDate;//交强险起始时间
            string postBizStartDate;
            postForceStartDate = _getDateService.GetDate(userinfo, out postBizStartDate);
            //请求报价的信息  
            my = _setQuoteReqService.SetQuoteReq(my, userinfo, ref  postBizStartDate, ref postForceStartDate);
            //给交强商业起始时间、到期时间赋值
            my = _setDateService.SetDate(my, userinfo, postBizStartDate, postForceStartDate);

            my = _setPrecisePriceItemService.SetPrecisePriceItem(my, userinfo, request, quoteresultCarinfo,my.SeatCount??0);

            //获取出险记录
            my = _setClaimsService.SetClaims(my, request, userinfo);
            //获取优惠活动
            my = _setActivitiesService.SetActivities(my, request);
            //获取图片
            my.Images = _imagesRepository.FindByBuid(userinfo.Id);
            //是否是临时被保险人
            my.IsTempInsured = GetTempInsured(userinfo.Id);
            //赋值代理人信息,并初始化费率 展示费率
            my = _setAgentService.SetAgent(my, userinfo, request);
            my.QuoteTime = userinfo.LatestQuoteTime.HasValue ? userinfo.LatestQuoteTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            my.LatestRenewalTime = userinfo.LatestRenewalTime.HasValue ? userinfo.LatestRenewalTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            if (request.ShowCarProperties <= 0)
            {
                my.IsLoans = null;
                my.TransferDate = null;
            }
            return my;
        }
        private int GetTempInsured(long buid)
        {
            //return 0;
            string _url = ConfigurationManager.AppSettings["SystemCrmUrl"];
            //string strUrl = string.Format("{0}/api/TempInsuredInfo/GetTempInsuredInfoAsync?agentId={1}&licenseNo={2}&insuredName={3}", _url, agentId, licenseNo, insuredName);
            //string strUrl = string.Format("{0}/api/TempInsuredInfo/GetTempInsuredInfoAsync?buId={1}", _url, buid);
            string strUrl = string.Format("{0}/api/TempUserInfo/GetTempRelationAsync?buId={1}&temptype=2", _url, buid);
            try
            {
                using (var client = new HttpClient())
                {
                    var clientResult = client.GetAsync(strUrl).Result;
                    if (clientResult.IsSuccessStatusCode)
                    {
                        var httpMsg = clientResult.Content.ReadAsStringAsync().Result;
                        var result = httpMsg.FromJson<TempInsuredResponse>();
                        if (result != null)
                        {
                            logInfo.Info(string.Format("CRM获取被保险人请求返回值:url:{0} ;data:{1}", strUrl, result.ToJson()));
                            if (result.tempUserInfo.Any())
                            {
                                return result.tempUserInfo[0].TagTypeTempInsured;
                            }
                        }
                        return 0;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                logErr.Info("Get请求接口数据异常，请求串为：" + strUrl + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
                return 0;
            }
        }
    }
}
