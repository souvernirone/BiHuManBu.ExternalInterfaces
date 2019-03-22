using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
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
    public class GetBjdDetailFromHistoryService : IGetBjdDetailFromHistoryService
    {
        private ICheckRequestService _checkRequestService;
        private IGetDateNewService _getDateNewService;
        private ISetActivitiesService _setActivitiesService;
        private ISetAgentService _setAgentService;
        private ISetBaseInfoService _setBaseInfoService;
        private ISetCarInfoService _setCarInfoService;
        private ISetClaimsService _setClaimsService;
        private ISetDateService _setDateService;
        private ISetOrderNewService _setOrderNewService;
        private ISetPrecisePriceItemNewService _setPrecisePriceItemNewService;
        private ISetQuoteReqNewService _setQuoteReqNewService;
        private IImagesRepository _imagesRepository;
        private IUserInfoRepository _userInfoRepository;
        private IGetModelsFromQuoteHistory _getModelsFromQuoteHistory;
        private ISetBaseInfoHistoryService _setBaseInfoHistoryService;
        private IQuotehistoryRelatedRepository _quotehistoryRelatedRepository;
        private ILog logErr = LogManager.GetLogger("ERROR");
        private ILog logInfo = LogManager.GetLogger("INFO");

        public GetBjdDetailFromHistoryService(ICheckRequestService checkRequestService, IGetDateNewService getDateNewService, ISetActivitiesService setActivitiesService,
            ISetAgentService setAgentService, ISetBaseInfoService setBaseInfoService, ISetCarInfoService setCarInfoService, ISetClaimsService setClaimsService,
            ISetDateService setDateService, ISetOrderNewService setOrderNewService, ISetPrecisePriceItemNewService setPrecisePriceItemNewService, ISetQuoteReqNewService setQuoteReqNewService,
            IImagesRepository imagesRepository, IUserInfoRepository userInfoRepository, IGetModelsFromQuoteHistory getModelsFromQuoteHistory, ISetBaseInfoHistoryService setBaseInfoHistoryService,
            IQuotehistoryRelatedRepository quotehistoryRelatedRepository)
        {
            _userInfoRepository = userInfoRepository;
            _imagesRepository = imagesRepository;
            _checkRequestService = checkRequestService;
            _getDateNewService = getDateNewService;
            _setActivitiesService = setActivitiesService;
            _setAgentService = setAgentService;
            _setBaseInfoService = setBaseInfoService;
            _setCarInfoService = setCarInfoService;
            _setClaimsService = setClaimsService;
            _setDateService = setDateService;
            _setOrderNewService = setOrderNewService;
            _setPrecisePriceItemNewService = setPrecisePriceItemNewService;
            _setQuoteReqNewService = setQuoteReqNewService;
            _getModelsFromQuoteHistory = getModelsFromQuoteHistory;
            _setBaseInfoHistoryService = setBaseInfoHistoryService;
            _quotehistoryRelatedRepository = quotehistoryRelatedRepository;
        }

        public MyBaoJiaViewModel GetMyBjdDetail(GetBjdDetailFromHistoryRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            DateTime date1 = DateTime.Now;
            bx_userinfo userinfo = _userInfoRepository.FindByBuid(request.Buid);

            var my = new MyBaoJiaViewModel();
            //校验
            //var my = _checkRequestService.CheckRequest(userinfo, request);
            //if (my.BusinessStatus != 1)
            //{
            //    return my;
            //}
            Tuple<bx_savequote, bx_quotereq_carinfo, List<bx_quoteresult>, List<bx_submit_info>, string, List<int>> historyList = _getModelsFromQuoteHistory.GetModels(request.Buid, request.GroupSpan);

            List<long> quote01 = new List<long>();//报价1+0的数据 成功+失败
            List<long> quote1 = new List<long>();//报价1的数据 成功
            if (historyList.Item4 != null && historyList.Item4.Any())
            {
                List<int> intquote1 = historyList.Item4.Where(w => w.quote_status > 0).Select(l => l.source.Value).ToList();
                if (intquote1 != null && intquote1.Any())
                {
                    foreach (var i in intquote1)
                    {
                        quote1.Add(i);
                    }
                }
            }
            if (historyList.Item6.Any())
            {
                foreach (var i in historyList.Item6)
                {
                    quote01.Add(i);
                }
            }

            //获取修改时间
            my.UpdateTime = historyList.Item5;

            //获取在该报价单在预约单中记录
            my = _setOrderNewService.SetOrder(my, userinfo);

            //从userinfo获取关系人 和一些其他配置
            var relationitem = _quotehistoryRelatedRepository.GetModel(request.Buid, request.GroupSpan);
            if (relationitem != null && relationitem.id > 0)
            {
                my = _setBaseInfoHistoryService.SetBaseInfoHistory(my, userinfo, relationitem);
            }
            else
            {
                my = _setBaseInfoService.SetBaseInfo(my, userinfo);
            }

            List<bx_quoteresult_carinfo> quoteresultCarinfo;
            //获取车辆信息
            my = _setCarInfoService.SetCarInfo(my, userinfo, out quoteresultCarinfo);

            //取商业险和交强险开始时间
            Tuple<string, string> startDate = _getDateNewService.GetDate(historyList.Item3);
            string postBizStartDate = startDate.Item1;//商业险起始时间
            string postForceStartDate = startDate.Item2;//交强险起始时间

            //请求报价的信息  
            my = _setQuoteReqNewService.SetQuoteReq(my, quote1, historyList.Item2, ref postBizStartDate, ref postForceStartDate);
            //给交强商业起始时间、到期时间赋值
            //顺便给出险次数赋值。因为平安从bx_claimdetails没办法拿值，中心沟通从此表取值
            my = _setDateService.SetDate(my, userinfo, postBizStartDate, postForceStartDate);

            GetMyBjdDetailRequest request1 = new GetMyBjdDetailRequest()
            {
                Agent = request.Agent,
                Source = -1,
                NewRate = null
            };

            my = _setPrecisePriceItemNewService.SetPrecisePriceItem(my, userinfo, quoteresultCarinfo, quote01, quote1.Count == 0, historyList.Item1, historyList.Item3, historyList.Item4);

            //获取出险记录
            //my = _setClaimsService.SetClaims(my, request, userinfo);
            //获取优惠活动
            //my = _setActivitiesService.SetActivities(my, request);
            //获取图片
            //my.Images = _imagesRepository.FindByBuid(userinfo.Id);
            my.Images = new List<bx_images>();
            //是否是临时被保险人
            my.IsTempInsured = GetTempInsured(userinfo.Id);
            //赋值代理人信息,并初始化费率 展示费率
            //my = _setAgentService.SetAgent(my, userinfo, request);
            my.QuoteTime = historyList.Item5;//userinfo.LatestQuoteTime.HasValue ? userinfo.LatestQuoteTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            my.LatestRenewalTime = "";// userinfo.LatestRenewalTime.HasValue ? userinfo.LatestRenewalTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
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
