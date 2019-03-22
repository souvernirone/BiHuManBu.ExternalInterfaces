using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class GetIntelligentReInfoService : CommonBehaviorService, IGetIntelligentReInfoService
    {
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        private IAgentRepository _agentRepository;
        private ICacheHelper _cacheHelper;
        private readonly IGetAgentInfoService _getAgentInfoService;
        private readonly ICarInfoRepository _carInfoRepository;
        private readonly ICarRenewalRepository _carRenewalRepository;
        private readonly IGetIntelligentInsurance _getIntelligentInsurance;
        private readonly IRenewalStatusService _renewalStatusService;

        public GetIntelligentReInfoService(IAgentRepository agentRepository, ICacheHelper cacheHelper,
            IGetAgentInfoService getAgentInfoService, ICarInfoRepository carInfoRepository, ICarRenewalRepository carRenewalRepository,
            IGetIntelligentInsurance getIntelligentInsurance, IRenewalStatusService renewalStatusService) : base(agentRepository, cacheHelper)
        {
            _agentRepository = agentRepository;
            _getAgentInfoService = getAgentInfoService;
            _carInfoRepository = carInfoRepository;
            _carRenewalRepository = carRenewalRepository;
            _getIntelligentInsurance = getIntelligentInsurance;
            _renewalStatusService = renewalStatusService;

        }
        public async Task<GetIntelligentReInfoResponse> GetIntelligentReInfo(GetIntelligentReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetIntelligentReInfoResponse();
            var isReadCache = true;
            try
            {
                //代理人校验
                IBxAgent agentModel = _getAgentInfoService.GetAgentModelFactory(request.Agent);
                if (!agentModel.AgentCanUse())
                {
                    response.Status = HttpStatusCode.Forbidden;
                    if (agentModel.endDate.HasValue && agentModel.endDate.Value < DateTime.Now)
                    {
                        response.ErrMsg = string.Format("参数校验错误，账号已过期。过期时间为：{0}", agentModel.endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        return response;
                    }
                    response.ErrMsg = "参数校验错误，账号已禁用。";
                    return response;
                }
                if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                //微信端逻辑 次级代理
                if (request.ChildAgent > 0)
                {
                    var item = _agentRepository.GetAgent(request.ChildAgent);
                    if (item != null && item.IsUsed == 1)
                    {
                        request.Agent = request.ChildAgent;
                    }
                    else
                    {
                        return new GetIntelligentReInfoResponse
                        {
                            ErrMsg = "您的账号已被禁用，如有疑问请联系管理员。",
                            Status = HttpStatusCode.Forbidden
                        };
                    }
                }
                ///先从库里读取,没有在走后续流程
                if (request.IsCarVin == 1)
                {
                    //根据车架号查询
                    response.CarInfo = _carInfoRepository.FindVinCarInfo(request.CarVin, request.RenewalCarType);

                }
                else
                {
                    //车牌号查询
                    response.CarInfo = _carInfoRepository.FindOrderDate(request.LicenseNo, request.RenewalCarType);
                }

                response.Status = HttpStatusCode.OK;
                if (response.CarInfo != null)
                {
                    response.ErrCode = 1;
                    response.ErrMsg = "成功获取信息";
                    //然后调用中心取险种推荐的逻辑
                    response.SaveQuote = new SaveQuoteViewModel();
                    bx_car_renewal car_Renewal = _carRenewalRepository.FindByLicenseno(response.CarInfo.license_no);
                    if (car_Renewal != null)
                    {
                        response.SaveQuote = car_Renewal.ConvetToViewModel();
                    }
                    else
                    {
                        request.MoldName = response.CarInfo.mold_name;
                        request.RegisterDate = response.CarInfo.register_date.HasValue ? response.CarInfo.register_date.Value.ToString("yyyy-MM-dd") : string.Empty;
                        response.IsIntelligent = 1;
                        //获取推荐信息失败，也返回1
                        var tuple = await _getIntelligentInsurance.GetCenterInsurance(request);
                        response.SaveQuote = tuple.Item1;
                        if (!tuple.Item2) {
                            response.ErrCode = 2;
                            response.ErrMsg = "成功获取车辆信息，未获取到险种信息";
                        }
                    }
                }
                else
                {
                    response.ErrCode = 0;
                    response.ErrMsg = "未获取到车辆信息";
                }
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                Task.Factory.StartNew(() =>
                {
                    GetReInfoRequest requestNew = new GetReInfoRequest()
                    {
                        LicenseNo = request.LicenseNo,
                        Agent = request.Agent
                    };
                    int datastatus = int.Parse(response.ErrCode.ToString() + response.IsIntelligent.ToString());
                    _renewalStatusService.AddRenewalStatus(response.ErrCode, requestNew);
                });
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                return response;
            }
            catch (Exception ex)
            {
                response = new GetIntelligentReInfoResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("获取简易续保信息发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }
    }
}
