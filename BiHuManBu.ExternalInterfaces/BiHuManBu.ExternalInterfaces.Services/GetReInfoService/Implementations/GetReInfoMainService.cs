using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.CacheServices;
using BiHuManBu.ExternalInterfaces.Services.DistributeService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Factories;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using log4net;
using MetricsLibrary;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Common;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class GetReInfoMainService : IGetReInfoMainService
    {
        #region 初始化
        private IManagerRoleRepository _managerRoleRepository;
        private readonly IGetAgentInfoService _getAgentInfoService;
        private IFiterAndRepeatDataService _fiterAndRepeatDataService;
        private IUserInfoRepository _userInfoRepository;
        private IAgentRepository _agentRepository;
        private static readonly string _isAddIdCardBack6 = ConfigurationManager.AppSettings["IsAddIdCardBack6"];
        private ILog logError = LogManager.GetLogger("ERROR");
        private readonly ICheckCarNeedDrivingInfoService _checkCarNeedDrivingInfoService;
        private IQuoteReqCarinfoRepository _quoteReqCarinfoRepository;
        private IMessageCenter _messageCenter;
        private ICarInsuranceCache _carInsuranceCache;
        private ICarRenewalRepository _carRenewalRepository;
        private ICarInfoRepository _carInfoRepository;
        private readonly IBatchRenewalRepository _batchRenewalRepository;
        private readonly ISentDistributedService _sentDistributedService;
        private IGetReInfoState _getReInfoState;
        private IToCenterFromReInfoService _toCenterFromReInfoService;
        public GetReInfoMainService(IManagerRoleRepository managerRoleRepository, IGetAgentInfoService getAgentInfoService, IFiterAndRepeatDataService fiterAndRepeatDataService,
            IUserInfoRepository userInfoRepository, IAgentRepository agentRepository, ICheckCarNeedDrivingInfoService checkCarNeedDrivingInfoService,
            IQuoteReqCarinfoRepository quoteReqCarinfoRepository, IMessageCenter messageCenter, ICarInsuranceCache carInsuranceCache, ICarRenewalRepository carRenewalRepository, ICarInfoRepository carInfoRepository,
            IBatchRenewalRepository batchRenewalRepository, ISentDistributedService sentDistributedService, IGetReInfoState getReInfoState,
            IToCenterFromReInfoService toCenterFromReInfoService)
        {
            _managerRoleRepository = managerRoleRepository;
            _getAgentInfoService = getAgentInfoService;
            _fiterAndRepeatDataService = fiterAndRepeatDataService;
            _userInfoRepository = userInfoRepository;
            _agentRepository = agentRepository;
            _checkCarNeedDrivingInfoService = checkCarNeedDrivingInfoService;
            _quoteReqCarinfoRepository = quoteReqCarinfoRepository;
            _messageCenter = messageCenter;
            _carInsuranceCache = carInsuranceCache;
            _carRenewalRepository = carRenewalRepository;
            _carInfoRepository = carInfoRepository;
            _batchRenewalRepository = batchRenewalRepository;
            _sentDistributedService = sentDistributedService;
            _getReInfoState = getReInfoState;
            _toCenterFromReInfoService = toCenterFromReInfoService;
        }
        #endregion

        public async Task<GetReInfoResponse> GetReInfo(GetReInfoRequest request)
        {
            var response = new GetReInfoResponse();
            var isReadCache = true;
            var topAgent = request.Agent;//顶级代理人

            //获取当前数据的角色addbygpj20180829            
            int roleType = _managerRoleRepository.GetRoleTypeByAgentId(request.ChildAgent > 0 ? request.ChildAgent : request.Agent);
            try
            {
                IBxAgent agentModel = _getAgentInfoService.GetAgentModelFactory(request.Agent);
                if (!agentModel.AgentCanUse())
                {
                    response.Status = HttpStatusCode.Forbidden;
                    if (agentModel.endDate.HasValue && agentModel.endDate.Value < DateTime.Now)
                    {
                        response.BusinessMessage = string.Format("参数校验错误，账号已过期。过期时间为：{0}", agentModel.endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        return response;
                    }
                    response.BusinessMessage = "参数校验错误，账号已禁用。";
                    return response;
                }
                //获取[顶级]代理人是否重复数据的标记addbygpj20180829
                int repeateQuote = agentModel.repeat_quote;
                #region userinfo逻辑
                //微信端逻辑 次级代理
                if (request.ChildAgent > 0)
                {
                    agentModel = _getAgentInfoService.GetAgentModelFactory(request.ChildAgent);
                    if (agentModel.AgentCanUse())
                    {
                        request.Agent = request.ChildAgent;
                    }
                    else
                    {
                        return new GetReInfoResponse
                        {
                            BusinessMessage = "您的账号已被禁用，如有疑问请联系管理员。",
                            Status = HttpStatusCode.Forbidden
                        };
                    }
                }
                #region 摄像头的黑名单、查重过滤
                bool isAdd = true;
                if (request.RenewalType == 3)
                {
                    //黑名单 //查重
                    CameraBackDataViewModel backData = _fiterAndRepeatDataService.GetFiterData(request.LicenseNo, topAgent, request.ChildAgent, request.CustKey, request.CityCode, request.RenewalCarType, repeateQuote, roleType);
                    if (backData != null)
                    {
                        if (backData.IsBlack)
                        {
                            //在黑名单列表中，无需继续往下，执行直接return
                            return new GetReInfoResponse
                            {
                                BusinessMessage = "车辆已进入黑名单，未执行续保操作",
                                Status = HttpStatusCode.Forbidden
                            };
                        }
                        else
                        {
                            //不在黑名单列表中
                            if (!string.IsNullOrEmpty(backData.Buid) && !string.IsNullOrEmpty(backData.Agent) && !string.IsNullOrEmpty(backData.OpenId))
                            {
                                isAdd = false;
                                request.Agent = int.Parse(backData.Agent);
                                request.CustKey = backData.OpenId;
                            }
                        }
                    }
                }
                #endregion
                #region 操作bx_userinfo对象
                bx_userinfo userinfo = UserinfoSearchFactory.Find(request, _userInfoRepository);
                if (userinfo == null)
                {
                    if (!string.IsNullOrEmpty(_isAddIdCardBack6) && _isAddIdCardBack6.Equals("1") && string.IsNullOrEmpty(request.SixDigitsAfterIdCard))
                    {
                        //如果身份证后6位字段为空，则取数据库中该车牌其他的记录的值
                        Stopwatch ssw = new Stopwatch();
                        ssw.Start();
                        //查重复数据
                        RepeatUserInfoModel model = new RepeatUserInfoModel();
                        List<RepeatUserInfoModel> listRepeat = _userInfoRepository.GetLicenseRepeat(topAgent, request.LicenseNo);
                        if (listRepeat.Any())
                        {
                            model = listRepeat.Where(l => l.SixDigitsAfterIdCard != null && l.SixDigitsAfterIdCard.Trim() != "").OrderByDescending(o => o.UpdateTime).FirstOrDefault();
                        }
                        //取一条赋值后六位
                        if (model != null && model.Buid > 0)
                        {
                            request.SixDigitsAfterIdCard = model.SixDigitsAfterIdCard;
                        }
                        logError.Info(string.Format("buid为{0},后6位插入时间{1}", 0, ssw.ElapsedMilliseconds));
                        ssw.Stop();
                    }
                    userinfo = UserinfoMakeFactory.Save(request, roleType, _userInfoRepository, agentModel.TopAgentId);
                    if (request.IsLastYearNewCar == 1)
                    {
                        isReadCache = await _checkCarNeedDrivingInfoService.GetInfo(userinfo);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(_isAddIdCardBack6) && _isAddIdCardBack6.Equals("1") && string.IsNullOrEmpty(request.SixDigitsAfterIdCard) && string.IsNullOrEmpty(userinfo.SixDigitsAfterIdCard))
                    {
                        //如果身份证后6位字段为空，则取数据库中该车牌其他的记录的值
                        Stopwatch ssw = new Stopwatch();
                        ssw.Start();
                        //查重复数据
                        RepeatUserInfoModel model = new RepeatUserInfoModel();
                        List<RepeatUserInfoModel> listRepeat = _userInfoRepository.GetLicenseRepeat(topAgent, request.LicenseNo);
                        if (listRepeat.Any())
                        {
                            model = listRepeat.Where(l => l.SixDigitsAfterIdCard != null && l.SixDigitsAfterIdCard.Trim() != "").OrderByDescending(o => o.UpdateTime).FirstOrDefault();
                        }
                        //取一条赋值后六位
                        if (model != null && model.Buid > 0)
                        {
                            request.SixDigitsAfterIdCard = model.SixDigitsAfterIdCard;
                        }
                        logError.Info(string.Format("buid为{0},后6位插入时间{1}", userinfo.Id, ssw.ElapsedMilliseconds));
                        ssw.Stop();
                    }
                    userinfo = UserinfoMakeFactory.Update(request, userinfo, _userInfoRepository, agentModel.TopAgentId);
                    bool isNeedCheckNeed = !(request.IsLastYearNewCar == 1 & (!string.IsNullOrWhiteSpace(request.CarVin) && !string.IsNullOrWhiteSpace(request.EngineNo)));
                    if (request.IsLastYearNewCar == 1)
                    {
                        //超过半年的数据要重新调用车型信息
                        if (userinfo.UpdateTime.HasValue && userinfo.UpdateTime.Value.AddMonths(6) <= DateTime.Now)
                        {
                            isReadCache = await _checkCarNeedDrivingInfoService.GetInfo(userinfo);
                        }
                        else
                        {
                            if (userinfo.NeedEngineNo == 1)//以前没有拿到行驶证信息
                            {
                                if (isNeedCheckNeed)//没有拿到车架号及发动机号
                                {
                                    isReadCache = await _checkCarNeedDrivingInfoService.GetInfo(userinfo);
                                }
                            }
                        }
                    }
                }
                #endregion
                long buid = userinfo.Id;
                #endregion
                #region bx_quotereq_carinfo 逻辑
                var reqCacheKey = string.Format("{0}-{1}", buid, "reqcarinfo");
                var quotereq = _quoteReqCarinfoRepository.Find(buid);
                if (quotereq == null)
                {
                    QuoteReqCarInfoMakeFactory.Save(buid, request.IsLastYearNewCar, _quoteReqCarinfoRepository);
                }
                else
                {
                    QuoteReqCarInfoMakeFactory.Update(quotereq, request.IsLastYearNewCar, _quoteReqCarinfoRepository);
                }
                #endregion
                if (request.RenewalSource != -1)
                {
                    #region 调用中心返回response信息
                    response = await _toCenterFromReInfoService.PushCenterService(request, buid, reqCacheKey);
                    #endregion
                }
                if (response.BusinessStatus == 4 || request.RenewalSource == -1)
                {
                    bool isOver = false;
                    #region  读库数据
                    isOver = _getReInfoState.GetState(buid);
                    response = new GetReInfoResponse();
                    response.UserInfo = _userInfoRepository.FindByBuid(buid);
                    if (!isOver)
                    {
                        if (response.UserInfo.NeedEngineNo == 1)
                        {
                            //需要完善行驶证信息
                            response.BusinessStatus = 2;
                            response.BusinessMessage = "需要完善行驶证信息（车辆信息和险种都没有获取到）";
                        }
                        if (response.UserInfo.NeedEngineNo == 0 && response.UserInfo.RenewalStatus != 1)
                        {
                            //获取车辆信息成功，但获取险种失败
                            response.BusinessStatus = 3;
                            response.BusinessMessage = "获取车辆信息成功(车架号，发动机号，品牌型号及初登日期)，险种获取失败";
                        }
                        if ((response.UserInfo.NeedEngineNo == 0 && response.UserInfo.LastYearSource > -1) || response.UserInfo.RenewalStatus == 1)
                        {
                            //续保成功
                            response.BusinessStatus = 1;
                            response.BusinessMessage = "续保成功";
                            response.SaveQuote = _carRenewalRepository.FindByLicenseno(request.LicenseNo);
                            response.CarInfo = _carInfoRepository.Find(response.UserInfo.LicenseNo);
                        }
                    }
                    else
                    {
                        if (response.UserInfo.LastYearSource == -1)
                        {
                            response.BusinessStatus = -10002;
                            response.BusinessMessage = "获取续保信息失败";
                        }
                    }
                    response.Status = HttpStatusCode.OK;
                    #endregion
                }
                response.UserInfo.Id = buid;
                #region 发送分配请求
                string moldName = response.UserInfo != null ? response.UserInfo.MoldName : string.Empty;//车型
                string businessExpireDate = string.Empty;//商业险到期
                string forceExpireDate = string.Empty;//交强险到期
                if (response.SaveQuote != null)
                {
                    businessExpireDate = response.SaveQuote.LastBizEndDate.HasValue
                        ? response.SaveQuote.LastBizEndDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : string.Empty;
                    forceExpireDate = response.SaveQuote.LastForceEndDate.HasValue
                        ? response.SaveQuote.LastForceEndDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : string.Empty;
                }
                //调用重新分配的接口
                if (request.RenewalType == 3)
                {
                    /*
                     * 调用院院的分配方法，将数据推过去即可，无需返回值
                     * 20180820修改，1,去掉是否调用摄像头进店（api.config的IsUrlDistributed标识）判断；2,修改分配方法，调用刘松年的新版接口
                     */
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                    Task.Factory.StartNew(() =>
                    {
                        _sentDistributedService.SentDistributed(response.BusinessStatus, moldName, buid, topAgent, request.ChildAgent, userinfo.Agent, request.CityCode, request.LicenseNo, request.RenewalType, userinfo.RenewalType.Value, businessExpireDate, forceExpireDate, false, request.ChildAgent, request.CameraId, isAdd, repeateQuote, roleType, request.CustKey, request.CityCode);
                    });
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                }
                #endregion
                #region 修改批续进来的状态
                if (response.BusinessStatus == 1)
                {
                    UpdateBatchRenewalStatus(buid, 1);
                }
                else if (response.BusinessStatus == 2)
                {
                    UpdateBatchRenewalStatus(buid, 2);
                }
                else if (response.BusinessStatus == 3)
                {
                    UpdateBatchRenewalStatus(buid, 4);
                }
                else
                {
                    UpdateBatchRenewalStatus(buid, 2);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MetricUtil.UnitReports("renewal_service");
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("续保请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + ",请求对象信息：" + request.ToJson() + ";返回对象信息" + response.ToJson());
            }
            return response;
        }


        /// <summary>
        /// 更改续保信息状态
        /// 以前调用crm接口的EnterpriseBatchRenewal/UpdateItemStatus
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="status"></param>
        private void UpdateBatchRenewalStatus(long buid, int status)
        {
            int isSuccess = _batchRenewalRepository.UpdateItemStatus(buid, status);
        }
    }
}
