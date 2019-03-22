using BiHuManBu.ExternalInterfaces.Infrastructure;
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
using BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetPrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetSubmitInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using BiHuManBu.Redis;
using log4net;
using MetricsLibrary;
using ServiceStack.Common;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class CarInsuranceService : CommonBehaviorService, ICarInsuranceService
    {
        #region 初始化调用方法
        private static readonly string _url = ConfigurationManager.AppSettings["BaoxianCenter"];
        private static readonly string _apiurl = ConfigurationManager.AppSettings["RateCentertest"];
        private static readonly string _isAddIdCardBack6 = ConfigurationManager.AppSettings["IsAddIdCardBack6"];
        private static readonly string _host = ConfigurationManager.AppSettings["SystemCrmUrl"];
        private const string PreXubaoCacheKey = "xubao_cach_key_";
        private ISaveQuoteRepository _saveQuoteRepository;
        private IUserInfoRepository _userInfoRepository;
        private ILoginService _loginService;
        private ILastInfoRepository _infoRepository;
        private ISubmitInfoRepository _submitInfoRepository;
        private IQuoteResultRepository _quoteResultRepository;
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        private IAgentRepository _agentRepository;
        private IMessageCenter _messageCenter;
        private ICarInfoRepository _carInfoRepository;
        private ICacheHelper _cacheHelper;
        private ICarInsuranceCache _carInsuranceCache;
        private ICarRenewalRepository _carRenewalRepository;
        private IDeviceDetailRepository _detailRepository;
        private IQuoteReqCarinfoRepository _quoteReqCarinfoRepository;
        private IQuoteResultCarinfoRepository _quoteResultCarinfoRepository;
        private IAgentConfigRepository _agentConfig;
        private IConfigRepository _configRepository;
        private INoticexbService _noticexbService;
        private ICarModelRepository _carModelRepository;
        private readonly IMultiChannelsService _multiChannelsService;
        private readonly ICheckRequestGetPrecisePrice _checkRequestGetPrecisePrice;
        private readonly ICheckRequestGetSubmitInfo _checkRequestGetSubmitInfo;
        private readonly IGetAgentInfoService _getAgentInfoService;
        private readonly ISpecialOptionService _specialOptionService;
        private readonly IGetMoldNameFromCenter _getMoldNameFromCenter;
        private readonly IBatchRenewalRepository _batchRenewalRepository;
        private IFiterAndRepeatDataService _fiterAndRepeatDataService;
        private IFilterMoldNameService _filterMoldNameService;
        private IManagerRoleRepository _managerRoleRepository;
        private IAddJiaYiService _addJiaYiService;
        private IYwxdetailRepository _ywxdetailRepository;
        private IGetReInfoMainService _getReInfoMainService;

        public CarInsuranceService(ISaveQuoteRepository saveQuoteRepository, IUserInfoRepository userInfoRepository, ILoginService loginService, ISubmitInfoRepository submitInfoRepository,
            IQuoteResultRepository quoteResultRepository, ILastInfoRepository lastInfoRepository, IAgentRepository agentRepository, IMessageCenter messageCenter,
            ICarInfoRepository carInfoRepository, IRenewalQuoteRepository renewalQuoteRepository, IQuoteReqCarinfoRepository quoteReqCarinfoRepository, IQuoteResultCarinfoRepository quoteResultCarinfoRepository,
            ICacheHelper cacheHelper, ICarInsuranceCache carInsuranceCache, ICarRenewalRepository carRenewalRepository, IDeviceDetailRepository detailRepository, IAgentConfigRepository agentConfig, INoticexbService noticexbService, IConfigRepository configRepository,
            ICarModelRepository carModelRepository, IMultiChannelsService multiChannelsService, ICheckRequestGetPrecisePrice checkRequestGetPrecisePrice, ICheckRequestGetSubmitInfo checkRequestGetSubmitInfo, IGetAgentInfoService getAgentInfoService, ISpecialOptionService specialOptionService,
            IGetMoldNameFromCenter getMoldNameFromCenter, IBatchRenewalRepository batchRenewalRepository, IFiterAndRepeatDataService fiterAndRepeatDataService, IFilterMoldNameService filterMoldNameService,
            IManagerRoleRepository managerRoleRepository, IAddJiaYiService addJiaYiService, IYwxdetailRepository ywxdetailRepository,
            IGetReInfoMainService getReInfoMainService)
            : base(agentRepository, cacheHelper)
        {
            _saveQuoteRepository = saveQuoteRepository;
            _userInfoRepository = userInfoRepository;
            _loginService = loginService;
            _infoRepository = lastInfoRepository;
            _submitInfoRepository = submitInfoRepository;
            _quoteResultRepository = quoteResultRepository;
            _agentRepository = agentRepository;
            _messageCenter = messageCenter;
            _carInfoRepository = carInfoRepository;
            _carInsuranceCache = carInsuranceCache;
            _carRenewalRepository = carRenewalRepository;
            _detailRepository = detailRepository;
            _quoteReqCarinfoRepository = quoteReqCarinfoRepository;
            _quoteResultCarinfoRepository = quoteResultCarinfoRepository;
            _agentConfig = agentConfig;
            _noticexbService = noticexbService;
            _configRepository = configRepository;
            _carModelRepository = carModelRepository;
            _multiChannelsService = multiChannelsService;
            _checkRequestGetPrecisePrice = checkRequestGetPrecisePrice;
            _checkRequestGetSubmitInfo = checkRequestGetSubmitInfo;
            _getAgentInfoService = getAgentInfoService;
            _specialOptionService = specialOptionService;
            _getMoldNameFromCenter = getMoldNameFromCenter;
            _batchRenewalRepository = batchRenewalRepository;
            _fiterAndRepeatDataService = fiterAndRepeatDataService;
            _filterMoldNameService = filterMoldNameService;
            _managerRoleRepository = managerRoleRepository;
            _addJiaYiService = addJiaYiService;
            _ywxdetailRepository = ywxdetailRepository;
            _getReInfoMainService = getReInfoMainService;
        }
        #endregion
        /// <summary>
        /// 续保信息服务层
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<GetReInfoResponse> GetReInfo(GetReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            return await _getReInfoMainService.GetReInfo(request);
        }

        /// <summary>
        /// 获取核保信息服务层
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetSubmitInfoResponse> GetSubmitInfo(GetSubmitInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetSubmitInfoResponse();
            //校验
            BaseResponse baseResponse = _checkRequestGetSubmitInfo.CheckRequest(request, pairs);
            if (baseResponse.Status == HttpStatusCode.Forbidden)
            {
                response.Status = HttpStatusCode.Forbidden;
                response.ErrMsg = baseResponse.ErrMsg;
                return response;
            }
            //微信端逻辑 次级代理
            if (request.ChildAgent > 0)
            {
                request.Agent = request.ChildAgent;
            }
            //报价组合
            request = GetSubmitGroup(request);
            #region 读取数据
            ExecutionContext.SuppressFlow();
            response = await _carInsuranceCache.GetSubmitInfo(request);
            if (response.BusinessStatus != 1 && response.BusinessStatus != 3)
            {
                try
                {
                    bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.Agent.ToString(), request.RenewalCarType);
                    if (userinfo == null)
                    {
                    }
                    else
                    {
                        response.SubmitInfo = _submitInfoRepository.GetSubmitInfo(userinfo.Id, request.IntentionCompany);
                        response.Status = HttpStatusCode.OK;
                        response.CustKey = request.CustKey;
                        string baojiaCacheKey = CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);
                        string baojiaCacheOrderIdKey = string.Format("{0}OrderIdKey", baojiaCacheKey);
                        var ck = CacheProvider.Get<string>(baojiaCacheOrderIdKey);
                        string baojiaCacheCheckCodeIdKey = string.Format("{0}CheckCodeKey", baojiaCacheKey);
                        var bk = CacheProvider.Get<string>(baojiaCacheCheckCodeIdKey);
                        if (!string.IsNullOrWhiteSpace(ck))
                        {
                            response.OrderId = ck;
                        }
                        if (!string.IsNullOrWhiteSpace(bk))
                        {
                            response.CheckCode = bk;
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Status = HttpStatusCode.ExpectationFailed;
                    logError.Info("获取核保信息发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                }
            }
            #endregion
            return response;
        }
        /// <summary>
        /// 插入险种信息 并发送报价消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PostPrecisePriceResponse> InsertUserInfo(PostPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            int MsgTopAgent = request.Agent;
            StringBuilder sb = new StringBuilder();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            sb.Append("请求参数：" + request.ToJson());
            var response = new PostPrecisePriceResponse();
            //代理人校验
            IBxAgent agentModel = _getAgentInfoService.GetAgentModelFactory(request.Agent);
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                if (agentModel.endDate.HasValue && agentModel.endDate.Value < DateTime.Now)
                {
                    response.StatusMessage = string.Format("参数校验错误，账号已过期。过期时间为：{0}", agentModel.endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    return response;
                }
                response.StatusMessage = "参数校验错误，账号已禁用。";
                return response;
            }
            if (!agentModel.AgentCanQuote())
            {
                response.Status = HttpStatusCode.Forbidden;
                response.StatusMessage = "参数校验错误，账号不允许报价。";
                return response;
            }
            //if (!agentModel.AgentCanSubmit())//不再置零
            //{
            //    request.SubmitGroup = 0;
            //}
            try
            {
                //报价转换 
                request = GetQuoteAndSubmitGroup(request);
                var ret = CheckRequestGroup(request);
                if (ret == HttpStatusCode.NotAcceptable || ret == HttpStatusCode.NoContent)
                {
                    return new PostPrecisePriceResponse
                    {
                        Status = ret
                    };
                }
                //微信端逻辑 次级代理
                if (request.ChildAgent > 0)
                {
                    var item = GetAgentModelFactory(request.ChildAgent);
                    if (item.AgentCanUse())
                    {
                        request.Agent = request.ChildAgent;
                    }
                    else
                    {
                        return new PostPrecisePriceResponse
                        {
                            StatusMessage = "您的账号已被禁用，如有疑问请联系管理员。",
                            Status = HttpStatusCode.Forbidden
                        };
                    }
                    if (!item.AgentCanQuote())
                    {
                        response.StatusMessage = "参数校验错误，账号不允许报价。";
                        response.Status = HttpStatusCode.Forbidden;
                        return response;
                    }
                    //if (!item.AgentCanSubmit())
                    //{
                    //    request.SubmitGroup = 0;
                    //    request.IntentionCompany = 0;
                    //}
                }
                sw.Stop();
                sb.AppendLine("报价校验阶段耗时：" + sw.ElapsedMilliseconds);
                sw.Restart();
                #region 修改userinfo记录
                bx_userinfo userinfo = UserinfoSearchFactory.FindByQuoteRequest(request, _userInfoRepository);
                if (userinfo == null)
                {
                    logError.Info("根据险种获取报价失败，没有uerinfo信息");
                    response.Status = HttpStatusCode.NotFound;
                    return response;
                }
                if (userinfo.NeedEngineNo == 1)
                {
                    response.Status = HttpStatusCode.NonAuthoritativeInformation;
                    return response;
                }
                if (request.QuoteParalelConflict == 1)
                {
                    //报价频繁逻辑处理
                    if (userinfo.IsSingleSubmit > 0 && userinfo.QuoteStatus == -1 && userinfo.LatestQuoteTime.HasValue && userinfo.LatestQuoteTime.GetValueOrDefault().AddMinutes(5) > DateTime.Now)
                    {
                        response.Status = HttpStatusCode.Conflict;
                        return response;
                    }
                }
                //当续保成功，将座位数改为发生变化
                if (userinfo.RenewalStatus == 1)
                {
                    request.SeatUpdated = 1;
                }

                #region 赋值车架号、品牌型号
                if (!string.IsNullOrWhiteSpace(request.CarVin))
                {
                    userinfo.CarVIN = request.CarVin.ToUpper();
                }
                if (!string.IsNullOrWhiteSpace(request.MoldName))
                {
                    userinfo.MoldName = request.MoldName;
                }
                //获取品牌型号
                var moldNameViewModle = await _getMoldNameFromCenter.GetMoldNameService(userinfo.CarVIN, userinfo.MoldName, MsgTopAgent, int.Parse(userinfo.CityCode));
                if (moldNameViewModle != null && !string.IsNullOrEmpty(moldNameViewModle.MoldName))
                    userinfo.MoldName = moldNameViewModle.MoldName;
                #endregion

                UserinfoMakeFactory.QuoteUpdate(request, userinfo, _url, _userInfoRepository, agentModel.TopAgentId);
                #endregion
                sw.Stop();
                sb.Append("insert userinf阶段耗时：" + sw.ElapsedMilliseconds);
                sw.Restart();
                #region 公私车判断
                var quotereq = _quoteReqCarinfoRepository.Find(userinfo.Id);
                //公私车类型
                int publictype = 0;
                if (request.CityCode == 11 || request.CityCode == 14 || request.CityCode == 17 || request.CityCode == 20)
                {//深圳 广州 东莞 佛山
                    publictype = CheckIsPublicFieldNew(request, quotereq != null ? (quotereq.is_public ?? 0) : 0);
                }
                else
                {
                    publictype = CheckIsPublicField(request, quotereq != null ? (quotereq.is_public ?? 0) : 0);
                }
                if (request.CarUsedType == 6 || request.CarUsedType == 7 || request.CarUsedType == 20)
                {
                }
                else
                {
                    int intcarusedtype = 0;
                    //当续保成功以后，取续保返回的车辆使用性质//对外接口不做此拦截
                    if (request.RenewalType != 2 && userinfo != null && userinfo.RenewalStatus == 1)
                    {
                        //获取续保信息的车辆使用性质
                        intcarusedtype = _carRenewalRepository.GetCarUsedType(userinfo.Id);
                        if (intcarusedtype != 0)
                        {//当拿到值的时候，给请求的CarUsedType赋值
                            request.CarUsedType = intcarusedtype;
                        }
                    }
                    if (intcarusedtype < 1)
                    {//如果续保拿到值，就不执行校验了
                        CheckCarUsedType(request, publictype);
                        var checkvalue = CheckInput(request, publictype, request.CarUsedType);
                        if (checkvalue == 1)
                        {
                            response.Status = HttpStatusCode.Forbidden;
                            response.StatusMessage = "该车是非营业企业时,车主/被保险人/投保人必须至少有一个信息完整（姓名、证件类型、证件号码）且为组织机构代码或者营业执照";
                            return response;
                        }
                        else if (checkvalue == 2)
                        {
                            response.Status = HttpStatusCode.Forbidden;
                            response.StatusMessage = "该车是家庭自用车,车主/被保险人/投保人不能都为公户";
                            return response;
                        }
                        if (request.SanZhe <= 0 || request.CarUsedType != 1)
                        {
                            if (request.SanZheJieJiaRi > 0)
                            {
                                response.Status = HttpStatusCode.Forbidden;
                                response.StatusMessage = "输入参数错误，只有家庭自用车选择了第三者责任险，才可以选择三责险附加法定节假日限额翻倍险";
                                return response;
                            }
                        }
                    }
                }
                #endregion
                #region bx_quotereq_carinfo 逻辑
                if (quotereq == null)
                {
                    QuoteReqCarInfoMakeFactory.QuoteAdd(request, userinfo.Id, publictype, _quoteReqCarinfoRepository);
                }
                else
                {
                    QuoteReqCarInfoMakeFactory.QuoteUpdate(quotereq, request, publictype, _quoteReqCarinfoRepository);
                }
                sw.Stop();
                sb.AppendLine("req阶段耗时:" + sw.ElapsedMilliseconds);
                #endregion
                #region //---新增SaveQuote记录 begin---//
                bool isSuccess = false;
                var savequote = _saveQuoteRepository.GetSavequoteByBuid(userinfo.Id);
                long qid = 0;
                #region 添加设备详情
                List<bx_devicedetail> devicedetails = new List<bx_devicedetail>();
                if (request.SheBeiSunshi == 1)
                {
                    _detailRepository.Delete(userinfo.Id);
                    if (!string.IsNullOrWhiteSpace(request.DN1))
                    {
                        var d1 = new bx_devicedetail
                        {
                            b_uid = userinfo.Id,
                            device_name = request.DN1,
                            device_amount = request.DA1,
                            device_quantity = request.DQ1,
                            device_depreciationamount = request.DD1,
                            device_type = request.DT1,
                            //purchase_date = DateTime.Parse(request.PD1)
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD1))
                        {
                            d1.purchase_date = DateTime.Parse(request.PD1);
                        }
                        else
                        {
                            d1.purchase_date = null;
                        }
                        devicedetails.Add(d1);
                        _detailRepository.Add(d1);
                    }
                    if (!string.IsNullOrWhiteSpace(request.DN2))
                    {
                        var d2 = new bx_devicedetail
                        {
                            b_uid = userinfo.Id,
                            device_name = request.DN2,
                            device_amount = request.DA2,
                            device_quantity = request.DQ2,
                            //purchase_date = DateTime.Parse(request.PD2),
                            device_depreciationamount = request.DD2,
                            device_type = request.DT2,
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD2))
                        {
                            d2.purchase_date = DateTime.Parse(request.PD2);
                        }
                        else
                        {
                            d2.purchase_date = null;
                        }
                        devicedetails.Add(d2);
                        _detailRepository.Add(d2);
                    }
                    if (!string.IsNullOrWhiteSpace(request.DN3))
                    {
                        var d3 = new bx_devicedetail
                        {
                            b_uid = userinfo.Id,
                            device_name = request.DN3,
                            device_amount = request.DA3,
                            device_quantity = request.DQ3,
                            //purchase_date = DateTime.Parse(request.PD3),
                            device_depreciationamount = request.DD3,
                            device_type = request.DT3,
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD3))
                        {
                            d3.purchase_date = DateTime.Parse(request.PD3);
                        }
                        else
                        {
                            d3.purchase_date = null;
                        }
                        devicedetails.Add(d3);
                        _detailRepository.Add(d3);
                    }
                    if (!string.IsNullOrWhiteSpace(request.DN4))
                    {
                        var d4 = new bx_devicedetail
                        {
                            b_uid = userinfo.Id,
                            device_name = request.DN4,
                            device_amount = request.DA4,
                            device_quantity = request.DQ4,
                            //purchase_date = DateTime.Parse(request.PD4),
                            device_depreciationamount = request.DD4,
                            device_type = request.DT4,
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD4))
                        {
                            d4.purchase_date = DateTime.Parse(request.PD4);
                        }
                        else
                        {
                            d4.purchase_date = null;
                        }
                        devicedetails.Add(d4);
                        _detailRepository.Add(d4);
                    }
                    if (!string.IsNullOrWhiteSpace(request.DN5))
                    {
                        var d5 = new bx_devicedetail
                        {
                            b_uid = userinfo.Id,
                            device_name = request.DN5,
                            device_amount = request.DA5,
                            device_quantity = request.DQ5,
                            //purchase_date = DateTime.Parse(request.PD5),
                            device_depreciationamount = request.DD5,
                            device_type = request.DT5,
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD5))
                        {
                            d5.purchase_date = DateTime.Parse(request.PD5);
                        }
                        else
                        {
                            d5.purchase_date = null;
                        }
                        devicedetails.Add(d5);
                        _detailRepository.Add(d5);
                    }
                    if (!string.IsNullOrWhiteSpace(request.DN6))
                    {
                        var d6 = new bx_devicedetail
                        {
                            b_uid = userinfo.Id,
                            device_name = request.DN6,
                            device_amount = request.DA6,
                            device_quantity = request.DQ6,
                            //purchase_date = DateTime.Parse(request.PD6),
                            device_depreciationamount = request.DD6,
                            device_type = request.DT6,
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD6))
                        {
                            d6.purchase_date = DateTime.Parse(request.PD6);
                        }
                        else
                        {
                            d6.purchase_date = null;
                        }
                        devicedetails.Add(d6);
                        _detailRepository.Add(d6);
                    }
                }
                else
                {
                    _detailRepository.Delete(userinfo.Id);
                }
                #endregion 添加设备详情
                #region 添加多渠道 add.gpj/20171213
                _multiChannelsService.MultiChannels(request.MultiChannels, request.ChildAgent, MsgTopAgent, userinfo.Id, request.QuoteGroup, request.CityCode);
                #endregion
                #region 添加特约
                _specialOptionService.AddSpecialOptionList(userinfo.Id, request.SpecialOption);
                #endregion
                #region 添加驾意
                _addJiaYiService.AddJiaYi(userinfo.Id, request.JiaYi);
                #endregion
                sw.Restart();
                if (savequote == null)
                {
                    bx_savequote insertinfo = new bx_savequote
                    {
                        B_Uid = userinfo.Id,
                        SanZheJieJiaRi = request.SanZheJieJiaRi,
                        CheSun = request.CheSun,
                        SanZhe = request.SanZhe,
                        DaoQiang = request.DaoQiang,
                        SiJi = request.SiJi,
                        ChengKe = request.ChengKe,
                        BoLi = request.BoLi,
                        HuaHen = request.HuaHen,
                        BuJiMianCheSun = request.BuJiMianCheSun,
                        BuJiMianSanZhe = request.BuJiMianSanZhe,
                        BuJiMianDaoQiang = request.BuJiMianDaoQiang,
                        // BuJiMianRenYuan = request.BuJiMianRenYuan,
                        // BuJiMianFuJian = request.BuJiMianFuJia,
                        //2.1.5版本修改，增加6个字段
                        BuJiMianChengKe = request.BuJiMianChengKe,
                        BuJiMianSiJi = request.BuJiMianSiJi,
                        BuJiMianHuaHen = request.BuJiMianHuaHen,
                        BuJiMianSheShui = request.BuJiMianSheShui,
                        BuJiMianZiRan = request.BuJiMianZiRan,
                        BuJiMianJingShenSunShi = request.BuJiMianJingShenSunShi,
                        SheShui = request.SheShui,
                        // CheDeng = request.CheDeng,
                        ZiRan = request.ZiRan,
                        JiaoQiang = request.ForceTax, //默认值1，1=报价交强车船，0=不报交强车船,
                        //HcFeiYongBuChang = request.HcFeiYongBuChang,
                        HcHuoWuZeRen = request.HcHuoWuZeRen,
                        HcJingShenSunShi = request.HcJingShenSunShi,
                        HcSanFangTeYue = request.HcSanFangTeYue,
                        HcSheBeiSunshi = request.SheBeiSunshi,
                        BuJiMianSheBeiSunshi = request.BjmSheBeiSunshi,
                        HcXiuLiChang = request.HcXiuLiChang,
                        HcXiuLiChangType = request.HcXiuLiChangType,
                        HcFeiYongBuChang = request.Fybc,
                        FeiYongBuChangDays = request.FybcDays,
                        SheBeiSunShiConfig = devicedetails.ToJson()
                    };
                    if (!string.IsNullOrWhiteSpace(request.BizStartDate))
                    {
                        insertinfo.BizStartDate = DateTime.Parse(request.BizStartDate);
                    }
                    _saveQuoteRepository.Add(insertinfo);
                }
                else
                {
                    // savequote.B_Uid = userinfo.Id;
                    savequote.SanZheJieJiaRi = request.SanZheJieJiaRi;
                    savequote.CheSun = request.CheSun;
                    savequote.SanZhe = request.SanZhe;
                    savequote.DaoQiang = request.DaoQiang;
                    savequote.SiJi = request.SiJi;
                    savequote.ChengKe = request.ChengKe;
                    savequote.BoLi = request.BoLi;
                    savequote.HuaHen = request.HuaHen;
                    savequote.BuJiMianCheSun = request.BuJiMianCheSun;
                    savequote.BuJiMianSanZhe = request.BuJiMianSanZhe;
                    savequote.BuJiMianDaoQiang = request.BuJiMianDaoQiang;
                    //savequote.BuJiMianRenYuan = request.BuJiMianRenYuan;
                    //savequote.BuJiMianFuJian = request.BuJiMianFuJia;
                    //2.1.5版本修改，增加6个字段
                    savequote.BuJiMianChengKe = request.BuJiMianChengKe;
                    savequote.BuJiMianSiJi = request.BuJiMianSiJi;
                    savequote.BuJiMianHuaHen = request.BuJiMianHuaHen;
                    savequote.BuJiMianSheShui = request.BuJiMianSheShui;
                    savequote.BuJiMianZiRan = request.BuJiMianZiRan;
                    savequote.BuJiMianJingShenSunShi = request.BuJiMianJingShenSunShi;
                    savequote.SheShui = request.SheShui;
                    //savequote.CheDeng = request.CheDeng;
                    savequote.ZiRan = request.ZiRan;
                    savequote.JiaoQiang = request.ForceTax; //默认值1，1=报价交强车船，0=不报交强车船
                    if (!string.IsNullOrWhiteSpace(request.BizStartDate))
                    {
                        savequote.BizStartDate = DateTime.Parse(request.BizStartDate);
                    }
                    //savequote.HcFeiYongBuChang = request.HcFeiYongBuChang;
                    savequote.HcHuoWuZeRen = request.HcHuoWuZeRen;
                    savequote.HcJingShenSunShi = request.HcJingShenSunShi;
                    savequote.HcSanFangTeYue = request.HcSanFangTeYue;
                    savequote.HcSheBeiSunshi = request.SheBeiSunshi;
                    savequote.BuJiMianSheBeiSunshi = request.BjmSheBeiSunshi;
                    savequote.HcXiuLiChang = request.HcXiuLiChang;
                    savequote.HcXiuLiChangType = request.HcXiuLiChangType;
                    savequote.HcFeiYongBuChang = request.Fybc;
                    savequote.FeiYongBuChangDays = request.FybcDays;
                    savequote.SheBeiSunShiConfig = devicedetails.ToJson();
                    var count = _saveQuoteRepository.Update(savequote);
                }
                sw.Stop();
                sb.AppendLine("savequote阶段耗时：" + sw.ElapsedMilliseconds);
                #endregion//---新增SaveQuote记录 end---//
                //次新车报价，按照更改后的车牌号报价
                if (!string.IsNullOrWhiteSpace(request.UpdateLicenseNo))
                {
                    request.LicenseNo = request.UpdateLicenseNo;
                }
                string baojiaCacheKey = CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);
                //using (var client = RedisManager.GetClient())
                //{
                //    using (var tran = client.CreatePipeline())
                //    {
                try
                {
                    for (var i = 0; i < 13; i++)
                    {
                        //tran.QueueCommand(p =>
                        //{
                        RedisManager.Remove(string.Format("{0}-{1}-bj-{2}", baojiaCacheKey, i, "key"));
                        //});
                        //tran.QueueCommand(p =>
                        //{
                        RedisManager.Remove(string.Format("{0}-{1}-hb-{2}", baojiaCacheKey, i, "key"));
                        //});
                    }
                    //清空出险记录
                    //tran.QueueCommand(p =>
                    //{
                    RedisManager.Remove(string.Format("{0}-claimdetai-key", baojiaCacheKey));
                    //});
                    //tran.QueueCommand(p =>
                    //{
                    RedisManager.Remove(string.Format("{0}-lastinfo-repeat-key", baojiaCacheKey));
                    //});

                    //tran.Flush();
                }
                catch (Exception ex)
                {
                    logError.Info("InsertUserInfo清空缓存失败:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                }
                //}
                //}
                sw.Restart();
                var msgBody = new
                {
                    B_Uid = userinfo.Id,
                    IsCloseSms = 0,
                    NotifyCacheKey = baojiaCacheKey,
                    RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    AgentId = MsgTopAgent,
                    CityId = request.CityCode,
                    IsOrderChangeRelation = request.IsOrderChangeRelation,//是否要出单变更保司系统关系人信息1是0否
                    IsPaBatchQuote = request.IsPaBatchQuote
                };
                //发送续保信息
                var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                    ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["BxBaoJiaOrHeBaoName"]);
                response.Status = HttpStatusCode.OK;
                logInfo.Info("根据行驶证请求报价信息成功:" + request.ToJson() + "\n\n 消息体：" + msgbody.ToJson() + ",报价键值：" + baojiaCacheKey);
                sw.Stop();
                sb.AppendLine("发送消息阶段耗时：" + sw.ElapsedMilliseconds);
                //报价调用其他接口
                PreciseRequestThirdPart(userinfo.Id, request.ChildAgent);
            }
            catch (Exception ex)
            {
                MetricUtil.UnitReports("quote_insert_service");
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("根据险种报价发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            logError.Info("insert 方法耗时：" + sb.ToString());
            return response;
        }
        /// <summary>
        /// 更新行驶证信息并发送报价消息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<PostPrecisePriceResponse> UpdateDrivingLicense(PostPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            int MsgTopAgent = request.Agent;
            StringBuilder sb = new StringBuilder();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            sb.Append("请求参数：" + request.ToJson());
            PostPrecisePriceResponse response = new PostPrecisePriceResponse();
            //代理人校验
            IBxAgent agentModel = _getAgentInfoService.GetAgentModelFactory(request.Agent);
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                if (agentModel.endDate.HasValue && agentModel.endDate.Value < DateTime.Now)
                {
                    response.StatusMessage = string.Format("参数校验错误，账号已过期。过期时间为：{0}", agentModel.endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    return response;
                }
                response.StatusMessage = "参数校验错误，账号已禁用。";
                return response;
            }
            //是否可以报价
            if (!agentModel.AgentCanQuote())
            {
                response.Status = HttpStatusCode.Forbidden;
                response.StatusMessage = "参数校验错误，账号不允许报价。";
                return response;
            }
            ////是否可以核保//不再置零
            //if (!agentModel.AgentCanSubmit())
            //{
            //    request.SubmitGroup = 0;
            //}
            var topAgent = request.Agent;
            try
            {
                //报价转换 
                request = GetQuoteAndSubmitGroup(request);
                var ret = CheckRequestGroup(request);
                if (ret == HttpStatusCode.NotAcceptable || ret == HttpStatusCode.NoContent)
                {
                    //logInfo.Info("报价渠道不对" + request.ToJson());
                    return new PostPrecisePriceResponse
                    {
                        Status = ret
                    };
                }
                //微信端逻辑 次级代理
                if (request.ChildAgent > 0)
                {
                    var item = GetAgentModelFactory(request.ChildAgent);
                    if (item.AgentCanUse())
                    {
                        request.Agent = request.ChildAgent;
                    }
                    else
                    {
                        //logInfo.Info("次级代理不可用" + request.ToJson());
                        return new PostPrecisePriceResponse
                        {
                            StatusMessage = "您的账号已被禁用，如有疑问请联系管理员。",
                            Status = HttpStatusCode.Forbidden
                        };
                    }
                    if (!item.AgentCanQuote())
                    {
                        //logInfo.Info("次级代理不可以报价" + request.ToJson());
                        response.StatusMessage = "参数校验错误，账号不允许报价。";
                        response.Status = HttpStatusCode.Forbidden;
                        return response;
                    }
                    //if (!item.AgentCanSubmit())//不再置零
                    //{
                    //    request.SubmitGroup = 0;
                    //    request.IntentionCompany = 0;
                    //}
                }
                sw.Stop();
                sb.Append("校验阶段耗时：" + sw.ElapsedMilliseconds);
                if (!string.IsNullOrWhiteSpace(request.MoldNameUrlEncode))
                {
                    request.MoldName = System.Web.HttpUtility.UrlDecode(request.MoldNameUrlEncode);
                }
                sw.Restart();
                bx_userinfo userinfo = UserinfoSearchFactory.FindByQuoteRequest(request, _userInfoRepository);
                long buid = 0;
                if (userinfo == null)
                {
                    //获取品牌型号
                    var moldNameViewModle = await _getMoldNameFromCenter.GetMoldNameService(request.CarVin, request.MoldName, agentModel.TopAgentId, request.CityCode);
                    if (moldNameViewModle != null && !string.IsNullOrEmpty(moldNameViewModle.MoldName))
                        request.MoldName = moldNameViewModle.MoldName;

                    buid = UserinfoMakeFactory.QuoteAdd(request, _url, _userInfoRepository, agentModel.TopAgentId);
                }
                else
                {
                    //当续保成功，将座位数改为发生变化
                    if (userinfo.RenewalStatus == 1)
                    {
                        request.SeatUpdated = 1;
                    }
                    if (request.QuoteParalelConflict == 1)
                    {
                        //报价频繁逻辑处理
                        if (userinfo.IsSingleSubmit > 0 && userinfo.QuoteStatus == -1 && userinfo.LatestQuoteTime.HasValue && userinfo.LatestQuoteTime.GetValueOrDefault().AddMinutes(5) > DateTime.Now)
                        {
                            response.Status = HttpStatusCode.Conflict;
                            return response;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(request.CarVin))
                    {
                        userinfo.CarVIN = request.CarVin.ToUpper();
                    }
                    if (!string.IsNullOrWhiteSpace(request.MoldName))
                    {
                        userinfo.MoldName = request.MoldName;
                    }
                    //获取品牌型号
                    var moldNameViewModle = await _getMoldNameFromCenter.GetMoldNameService(userinfo.CarVIN, userinfo.MoldName, MsgTopAgent, int.Parse(userinfo.CityCode));
                    if (moldNameViewModle != null && !string.IsNullOrEmpty(moldNameViewModle.MoldName))
                        userinfo.MoldName = moldNameViewModle.MoldName;

                    UserinfoMakeFactory.QuoteUpdate(request, userinfo, _url, _userInfoRepository, agentModel.TopAgentId);
                    buid = userinfo.Id;
                }
                sw.Stop();
                sb.AppendLine("userinfo阶段耗时：" + sw.ElapsedMilliseconds);
                #region 公私车判断
                sw.Restart();
                var quotereq = _quoteReqCarinfoRepository.Find(buid);
                int publictype = 0;
                if (request.CityCode == 11 || request.CityCode == 14 || request.CityCode == 17 || request.CityCode == 20)
                {//深圳 广州 东莞 佛山
                    publictype = CheckIsPublicFieldNew(request, quotereq != null ? (quotereq.is_public ?? 0) : 0);
                }
                else
                {
                    publictype = CheckIsPublicField(request, quotereq != null ? (quotereq.is_public ?? 0) : 0);
                }
                if (request.CarUsedType == 6 || request.CarUsedType == 7 || request.CarUsedType == 20)
                {
                }
                else
                {
                    int intcarusedtype = 0;
                    //当续保成功以后，取续保返回的车辆使用性质//对外接口不做此拦截
                    if (request.RenewalType != 2 && userinfo != null && userinfo.RenewalStatus == 1)
                    {
                        //获取续保信息的车辆使用性质
                        intcarusedtype = _carRenewalRepository.GetCarUsedType(userinfo.Id);
                        if (intcarusedtype != 0)
                        {//当拿到值的时候，给请求的CarUsedType赋值
                            request.CarUsedType = intcarusedtype;
                        }
                    }
                    if (intcarusedtype < 1)
                    {//如果续保拿到值，就不执行校验了
                        CheckCarUsedType(request, publictype);
                        var checkvalue = CheckInput(request, publictype, request.CarUsedType);
                        if (checkvalue == 1)
                        {
                            response.Status = HttpStatusCode.Forbidden;
                            response.StatusMessage = "该车是非营业企业时,车主/被保险人/投保人必须至少有一个信息完整（姓名、证件类型、证件号码）且为组织机构代码或者营业执照";
                            return response;
                        }
                        else if (checkvalue == 2)
                        {
                            response.Status = HttpStatusCode.Forbidden;
                            response.StatusMessage = "该车是家庭自用车,车主/被保险人/投保人不能都为公户";
                            return response;
                        }
                        if (request.SanZhe <= 0 || request.CarUsedType != 1)
                        {
                            if (request.SanZheJieJiaRi > 0)
                            {
                                response.Status = HttpStatusCode.Forbidden;
                                response.StatusMessage = "输入参数错误，只有家庭自用车选择了第三者责任险，才可以选择三责险附加法定节假日限额翻倍险";
                                return response;
                            }
                        }
                    }
                }
                #endregion
                #region bx_quotereq_carinfo 逻辑
                if (quotereq == null)
                {
                    QuoteReqCarInfoMakeFactory.QuoteAdd(request, buid, publictype, _quoteReqCarinfoRepository);
                }
                else
                {
                    QuoteReqCarInfoMakeFactory.QuoteUpdate(quotereq, request, publictype, _quoteReqCarinfoRepository);
                }
                #endregion
                sw.Stop();
                sb.AppendLine("req阶段耗时：" + sw.ElapsedMilliseconds);
                var savequote = _saveQuoteRepository.GetSavequoteByBuid(buid);
                long qid = 0;
                #region 添加设备详情
                List<bx_devicedetail> devicedetails = new List<bx_devicedetail>();
                if (request.SheBeiSunshi == 1)
                {
                    _detailRepository.Delete(buid);
                    if (!string.IsNullOrWhiteSpace(request.DN1))
                    {
                        var d1 = new bx_devicedetail
                        {
                            b_uid = buid,
                            device_name = request.DN1,
                            device_amount = request.DA1,
                            device_quantity = request.DQ1,
                            device_depreciationamount = request.DD1,
                            device_type = request.DT1,
                            //purchase_date = DateTime.Parse(request.PD1)
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD1))
                        {
                            d1.purchase_date = DateTime.Parse(request.PD1);
                        }
                        else
                        {
                            d1.purchase_date = null;
                        }
                        devicedetails.Add(d1);
                        _detailRepository.Add(d1);
                    }
                    if (!string.IsNullOrWhiteSpace(request.DN2))
                    {
                        var d2 = new bx_devicedetail
                        {
                            b_uid = buid,
                            device_name = request.DN2,
                            device_amount = request.DA2,
                            device_quantity = request.DQ2,
                            //purchase_date = DateTime.Parse(request.PD2),
                            device_depreciationamount = request.DD2,
                            device_type = request.DT2,
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD2))
                        {
                            d2.purchase_date = DateTime.Parse(request.PD2);
                        }
                        else
                        {
                            d2.purchase_date = null;
                        }
                        devicedetails.Add(d2);
                        _detailRepository.Add(d2);
                    }
                    if (!string.IsNullOrWhiteSpace(request.DN3))
                    {
                        var d3 = new bx_devicedetail
                        {
                            b_uid = buid,
                            device_name = request.DN3,
                            device_amount = request.DA3,
                            device_quantity = request.DQ3,
                            //purchase_date = DateTime.Parse(request.PD3),
                            device_depreciationamount = request.DD3,
                            device_type = request.DT3,
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD3))
                        {
                            d3.purchase_date = DateTime.Parse(request.PD3);
                        }
                        else
                        {
                            d3.purchase_date = null;
                        }
                        devicedetails.Add(d3);
                        _detailRepository.Add(d3);
                    }
                    if (!string.IsNullOrWhiteSpace(request.DN4))
                    {
                        var d4 = new bx_devicedetail
                        {
                            b_uid = buid,
                            device_name = request.DN4,
                            device_amount = request.DA4,
                            device_quantity = request.DQ4,
                            //purchase_date = DateTime.Parse(request.PD4),
                            device_depreciationamount = request.DD4,
                            device_type = request.DT4,
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD4))
                        {
                            d4.purchase_date = DateTime.Parse(request.PD4);
                        }
                        else
                        {
                            d4.purchase_date = null;
                        }
                        devicedetails.Add(d4);
                        _detailRepository.Add(d4);
                    }
                    if (!string.IsNullOrWhiteSpace(request.DN5))
                    {
                        var d5 = new bx_devicedetail
                        {
                            b_uid = buid,
                            device_name = request.DN5,
                            device_amount = request.DA5,
                            device_quantity = request.DQ5,
                            //purchase_date = DateTime.Parse(request.PD5),
                            device_depreciationamount = request.DD5,
                            device_type = request.DT5,
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD5))
                        {
                            d5.purchase_date = DateTime.Parse(request.PD5);
                        }
                        else
                        {
                            d5.purchase_date = null;
                        }
                        devicedetails.Add(d5);
                        _detailRepository.Add(d5);
                    }
                    if (!string.IsNullOrWhiteSpace(request.DN6))
                    {
                        var d6 = new bx_devicedetail
                        {
                            b_uid = buid,
                            device_name = request.DN6,
                            device_amount = request.DA6,
                            device_quantity = request.DQ6,
                            //purchase_date = DateTime.Parse(request.PD6),
                            device_depreciationamount = request.DD6,
                            device_type = request.DT6,
                        };
                        if (!string.IsNullOrWhiteSpace(request.PD6))
                        {
                            d6.purchase_date = DateTime.Parse(request.PD6);
                        }
                        else
                        {
                            d6.purchase_date = null;
                        }
                        devicedetails.Add(d6);
                        _detailRepository.Add(d6);
                    }
                }
                else
                {
                    _detailRepository.Delete(buid);
                }
                #endregion 添加设备详情
                #region 添加多渠道 add.gpj/20171213
                _multiChannelsService.MultiChannels(request.MultiChannels, request.ChildAgent, MsgTopAgent, buid, request.QuoteGroup, request.CityCode);
                #endregion
                #region 添加特约
                _specialOptionService.AddSpecialOptionList(buid, request.SpecialOption);
                #endregion
                #region 添加驾意
                _addJiaYiService.AddJiaYi(buid, request.JiaYi);
                #endregion
                sw.Restart();
                if (savequote == null)
                {
                    bx_savequote insertinfo = new bx_savequote
                    {
                        SanZheJieJiaRi = request.SanZheJieJiaRi,
                        B_Uid = buid,
                        CheSun = request.CheSun,
                        SanZhe = request.SanZhe,
                        DaoQiang = request.DaoQiang,
                        SiJi = request.SiJi,
                        ChengKe = request.ChengKe,
                        BoLi = request.BoLi,
                        HuaHen = request.HuaHen,
                        BuJiMianCheSun = request.BuJiMianCheSun,
                        BuJiMianSanZhe = request.BuJiMianSanZhe,
                        BuJiMianDaoQiang = request.BuJiMianDaoQiang,
                        //BuJiMianRenYuan = request.BuJiMianRenYuan,
                        // BuJiMianFuJian = request.BuJiMianFuJia,
                        //2.1.5版本修改，增加6个字段
                        BuJiMianChengKe = request.BuJiMianChengKe,
                        BuJiMianSiJi = request.BuJiMianSiJi,
                        BuJiMianHuaHen = request.BuJiMianHuaHen,
                        BuJiMianSheShui = request.BuJiMianSheShui,
                        BuJiMianZiRan = request.BuJiMianZiRan,
                        BuJiMianJingShenSunShi = request.BuJiMianJingShenSunShi,
                        SheShui = request.SheShui,
                        //CheDeng = request.CheDeng,
                        ZiRan = request.ZiRan,
                        JiaoQiang = request.ForceTax, //默认值1，1=报价交强车船，0=不报交强车船
                        //HcFeiYongBuChang = request.HcFeiYongBuChang,
                        HcHuoWuZeRen = request.HcHuoWuZeRen,
                        HcJingShenSunShi = request.HcJingShenSunShi,
                        HcSanFangTeYue = request.HcSanFangTeYue,
                        HcSheBeiSunshi = request.SheBeiSunshi,
                        BuJiMianSheBeiSunshi = request.BjmSheBeiSunshi,
                        HcXiuLiChang = request.HcXiuLiChang,
                        HcXiuLiChangType = request.HcXiuLiChangType,
                        HcFeiYongBuChang = request.Fybc,
                        FeiYongBuChangDays = request.FybcDays,
                        SheBeiSunShiConfig = devicedetails.ToJson()
                    };
                    if (!string.IsNullOrWhiteSpace(request.BizStartDate))
                    {
                        insertinfo.BizStartDate = DateTime.Parse(request.BizStartDate);
                    }
                    _saveQuoteRepository.Add(insertinfo);
                }
                else
                {
                    // savequote.B_Uid = userinfo.Id;
                    savequote.SanZheJieJiaRi = request.SanZheJieJiaRi;
                    savequote.CheSun = request.CheSun;
                    savequote.SanZhe = request.SanZhe;
                    savequote.DaoQiang = request.DaoQiang;
                    savequote.SiJi = request.SiJi;
                    savequote.ChengKe = request.ChengKe;
                    savequote.BoLi = request.BoLi;
                    savequote.HuaHen = request.HuaHen;
                    savequote.BuJiMianCheSun = request.BuJiMianCheSun;
                    savequote.BuJiMianSanZhe = request.BuJiMianSanZhe;
                    savequote.BuJiMianDaoQiang = request.BuJiMianDaoQiang;
                    //savequote.BuJiMianRenYuan = request.BuJiMianRenYuan;
                    //savequote.BuJiMianFuJian = request.BuJiMianFuJia;
                    //2.1.5版本修改，增加6个字段
                    savequote.BuJiMianChengKe = request.BuJiMianChengKe;
                    savequote.BuJiMianSiJi = request.BuJiMianSiJi;
                    savequote.BuJiMianHuaHen = request.BuJiMianHuaHen;
                    savequote.BuJiMianSheShui = request.BuJiMianSheShui;
                    savequote.BuJiMianZiRan = request.BuJiMianZiRan;
                    savequote.BuJiMianJingShenSunShi = request.BuJiMianJingShenSunShi;
                    savequote.SheShui = request.SheShui;
                    //savequote.CheDeng = request.CheDeng;
                    savequote.ZiRan = request.ZiRan;
                    savequote.JiaoQiang = request.ForceTax; //默认值1，1=报价交强车船，0=不报交强车船
                    if (!string.IsNullOrWhiteSpace(request.BizStartDate))
                    {
                        savequote.BizStartDate = DateTime.Parse(request.BizStartDate);
                    }
                    //savequote.HcFeiYongBuChang = request.HcFeiYongBuChang;
                    savequote.HcHuoWuZeRen = request.HcHuoWuZeRen;
                    savequote.HcJingShenSunShi = request.HcJingShenSunShi;
                    savequote.HcSanFangTeYue = request.HcSanFangTeYue;
                    savequote.HcSheBeiSunshi = request.SheBeiSunshi;
                    savequote.BuJiMianSheBeiSunshi = request.BjmSheBeiSunshi;
                    savequote.HcXiuLiChang = request.HcXiuLiChang;
                    savequote.HcXiuLiChangType = request.HcXiuLiChangType;
                    savequote.HcFeiYongBuChang = request.Fybc;
                    savequote.FeiYongBuChangDays = request.FybcDays;
                    savequote.SheBeiSunShiConfig = devicedetails.ToJson();
                    _saveQuoteRepository.Update(savequote);
                }
                sw.Stop();
                sb.AppendLine("save阶段耗时:" + sw.ElapsedMilliseconds);
                //次新车报价，按照更改后的车牌号报价
                if (!string.IsNullOrWhiteSpace(request.UpdateLicenseNo))
                {
                    request.LicenseNo = request.UpdateLicenseNo;
                }
                string baojiaCacheKey =
                   CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);
                //using (var client = RedisManager.GetClient())
                //{
                //    using (var tran = client.CreatePipeline())
                //    {
                try
                {
                    for (var i = 0; i < 13; i++)
                    {
                        //tran.QueueCommand(p =>
                        //{
                        RedisManager.Remove(string.Format("{0}-{1}-bj-{2}", baojiaCacheKey, i, "key"));
                        //});
                        //tran.QueueCommand(p =>
                        //{
                        RedisManager.Remove(string.Format("{0}-{1}-hb-{2}", baojiaCacheKey, i, "key"));
                        //});
                    }
                    //清空出险记录
                    //tran.QueueCommand(p =>
                    //{
                    RedisManager.Remove(string.Format("{0}-claimdetai-key", baojiaCacheKey));

                    //});
                    //tran.QueueCommand(p =>
                    //{
                    RedisManager.Remove(string.Format("{0}-lastinfo-repeat-key", baojiaCacheKey));

                    //});


                    //tran.Flush();
                }
                catch (Exception ex)
                {
                    logError.Info("UpdateDrivingLicense清空缓存失败:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                }
                //}
                //}
                sw.Restart();
                var msgBody = new
                {
                    B_Uid = buid,
                    IsCloseSms = 0,
                    NotifyCacheKey = baojiaCacheKey,
                    AgentId = MsgTopAgent,
                    CityId = request.CityCode,
                    IsOrderChangeRelation = request.IsOrderChangeRelation,//是否要出单变更保司系统关系人信息1是0否
                    ReQuoteAgent = request.ReQuoteAgent,
                    ReQuoteName = request.ReQuoteName,
                    IsPaBatchQuote=request.IsPaBatchQuote
                };
                //发送报价信息
                var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                     ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["BxBaoJiaOrHeBaoName"]);
                response.Status = HttpStatusCode.OK;
                logInfo.Info("根据行驶证请求报价信息成功:" + request.ToJson() + "\n\n 消息体：" + msgbody.ToJson() + ",报价键值：" + baojiaCacheKey);
                sw.Stop();
                sb.AppendLine("发送消息耗时:" + sw.ElapsedMilliseconds);
                //报价调用其他接口
                PreciseRequestThirdPart(buid, request.ChildAgent);
            }
            catch (Exception ex)
            {
                MetricUtil.UnitReports("quote_update_service");
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("根据行驶证请求报价发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            logError.Info("update 方法各阶段耗时:" + sb.ToString());
            return response;
        }
        /// <summary>
        /// 更新行驶证信息并发送报价消息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<PostPrecisePriceResponse> UpdateDrivingLicenseAgain(PostPrecisePriceRequestAgain request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            PostPrecisePriceResponse response = new PostPrecisePriceResponse();
            try
            {
                bx_userinfo userinfo = _userInfoRepository.FindByBuid(request.Buid);
                if (userinfo == null)
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                userinfo.JiSuanType = 0;
                userinfo.QuoteStatus = -1;
                userinfo.IsLastYear = 0;
                userinfo.Source = request.SubmitGroup;
                userinfo.IsSingleSubmit = request.QuoteGroup;
                userinfo.UpdateTime = DateTime.Now;
                var count = _userInfoRepository.Update(userinfo);
                string baojiaCacheKey =
                   CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(userinfo.LicenseNo, int.Parse(userinfo.Agent), userinfo.OpenId);
                //using (var client = RedisManager.GetClient())
                //{
                //    using (var tran = client.CreatePipeline())
                //    {
                try
                {
                    for (var i = 0; i < 13; i++)
                    {
                        //tran.QueueCommand(p =>
                        //{
                        RedisManager.Remove(string.Format("{0}-{1}-bj-{2}", baojiaCacheKey, i, "key"));
                        //});
                        //tran.QueueCommand(p =>
                        //{
                        RedisManager.Remove(string.Format("{0}-{1}-hb-{2}", baojiaCacheKey, i, "key"));
                        //    });
                    }
                    //tran.Flush();
                }
                catch (Exception ex)
                {
                    logError.Info("清空缓存失败:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                }
                //}
                //}
                var msgBody = new
                {
                    B_Uid = userinfo.Id,
                    IsCloseSms = 0,
                    NotifyCacheKey = baojiaCacheKey,
                    RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                //发送续保信息
                var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                     ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["BxBaoJiaOrHeBaoName"]);
                response.Status = HttpStatusCode.OK;
                logInfo.Info("重新报价信息发送成功:" + request.ToJson() + "\n\n 消息体：" + msgbody.ToJson() + ",报价键值：" + baojiaCacheKey);
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("根据行驶证请求报价发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }
        /// <summary>
        /// 获取车辆报价信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async Task<GetPrecisePriceReponse> GetPrecisePrice(GetPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetPrecisePriceReponse response = new GetPrecisePriceReponse();
            //校验
            BaseResponse baseResponse = _checkRequestGetPrecisePrice.CheckRequest(request, pairs);
            if (baseResponse.Status == HttpStatusCode.Forbidden)
            {
                response.Status = HttpStatusCode.Forbidden;
                response.ErrMsg = baseResponse.ErrMsg;
                return response;
            }
            //报价组合
            request = GetQuoteGroup(request);
            #region 读取数据
            #region 读取缓存
            //微信端逻辑 次级代理
            if (request.ChildAgent > 0)
            {
                request.Agent = request.ChildAgent;
            }
            ExecutionContext.SuppressFlow();
            response = await _carInsuranceCache.GetPrecisePrice(request);
            //报价返回渠道信息
            if (request.ShowChannel == 1)
            {
                if (response.SubmitInfo != null && response.SubmitInfo.channel_id.HasValue)
                {
                    response.AgentConifg = _agentConfig.FindById(response.SubmitInfo.channel_id.Value);
                }
            }


            if (response.BusinessStatus != 1 && response.BusinessStatus != 3)
            {
                #region 读库
                try
                {
                    bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.Agent.ToString(), request.RenewalCarType);
                    if (userinfo == null)
                    {
                        response.Status = HttpStatusCode.ExpectationFailed;
                        return response;
                    }
                    response.UserInfo = userinfo;
                    response.LastInfo = _infoRepository.GetByBuid(response.UserInfo.Id);
                    //查询保额
                    response.SaveQuote = _saveQuoteRepository.GetSavequoteByBuid(response.UserInfo.Id);
                    //查询保费
                    response.QuoteResult = _quoteResultRepository.GetQuoteResultByBuid(response.UserInfo.Id, Convert.ToInt32(request.IntentionCompany));
                    response.SubmitInfo = _submitInfoRepository.GetSubmitInfo(response.UserInfo.Id, Convert.ToInt32(request.IntentionCompany));
                    response.CarInfo = _quoteResultCarinfoRepository.Find(response.UserInfo.Id,
                        Convert.ToInt32(request.IntentionCompany));
                    response.YwxDetails = _ywxdetailRepository.GetList(response.UserInfo.Id);
                    if (response.YwxDetails.Any())
                    {
                        response.YwxDetails = response.YwxDetails.Where(l => l.source == Convert.ToInt32(request.IntentionCompany)).ToList();
                    }
                }
                catch (Exception ex)
                {
                    MetricUtil.UnitReports("getprice_service");
                    response.Status = HttpStatusCode.ExpectationFailed;
                    logError.Info("获取报价数据发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                }
                #endregion
            }
            #endregion
            #endregion
            response.Status = HttpStatusCode.OK;
            return response;
        }

        #region 续保接口拆分
        public async Task<GetLicenseResponse> GetVenchileLincenseInfo(GetReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {

            var response = new GetLicenseResponse();
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
                        response.BusinessMessage = string.Format("参数校验错误，账号已过期。过期时间为：{0}", agentModel.endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        return response;
                    }
                    response.BusinessMessage = "参数校验错误，账号已禁用。";
                    return response;
                }
                if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                #region userinfo逻辑
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
                        return new GetLicenseResponse
                        {
                            BusinessMessage = "您的账号已被禁用，如有疑问请联系管理员。",
                            Status = HttpStatusCode.Forbidden
                        };
                    }
                }
                ///先从库里读取,没有在走后续流程
                if (request.IsDirectRenewal == 1)
                {
                    if (request.GetAllCar == 1)
                    {
                        response.Carinfo = _carInfoRepository.Find(request.LicenseNo);
                    }
                    else
                    {
                        response.Carinfo = _carInfoRepository.FindOrderDate(request.LicenseNo, request.RenewalCarType);
                    }
                    if (response.Carinfo != null)
                    {
                        response.BusinessStatus = 1;
                        response.BusinessMessage = "成功获取到行驶证信息";
                    }
                    else
                    {
                        //response.BusinessStatus = 0;
                        //response.BusinessMessage = "没有获取到到行驶证信息";
                        //针对车架号+发动机号的逻辑
                        bx_userinfo userinfo = UserinfoSearchFactory.Find(request, _userInfoRepository);

                        if (userinfo == null)
                        {
                            //获取车五项的因为都是接口用户，所以不用关注分配状态和角色问题，默认传0
                            userinfo = UserinfoMakeFactory.Save(request, 0, _userInfoRepository);
                            if (request.IsLastYearNewCar == 1)
                            {
                                isReadCache = await CheckCarNeedDrivingInfo(userinfo);
                            }
                        }
                        else
                        {
                            userinfo = UserinfoMakeFactory.Update(request, userinfo, _userInfoRepository, agentModel.TopAgentId);
                            bool isNeedCheckNeed = !(request.IsLastYearNewCar == 1 & (!string.IsNullOrWhiteSpace(request.CarVin) && !string.IsNullOrWhiteSpace(request.EngineNo)));
                            if (request.IsLastYearNewCar == 1)
                            {
                                if (userinfo.NeedEngineNo == 1)
                                {
                                    if (isNeedCheckNeed)
                                    {
                                        isReadCache = await CheckCarNeedDrivingInfo(userinfo);
                                    }
                                }
                            }
                        }

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
                            //CacheProvider.Set(reqCacheKey, quotereq, 7200);
                        }
                        #endregion

                        if (request.IsCallNext == 1)
                        {
                            CacheProvider.Set(string.Format("{0}_iscallnext", buid), 1, 3600);
                            #region 发送续保消息
                            string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(request.LicenseNo + request.RenewalCarType);
                            var xuBaoKey = string.Format("{0}-xb-{1}", xuBaoCacheKey, "key");
                            CacheProvider.Remove(xuBaoKey);
                            var msgBody = new
                            {
                                B_Uid = buid,
                                IsCloseSms = 0,
                                NotifyCacheKey = xuBaoCacheKey
                            };
                            //发送续保信息
                            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                                ConfigurationManager.AppSettings["MessageCenter"],
                                ConfigurationManager.AppSettings["BxXuBaoName"]);
                            #endregion
                        }
                        else
                        {
                            CacheProvider.Set(string.Format("{0}_iscallnext", buid), 0, 3600);
                        }

                        //response.UserInfo = _userInfoRepository.FindByBuid(buid);
                        if (request.GetAllCar == 1)
                        {
                            response.Carinfo = _carInfoRepository.Find(request.LicenseNo);
                        }
                        else
                        {
                            response.Carinfo = _carInfoRepository.FindOrderDate(request.LicenseNo, request.RenewalCarType);
                        }
                        if (response.Carinfo != null)
                        {
                            response.BusinessStatus = 1;
                            response.BusinessMessage = "成功获取到行驶证信息";
                        }
                        else
                        {
                            response.BusinessStatus = 0;
                            response.BusinessMessage = "没有获取到到行驶证信息";
                        }
                    }
                    response.Status = HttpStatusCode.OK;
                    return response;
                }
                else
                {
                    //针对车架号+发动机号的逻辑
                    bx_userinfo userinfo = UserinfoSearchFactory.Find(request, _userInfoRepository);

                    if (userinfo == null)
                    {
                        //获取车五项的因为都是接口用户，所以不用关注分配状态和角色问题，默认传0
                        userinfo = UserinfoMakeFactory.Save(request, 0, _userInfoRepository);
                        if (request.IsLastYearNewCar == 1)
                        {
                            isReadCache = await CheckCarNeedDrivingInfo(userinfo);
                        }
                    }
                    else
                    {
                        userinfo = UserinfoMakeFactory.Update(request, userinfo, _userInfoRepository, agentModel.TopAgentId);
                        bool isNeedCheckNeed = !(request.IsLastYearNewCar == 1 & (!string.IsNullOrWhiteSpace(request.CarVin) && !string.IsNullOrWhiteSpace(request.EngineNo)));
                        if (request.IsLastYearNewCar == 1)
                        {
                            if (userinfo.NeedEngineNo == 1)
                            {
                                if (isNeedCheckNeed)
                                {
                                    isReadCache = await CheckCarNeedDrivingInfo(userinfo);
                                }
                            }
                        }
                    }

                    long buid = userinfo.Id;

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
                        //CacheProvider.Set(reqCacheKey, quotereq, 7200);
                    }
                    #endregion

                    if (request.IsCallNext == 1)
                    {
                        CacheProvider.Set(string.Format("{0}_iscallnext", buid), 1, 3600);
                        #region 发送续保消息
                        string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(request.LicenseNo + request.RenewalCarType);
                        var xuBaoKey = string.Format("{0}-xb-{1}", xuBaoCacheKey, "key");
                        CacheProvider.Remove(xuBaoKey);
                        var msgBody = new
                        {
                            B_Uid = buid,
                            IsCloseSms = 0,
                            NotifyCacheKey = xuBaoCacheKey
                        };
                        //发送续保信息
                        var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                            ConfigurationManager.AppSettings["MessageCenter"],
                            ConfigurationManager.AppSettings["BxXuBaoName"]);
                        #endregion
                    }
                    else
                    {
                        CacheProvider.Set(string.Format("{0}_iscallnext", buid), 0, 3600);
                    }

                    //response.UserInfo = _userInfoRepository.FindByBuid(buid);
                    if (request.GetAllCar == 1)
                    {
                        response.Carinfo = _carInfoRepository.Find(request.LicenseNo);
                    }
                    else
                    {
                        response.Carinfo = _carInfoRepository.FindOrderDate(request.LicenseNo, request.RenewalCarType);
                    }
                    if (response.Carinfo != null)
                    {
                        response.BusinessStatus = 1;
                        response.BusinessMessage = "成功获取到行驶证信息";
                    }
                    else
                    {
                        response.BusinessStatus = 0;
                        response.BusinessMessage = "没有获取到到行驶证信息";
                    }
                    response.Status = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                response = new GetLicenseResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("车五项续保请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }
        public async Task<GetReInfoResponse> GetRenewalInfo(GetReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetReInfoResponse();
            try
            {
                //代理人校验
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
                if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                #region userinfo逻辑
                //微信端逻辑 次级代理
                if (request.ChildAgent > 0)
                {
                    var item = GetAgentModelFactory(request.ChildAgent);
                    if (item.AgentCanUse())
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
                //针对车架号+发动机号的逻辑
                bx_userinfo userinfo = UserinfoSearchFactory.Find(request, _userInfoRepository);
                if (userinfo == null)
                {
                    return new GetReInfoResponse
                    {
                        BusinessMessage = "您还未发起续保请求",
                        Status = HttpStatusCode.Forbidden
                    };
                }
                long buid = userinfo.Id;
                var iscallnext = CacheProvider.Get<string>(string.Format("{0}_iscallnext", buid));
                if (iscallnext != "1")
                {
                    if (iscallnext == null && userinfo.RenewalStatus != -1)
                    {
                    }
                    else
                    {
                        return new GetReInfoResponse
                        {
                            BusinessMessage = "您还未发起续保请求",
                            Status = HttpStatusCode.PartialContent
                        };
                    }
                }
                #endregion
                #region bx_quotereq_carinfo 逻辑
                var reqCacheKey = string.Format("{0}-{1}", buid, "reqcarinfo");
                var quotereq = _quoteReqCarinfoRepository.Find(buid);
                #endregion
                #region 发送续保消息
                string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(request.LicenseNo);
                var xuBaoKey = string.Format("{0}-xb-{1}", xuBaoCacheKey, "key");
                #endregion
                #region 读取数据
                #region 缓存读取
                ExecutionContext.SuppressFlow();
                response = await _carInsuranceCache.GetReInfo(request);
                var tmpRep = CacheProvider.Get<bx_quotereq_carinfo>(reqCacheKey);
                if (tmpRep == null)
                {
                    response.ReqCarinfo = _quoteReqCarinfoRepository.Find(buid);
                    CacheProvider.Set(reqCacheKey, quotereq);
                }
                else
                {
                    response.ReqCarinfo = CacheProvider.Get<bx_quotereq_carinfo>(reqCacheKey);
                }
                #endregion
                if (response.BusinessStatus == 4)
                {
                    bool isOver = false;
                    #region  读库数据
                    isOver = GetReInfoState(buid);
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
                #endregion
            }
            catch (Exception ex)
            {
                response = new GetReInfoResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("续保请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }

        #endregion
        public async Task<GetCancelSubmitResponse> GetCancelSubmit(GetCancelSubmitRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetCancelSubmitResponse response = new GetCancelSubmitResponse();
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //参数校验
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //微信端逻辑 次级代理
            if (request.ChildAgent > 0)
            {
                request.Agent = request.ChildAgent;
            }
            //报价组合
            request = GetCancelSubmitGroup(request);
            var userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.Agent.ToString(), request.RenewalCarType);
            if (userinfo == null)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            var submitInfo = _submitInfoRepository.GetSubmitInfo(userinfo.Id, request.SubmitGroup);
            if (submitInfo == null)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            if (submitInfo.submit_status != 1)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            if (!string.IsNullOrWhiteSpace(request.BizNo))
            {
                if (string.IsNullOrWhiteSpace(submitInfo.biz_tno))
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                if (request.BizNo.ToLower() != submitInfo.biz_tno.ToLower())
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
            }
            if (!string.IsNullOrWhiteSpace(request.ForceNo))
            {
                if (string.IsNullOrWhiteSpace(submitInfo.force_tno))
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                if (request.ForceNo.ToLower() != submitInfo.force_tno.ToLower())
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
            }
            var makeCancleKey = string.Format("CancelSubmit-{0}-{1}", string.IsNullOrWhiteSpace(request.BizNo) ? "0" : request.BizNo, string.IsNullOrWhiteSpace(request.ForceNo) ? "1" : request.ForceNo);
            string cancleSubmitKey = CommonCacheKeyFactory.CreateKeyWithLicense(makeCancleKey);
            CacheProvider.Remove(cancleSubmitKey);
            var msgBody = new
            {
                B_Uid = userinfo.Id,
                BiztNo = string.IsNullOrWhiteSpace(request.BizNo) ? string.Empty : request.BizNo,
                ForcetNo = string.IsNullOrWhiteSpace(request.ForceNo) ? string.Empty : request.ForceNo,
                Source = request.SubmitGroup,
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                NotifyCacheKey = cancleSubmitKey
            };
            //发送获取车辆信息队列
            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(), ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["BxCancelSubmit"]);
            var cacheValue = CacheProvider.Get<CancelSubmitResult>(cancleSubmitKey);
            if (cacheValue == null)
            {
                for (int i = 0; i < 120; i++)
                {
                    cacheValue = CacheProvider.Get<CancelSubmitResult>(cancleSubmitKey);
                    if (cacheValue != null)
                    {
                        break;
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(500));
                    }
                }
            }
            var result = new CancelSubmitResult();
            if (cacheValue != null)
            {
                //result = cacheValue.FromJson<CancelSubmitResult>();
                response.BusinessStatus = 1;
                response.BusinessMessage = "请求成功";
                result.BizStatus = cacheValue.BizStatus;
                result.BizMessage = cacheValue.BizMessage;
                if (request.SubmitGroup == 1)
                {
                    result.ForceStatus = cacheValue.BizStatus;
                    result.ForceMessage = cacheValue.BizMessage;
                }
                else
                {
                    result.ForceMessage = cacheValue.ForceMessage;
                    result.ForceStatus = cacheValue.ForceStatus;
                }
                if (result.BizStatus == 0)
                {
                    result.BizMessage = "未处理";
                }
                if (result.ForceStatus == 0)
                {
                    result.ForceMessage = "未处理";
                }
            }
            else
            {
                response.BusinessStatus = 0;
                response.BusinessMessage = "发生异常";
            }
            response.Result = result;
            return response;
        }
        /// <summary>
        /// 普通查询车型接口
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<GetCarVehicleInfoResponse> GetCarVehicle(GetCarVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetCarVehicleInfoResponse response = new GetCarVehicleInfoResponse();
            //代理人校验
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
            //参数校验
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            var topagent = request.Agent;
            //微信端逻辑 次级代理
            if (request.ChildAgent > 0)
            {
                request.Agent = request.ChildAgent;
            }
            //针对车架号+发动机号的逻辑
            bx_userinfo userinfo = null;
            string xuBaoCacheKey;
            if (!string.IsNullOrWhiteSpace(request.EngineNo) && !string.IsNullOrWhiteSpace(request.CarVin))
            {
                userinfo = _userInfoRepository.FindByCarvin(request.CarVin, request.EngineNo, request.CustKey,
                     request.Agent.ToString(), request.CarType);
                xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(string.Format("{0}{1}{2}", request.CarVin, request.EngineNo, request.MoldName + request.CarType));
            }
            else
            {
                userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo,
                   request.Agent.ToString(), request.CarType);
                xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(string.Format("{0}{1}", request.LicenseNo, request.MoldName + request.CarType));
            }
            #region 新增逻辑 平安报价需要区分类型
            //userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.Agent.ToString(), request.CarType);
            if (userinfo != null)
            {
                var reqItem = _quoteReqCarinfoRepository.Find(userinfo.Id);
                if (reqItem != null)
                {
                    reqItem.pingan_quote_type = request.IsNeedCarVin;
                    _quoteReqCarinfoRepository.Update(reqItem);
                }
                else
                {
                    reqItem = new bx_quotereq_carinfo
                    {
                        pingan_quote_type = request.IsNeedCarVin
                    };
                    _quoteReqCarinfoRepository.Add(reqItem);
                }
            }
            #endregion
            //if (userinfo == null)
            //{
            //    response.Status = HttpStatusCode.Forbidden;
            //    return response;
            //}
            if (request.MoldName.Trim().Length == 0)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            var xuBaoKey = string.Format("{0}-{1}-carmodel-key", xuBaoCacheKey, request.IsNeedCarVin);
            CacheProvider.Remove(xuBaoKey);
            //获取品牌型号
            var moldNameViewModle = await _getMoldNameFromCenter.GetMoldNameService(request.CarVin, request.MoldName, topagent, request.CityCode);
            if (moldNameViewModle != null && !string.IsNullOrEmpty(moldNameViewModle.MoldName))
                request.MoldName = moldNameViewModle.MoldName;
            #region 老的获取品牌型号方法
            //if (!string.IsNullOrWhiteSpace(request.CarVin))
            //{
            //    if (request.CarVin.Length > 5)
            //    {
            //        var frontCarVin = request.CarVin.Substring(0, 5);
            //        if (!request.CarVin.StartsWith("L") && request.MoldName.ToUpper().IndexOf(frontCarVin, System.StringComparison.Ordinal) >= 0)
            //        {
            //            using (var client = new HttpClient())
            //            {
            //                client.BaseAddress = new Uri(_url);
            //                var getUrl = string.Format("api/taipingyang/gettaipycarinfoby?carvin={0}", request.CarVin);
            //                HttpResponseMessage responseVin = client.GetAsync(getUrl).Result;
            //                var resultVin = responseVin.Content.ReadAsStringAsync().Result;
            //                var carinfo = resultVin.FromJson<WaGetTaiPyCarInfoResponse>();
            //                if (carinfo != null && carinfo.CarInfo != null)
            //                {
            //                    request.MoldName = carinfo.CarInfo.moldName;
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion

            var msgBody = new
            {
                Agent = topagent,
                //B_Uid = userinfo != null ? userinfo.Id : 0,
                B_Uid = 0,
                VehicleName = request.MoldName,
                //cityId = userinfo != null ? userinfo.CityCode : request.CityCode.ToString(),
                cityId = request.CityCode.ToString(),
                NotifyCacheKey = xuBaoCacheKey,
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                IsNeedVin = request.IsNeedCarVin,
                CarVin = request.CarVin
            };
            //发送续保信息
            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                ConfigurationManager.AppSettings["MessageCenter"],
                ConfigurationManager.AppSettings["BxVechileQueue"]);
            try
            {
                var cacheKey = CacheProvider.Get<string>(xuBaoKey);
                if (cacheKey == null)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        cacheKey = CacheProvider.Get<string>(xuBaoKey);
                        if (!string.IsNullOrWhiteSpace(cacheKey))
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(0.5));
                        }
                    }
                }
                response.Vehicles = new List<BxCarVehicleInfo>();
                if (cacheKey == null)
                {
                    response.BusinessStatus = -1;//超时
                    response.BusinessMessage = "请求超时";
                }
                else
                {
                    string itemsCache = string.Format("{0}-{1}-carmodel", xuBaoCacheKey, request.IsNeedCarVin);
                    response.BusinessStatus = 1;
                    if (cacheKey == "1")
                    {
                        var temp = CacheProvider.Get<string>(itemsCache);
                        response.Vehicles = temp.FromJson<List<BxCarVehicleInfo>>();
                        if (response.Vehicles != null && response.Vehicles.Count > 1)
                        {
                            response.Vehicles = response.Vehicles.OrderBy(x => x.PriceT).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = new GetCarVehicleInfoResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("获取车型请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }

        /// <summary>
        /// 普通查询车型接口
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public async Task<GetCarVehicleInfoResponse> GetNewCarVehicle(GetCarVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetCarVehicleInfoResponse response = new GetCarVehicleInfoResponse();
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //参数校验
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            var topagent = request.Agent;
            //微信端逻辑 次级代理
            if (request.ChildAgent > 0)
            {
                request.Agent = request.ChildAgent;
            }
            //针对车架号+发动机号的逻辑
            bx_userinfo userinfo = null;
            string xuBaoCacheKey;
            if (!string.IsNullOrWhiteSpace(request.EngineNo) && !string.IsNullOrWhiteSpace(request.CarVin))
            {
                userinfo = _userInfoRepository.FindByCarvin(request.CarVin, request.EngineNo, request.CustKey,
                     request.Agent.ToString(), request.CarType);
                xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(string.Format("{0}{1}{2}", request.CarVin, request.EngineNo, request.MoldName + request.CarType));
            }
            else
            {
                userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo,
                   request.Agent.ToString(), request.CarType);
                xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(string.Format("{0}{1}", request.LicenseNo, request.MoldName + request.CarType));
            }
            #region 新增逻辑 平安报价需要区分类型
            //userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.Agent.ToString(), request.CarType);
            if (userinfo != null)
            {
                var reqItem = _quoteReqCarinfoRepository.Find(userinfo.Id);
                if (reqItem != null)
                {
                    reqItem.pingan_quote_type = request.IsNeedCarVin;
                    _quoteReqCarinfoRepository.Update(reqItem);
                }
                else
                {
                    reqItem = new bx_quotereq_carinfo
                    {
                        pingan_quote_type = request.IsNeedCarVin
                    };
                    _quoteReqCarinfoRepository.Add(reqItem);
                }
            }
            #endregion
            //if (userinfo == null)
            //{
            //    response.Status = HttpStatusCode.Forbidden;
            //    return response;
            //}
            if (request.MoldName.Trim().Length == 0)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            var xuBaoKey = string.Format("{0}-{1}-carmodel-key", xuBaoCacheKey, request.IsNeedCarVin);
            CacheProvider.Remove(xuBaoKey);

            //获取品牌型号
            var moldNameViewModle = await _getMoldNameFromCenter.GetMoldNameService(request.CarVin, request.MoldName, topagent, request.CityCode);
            if (moldNameViewModle != null && !string.IsNullOrEmpty(moldNameViewModle.MoldName))
                request.MoldName = moldNameViewModle.MoldName;
            #region 老获取品牌型号的方法
            //if (!string.IsNullOrWhiteSpace(request.CarVin))
            //{
            //    if (request.CarVin.Length > 5)
            //    {
            //        var frontCarVin = request.CarVin.Substring(0, 5);
            //        if (!request.CarVin.StartsWith("L") && request.MoldName.ToUpper().IndexOf(frontCarVin, System.StringComparison.Ordinal) >= 0)
            //        {
            //            using (var client = new HttpClient())
            //            {
            //                client.BaseAddress = new Uri(_url);
            //                var getUrl = string.Format("api/taipingyang/gettaipycarinfoby?carvin={0}", request.CarVin);
            //                HttpResponseMessage responseVin = client.GetAsync(getUrl).Result;
            //                var resultVin = responseVin.Content.ReadAsStringAsync().Result;
            //                var carinfo = resultVin.FromJson<WaGetTaiPyCarInfoResponse>();
            //                if (carinfo != null && carinfo.CarInfo != null)
            //                {
            //                    request.MoldName = carinfo.CarInfo.moldName;
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion
            var msgBody = new
            {
                Agent = topagent,
                //B_Uid = userinfo != null ? userinfo.Id : 0,
                B_Uid = 0,
                VehicleName = request.MoldName,
                //cityId = userinfo != null ? userinfo.CityCode : request.CityCode.ToString(),
                cityId = request.CityCode.ToString(),
                NotifyCacheKey = xuBaoCacheKey,
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                IsNeedVin = request.IsNeedCarVin,
                CarVin = request.CarVin
            };
            //发送续保信息
            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                ConfigurationManager.AppSettings["MessageCenter"],
                ConfigurationManager.AppSettings["BxVechileQueue"]);
            try
            {
                var cacheKey = CacheProvider.Get<string>(xuBaoKey);
                if (cacheKey == null)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        cacheKey = CacheProvider.Get<string>(xuBaoKey);
                        if (!string.IsNullOrWhiteSpace(cacheKey))
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(0.5));
                        }
                    }
                }
                response.Vehicles = new List<BxCarVehicleInfo>();
                if (cacheKey == null)
                {
                    response.BusinessStatus = -1;//超时
                    response.BusinessMessage = "请求超时";
                }
                else
                {
                    string itemsCache = string.Format("{0}-{1}-carmodel", xuBaoCacheKey, request.IsNeedCarVin);
                    response.BusinessStatus = 1;
                    if (cacheKey == "1")
                    {
                        var temp = CacheProvider.Get<string>(itemsCache);
                        response.Vehicles = temp.FromJson<List<BxCarVehicleInfo>>();
                        if (response.Vehicles != null && response.Vehicles.Count > 1)
                        {
                            response.Vehicles = response.Vehicles.OrderBy(x => x.PriceT).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = new GetCarVehicleInfoResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("获取车型请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }

        public async Task<CheckCarVehicleInfoResponse> CheckCarVehicle(GetNewCarSecondVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            CheckCarVehicleInfoResponse response = new CheckCarVehicleInfoResponse();
            //代理人校验
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
            //参数校验
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //微信端逻辑 次级代理
            if (request.ChildAgent > 0)
            {
                request.Agent = request.ChildAgent;
            }
            //if (request.MoldName.Trim().Length == 0)
            //{
            //    response.Status = HttpStatusCode.Forbidden;
            //    return response;
            //}
            string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(request.VehicleNo);
            var xuBaoKey = string.Format("{0}-carmodel2-key", xuBaoCacheKey);
            CacheProvider.Remove(xuBaoKey);
            var msgBody = new
            {
                Agent = request.Agent,
                //B_Uid = userinfo.Id,
                RegisterDate = string.IsNullOrWhiteSpace(request.RegisterDate) ? DateTime.Now.Date.ToString("yyyy-MM-dd") : request.RegisterDate,
                VehicleNo = request.VehicleNo,
                CarVin = request.CarVin,
                EngineNo = request.EngineNo,
                VehicleName = request.VehicleName,
                cityId = request.CityCode,
                NotifyCacheKey = xuBaoCacheKey,
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                IsCheckVehicleNo = 1,//就是校验专用
                CarType = request.CarType,
                MotorUsageTypeCode = request.CarUsedType,
                MotorTypeCode = string.Empty
            };
            //发送续保信息
            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(), ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["BxVechileQueueNewCar"]);
            try
            {
                var cacheKey = CacheProvider.Get<string>(xuBaoKey);
                if (cacheKey == null)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        cacheKey = CacheProvider.Get<string>(xuBaoKey);
                        if (!string.IsNullOrWhiteSpace(cacheKey))
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(0.5));
                        }
                    }
                }
                response.BusinessStatus = 1;


                if (cacheKey == null)
                {
                    response.BusinessStatus = -1;//超时
                    response.BusinessMessage = "请求超时";
                }
                else
                {
                    if (cacheKey == "1")
                    {
                        response.CheckCode = 0;
                        response.CheckMessage = "该车型可以全渠道报价";
                    }
                    else if (cacheKey == "-11000")
                    {
                        response.CheckCode = -1;
                        var msgkey = string.Format("{0}-carmodel2-msg", xuBaoCacheKey);
                        var msg = CacheProvider.Get<string>(msgkey);
                        response.CheckMessage = string.IsNullOrWhiteSpace(msg) ? string.Empty : msg;
                    }
                    else if (cacheKey == "-10002")
                    {
                        response.CheckCode = -1;
                        response.CheckMessage = "非新车,禁止按照新车报价";
                    }
                    else if (cacheKey == "-10003")
                    {
                        response.CheckCode = -1;
                        response.CheckMessage = "当前选择车型与平台返回信息不一致（输入参数可能不匹配）";
                    }
                    else if (cacheKey == "-10004")
                    {
                        response.CheckCode = -1;
                        response.CheckMessage = "保险公司服务发生异常";
                    }

                    var msgkey3 = string.Format("{0}-carmodel2-chexing", xuBaoCacheKey);
                    var cachevalue = CacheProvider.Get<string>(msgkey3);
                    var msg3 = cachevalue.FromJson<BxCarVehicleInfo>();
                    //var msg3 = CacheProvider.Get<BxCarVehicleInfo>(msgkey3);

                    //response.CheckCode = 1;
                    response.TypeName = msg3 != null ? msg3.VehicleClassName : string.Empty;
                    response.CarType = msg3 != null ? msg3.VehicleClassType : string.Empty;
                    //response.CheckMessage = "车牌类型校验";

                    logError.Info("车型校验第二步：请求串：" + request.ToJson() + "  返回的缓存值：" + cacheKey + "     carmodel2-chexing value是：" + cachevalue + "  carmodel2-msg:" + response.CheckMessage);
                }
            }
            catch (Exception ex)
            {
                response = new CheckCarVehicleInfoResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("报价车型校验请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }
        public async Task<GetMoldNameResponse> GetMoldName(GetMoldNameRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetMoldNameResponse response = new GetMoldNameResponse();
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //参数校验
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            ////微信端逻辑 次级代理
            //if (request.ChildAgent > 0)
            //{
            //    request.Agent = request.ChildAgent;
            //}
            //if (request.MoldName.Trim().Length == 0)
            //{
            //    response.Status = HttpStatusCode.Forbidden;
            //    return response;
            //}
            string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(request.CarVin);
            var xuBaoKey = string.Format("{0}-ModelName-key", xuBaoCacheKey);
            CacheProvider.Remove(xuBaoKey);
            var msgBody = new
            {
                Agent = request.Agent,
                //B_Uid = 0,
                CarVin = request.CarVin,
                cityId = request.CityCode,
                NotifyCacheKey = xuBaoCacheKey,
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            //发送续保信息
            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(), ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["FindMoldName"]);
            try
            {
                var cacheKey = CacheProvider.Get<string>(xuBaoKey);
                if (cacheKey == null)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        cacheKey = CacheProvider.Get<string>(xuBaoKey);
                        if (!string.IsNullOrWhiteSpace(cacheKey))
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(0.5));
                        }
                    }
                }
                if (cacheKey == null)
                {
                    response.BusinessStatus = -1;//超时
                    response.BusinessMessage = "请求超时";
                    response.MoldName = string.Empty;
                }
                else
                {
                    WaPaAutoModelResponse model = new WaPaAutoModelResponse();
                    string itemsCache = string.Format("{0}-ModelName", xuBaoCacheKey);
                    response.BusinessStatus = 1;
                    if (cacheKey == "1")
                    {
                        var temp = CacheProvider.Get<string>(itemsCache);
                        model = temp.FromJson<WaPaAutoModelResponse>();
                        if (model.ErrCode == 0)
                        {
                            response.MoldName = model.AutoModeType != null ? model.AutoModeType.autoModelName : string.Empty;
                            if (model.AutoModeType == null)
                            {
                                response.BusinessMessage = "没有取到对应的品牌型号";
                            }
                            else
                            {
                                response.BusinessMessage = "获取成功";
                            }
                        }
                        else
                        {
                            response.MoldName = string.Empty;
                            response.BusinessMessage = "没有取到对应的品牌型号";
                            response.BusinessStatus = -10002;
                        }
                    }
                    else
                    {
                        response.BusinessMessage = "没有取到对应的品牌型号";
                        response.BusinessStatus = -10002;
                        response.MoldName = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                response = new GetMoldNameResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("获取车型请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }
        private async Task<bool> CheckCarNeedDrivingInfo(bx_userinfo userinfo)
        {
            #region 修改前的方法
            //bool isSuccess = true;
            //var requestmodel = new
            //{
            //    //Mobile = userinfo.Mobile,
            //    LicenseNo = userinfo.LicenseNo,
            //    CityCode = userinfo.CityCode,
            //    RenewalIdNo = userinfo.RenewalIdNo,
            //    Agent = userinfo.Agent,
            //    Id = userinfo.Id
            //};
            //logInfo.Info("行驶证信息请求进来额:" + requestmodel.ToJson());
            //try
            //{
            //    using (var client = new HttpClient())
            //    {
            //        client.BaseAddress = new Uri(_url);
            //        HttpContent content = new StringContent(requestmodel.ToJson());
            //        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //        HttpResponseMessage responseCheck = client.PostAsync("api/userinfo/CheckNeedAdd", content).Result;
            //        var resultJson = responseCheck.Content.ReadAsStringAsync().Result;
            //        if (resultJson == "false")
            //        {
            //            logInfo.Info("太平洋check信息返回成功:" + requestmodel.ToJson());
            //        }
            //        else
            //        {
            //            isSuccess = false;
            //            logError.Info("太平洋check信息返回失败:" + requestmodel.ToJson());
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    logError.Info("checkneed 出现异常:" + requestmodel.ToJson());
            //    return false;
            //}
            //return isSuccess;
            #endregion
            bool isSuccess = false;
            //string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(userinfo.Id.ToString());
            string xuBaoCacheKey = userinfo.Id.ToString();
            var msgBody = new
            {
                B_Uid = userinfo.Id,
                NotifyCacheKey = xuBaoCacheKey
            };
            //发送获取车辆信息队列
            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                ConfigurationManager.AppSettings["MessageCenter"],
                 ConfigurationManager.AppSettings["BxVehicle"]);
            var cacheKey = string.Format("{0}-findvehicle", xuBaoCacheKey);
            // CacheProvider.Remove(cacheKey);
            var cacheValue = CacheProvider.Get<string>(cacheKey);
            if (cacheValue == null)
            {
                for (int i = 0; i < 60; i++)
                {
                    cacheValue = CacheProvider.Get<string>(cacheKey);
                    if (!string.IsNullOrWhiteSpace(cacheValue))
                    {
                        break;
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(500));
                    }
                }
            }
            return isSuccess;
        }
        private bool GetReInfoState(long buid)
        {
            var userinfo = _userInfoRepository.FindByBuid(buid);
            bool isContinue = true;
            if (userinfo.NeedEngineNo == 1)
            {//需要完善行驶证信息
                isContinue = false;
            }
            if (userinfo.NeedEngineNo == 0 && userinfo.RenewalStatus != 1)
            {  //获取车辆信息成功，但获取险种失败
                isContinue = false;
            }
            if ((userinfo.NeedEngineNo == 0 && userinfo.LastYearSource > -1) || userinfo.RenewalStatus == 1)
            {  //续保成功
                isContinue = false;
            }
            return isContinue;
        }
        private HttpStatusCode CheckRequestGroup(PostPrecisePriceRequest request)
        {
            if (request.QuoteGroup > 0)
            {
                string agentCacheKey = string.Format("{0}_{1}_agentsetting", request.Agent, request.CityCode);
                var agentCache = CacheProvider.Get<List<int>>(agentCacheKey);
                if (agentCache != null)
                {
                    if (agentCache.Count == 0)
                    {
                        return HttpStatusCode.NoContent;
                    }
                    Dictionary<int, long> keyInts = new Dictionary<int, long>();
                    for (int i = 0; i < agentCache.Count; i++)
                    {
                        if (agentCache[i] == 0)
                        {
                            keyInts.Add(0, 2);
                        }
                        else if (agentCache[i] == 1)
                        {
                            keyInts.Add(1, 1);
                        }
                        else if (agentCache[i] == 2)
                        {
                            keyInts.Add(2, 4);
                        }
                        else if (agentCache[i] == 3)
                        {
                            keyInts.Add(3, 8);
                        }
                        else if (agentCache[i] == 4)
                        {
                            keyInts.Add(4, 16);
                        }
                        else if (agentCache[i] == 5)
                        {
                            keyInts.Add(5, 32);
                        }
                        else if (agentCache[i] == 6)
                        {
                            keyInts.Add(6, 64);
                        }
                        else if (agentCache[i] == 7)
                        {
                            keyInts.Add(7, 128);
                        }
                        else if (agentCache[i] == 8)
                        {
                            keyInts.Add(8, 256);
                        }
                        else if (agentCache[i] == 9)
                        {
                            keyInts.Add(9, 512);
                        }
                        else if (agentCache[i] == 10)
                        {
                            keyInts.Add(10, 1024);
                        }
                        else if (agentCache[i] == 11)
                        {
                            keyInts.Add(11, 2048);
                        }
                        else if (agentCache[i] == 12)
                        {
                            keyInts.Add(11, 4096);
                        }
                    }
                    var avalibleValues = keyInts.Values.ToArray();
                    List<long> canGroups = AdditionGroupAlgorithm.GetGroups(avalibleValues);
                    if (canGroups.All(x => x != request.QuoteGroup))
                    {
                        return HttpStatusCode.NotAcceptable;
                    }
                    canGroups.Add(0);
                    if (canGroups.All(x => x != request.SubmitGroup))
                    {
                        return HttpStatusCode.NotAcceptable;
                    }
                }
            }
            return HttpStatusCode.OK;
        }
        /// <summary>
        /// 向下兼容：如果是老逻辑，则需要把各种情况转换到新情况里
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private PostPrecisePriceRequest GetQuoteAndSubmitGroup(PostPrecisePriceRequest request)
        {
            string agentCacheKey = string.Format("{0}_{1}_agentsetting", request.Agent, request.CityCode);
            var agentCache = CacheProvider.Get<List<int>>(agentCacheKey);
            if (agentCache == null)
            {
                int topAgent = request.ChildAgent > 0
                    ? int.Parse(_agentRepository.GetTopAgentId(request.Agent))
                    : request.Agent;
                var sourceList = _agentConfig.FindBy(topAgent, request.CityCode);
                agentCache =
                    sourceList.Select(x => (x.source.HasValue ? x.source.Value : 0)).Distinct().ToList();
                CacheProvider.Set(agentCacheKey, agentCache, 600);
            }
            //老逻辑
            if (request.QuoteGroup == 0 && request.SubmitGroup == 0)
            {
                //全部报价 不核保
                if (request.IsSingleSubmit == 0 && request.IntentionCompany == -1)
                {
                    int single = 0;
                    for (int i = 0; i < agentCache.Count; i++)
                    {
                        if (agentCache[i] == 0)
                        {
                            single += 2;
                        }
                        else if (agentCache[i] == 1)
                        {
                            single += 1;
                        }
                        else if (agentCache[i] == 2)
                        {
                            single += 4;
                        }
                        else if (agentCache[i] == 3)
                        {
                            single += 8;
                        }
                    }
                    request.IsSingleSubmit = single;
                    request.IntentionCompany = 0;
                }
                //一家报价 不核保
                if (request.IsSingleSubmit == 2 && request.IntentionCompany > -1)
                {
                    int submit = 0;
                    if (request.IntentionCompany == 0)
                    {
                        submit = 2;
                    }
                    else if (request.IntentionCompany == 1)
                    {
                        submit = 1;
                    }
                    else if (request.IntentionCompany == 2)
                    {
                        submit = 4;
                    }
                    else if (request.IntentionCompany == 3)
                    {
                        submit = 8;
                    }
                    request.IsSingleSubmit = submit;
                    request.IntentionCompany = 0;
                }
                //三家报价，一家核保
                if (request.IsSingleSubmit == 0 && request.IntentionCompany > -1)
                {
                    int single = 0;
                    for (int i = 0; i < agentCache.Count; i++)
                    {
                        if (agentCache[i] == 0)
                        {
                            single += 2;
                        }
                        else if (agentCache[i] == 1)
                        {
                            single += 1;
                        }
                        else if (agentCache[i] == 2)
                        {
                            single += 4;
                        }
                        else if (agentCache[i] == 3)
                        {
                            single += 8;
                        }
                    }
                    request.IsSingleSubmit = single;
                    int submit = 0;
                    if (request.IntentionCompany == 0)
                    {
                        submit = 2;
                    }
                    else if (request.IntentionCompany == 1)
                    {
                        submit = 1;
                    }
                    else if (request.IntentionCompany == 2)
                    {
                        submit = 4;
                    }
                    else if (request.IntentionCompany == 3)
                    {
                        submit = 8;
                    }
                    request.IntentionCompany = submit;
                }
                //单独报价单独核保
                if (request.IsSingleSubmit == 1 && request.IntentionCompany > -1)
                {
                    int submit = 0;
                    if (request.IntentionCompany == 0)
                    {
                        submit = 2;
                    }
                    else if (request.IntentionCompany == 1)
                    {
                        submit = 1;
                    }
                    else if (request.IntentionCompany == 2)
                    {
                        submit = 4;
                    }
                    else if (request.IntentionCompany == 3)
                    {
                        submit = 8;
                    }
                    request.IsSingleSubmit = submit;
                    request.IntentionCompany = submit;
                }
            }
            else
            {
                request.IsSingleSubmit = request.QuoteGroup;
                request.IntentionCompany = request.SubmitGroup;
            }
            return request;
        }
        /// <summary>
        /// 兼容报价 如果是新逻辑，则需要转换成老的逻辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private GetPrecisePriceRequest GetQuoteGroup(GetPrecisePriceRequest request)
        {
            if (request.QuoteGroup > 0)
            {
                //修改为通用的转换方法 by.20180904.gpj
                int submit = SourceGroupAlgorithm.GetOldSource(request.QuoteGroup);
                request.IntentionCompany = submit.ToString();
            }
            return request;
        }
        /// <summary>
        /// 兼容老逻辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private GetSubmitInfoRequest GetSubmitGroup(GetSubmitInfoRequest request)
        {
            if (request.SubmitGroup > 0)
            {
                //修改为通用的转换方法 by.20180904.gpj
                int submit = SourceGroupAlgorithm.GetOldSource(request.SubmitGroup);
                request.IntentionCompany = submit;
            }
            return request;
        }
        private GetCancelSubmitRequest GetCancelSubmitGroup(GetCancelSubmitRequest request)
        {
            if (request.SubmitGroup > 0)
            {
                int submit = request.SubmitGroup;
                //修改调用通用方法 by.20180904.gpj
                submit = SourceGroupAlgorithm.GetOldSource(request.SubmitGroup);
                request.SubmitGroup = submit;
            }
            return request;
        }
        public void Test(int num)
        {
            for (int j = 0; j < num; j++)
            {
                //using (var tran = RedisManager.GetClient().CreateTransaction())
                //{
                //    string baojiaCacheKey = CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey("京FF1234", 102, "18310825788");
                //    try
                //    {
                //        for (var i = 0; i < 12; i++)
                //        {
                //            tran.QueueCommand(p =>
                //            {
                //                p.Remove(string.Format("{0}-{1}-bj-{2}", baojiaCacheKey, i, "key"));
                //            });
                //            tran.QueueCommand(p =>
                //            {
                //                p.Remove(string.Format("{0}-{1}-hb-{2}", baojiaCacheKey, i, "key"));
                //            });
                //        }
                //        var chuxianKey = string.Format("{0}-claimdetail", baojiaCacheKey);
                //        tran.QueueCommand(p =>
                //        {
                //            p.Remove(chuxianKey);
                //        });
                //        tran.Commit();
                //    }
                //    catch (Exception ex)
                //    {
                //        tran.Rollback();
                //        logError.Info("清空缓存回滚:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                //    }
                //}
                string baojiaCacheKey = CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey("京FF1234", 102, "18310825788");
                for (int i = 0; i < 12; i++)
                {
                    CacheProvider.Remove(string.Format("{0}-{1}-bj-{2}", baojiaCacheKey, i, "key"));
                    CacheProvider.Remove(string.Format("{0}-{1}-hb-{2}", baojiaCacheKey, i, "key"));
                }
            }
        }
        /// <summary>
        /// 车辆是否已在bx_userinfo中存在
        /// </summary>
        /// <param name="agent">顶级代理人</param>
        /// <param name="licenseno">车牌号</param>
        /// <returns></returns>
        public bx_userinfo IsExistLicense(int agent, string licenseno)
        {
            //取出代理下面所有的经纪人
            var agentLists = _agentRepository.GetSonsList(agent, true);
            //根据经纪人和车牌号取最新的一条数据
            bx_userinfo userinfo = _userInfoRepository.FindAgentListByLicenseNo(licenseno, agentLists);
            return userinfo;
        }
        private int CheckIsPublicField(PostPrecisePriceRequest request, int renewalpublic)
        {
            var isPublic = renewalpublic;
            var isCarOwnerValid = false || !string.IsNullOrWhiteSpace(request.CarOwnersName) && !string.IsNullOrWhiteSpace(request.IdCard) &&
                                      request.OwnerIdCardType > 0;
            var isPosterValid = false ||
                                !string.IsNullOrWhiteSpace(request.HolderName) &&
                                !string.IsNullOrWhiteSpace(request.HolderIdCard) && request.HolderIdType > 0;
            //var isInsuredValid = false || !string.IsNullOrWhiteSpace(request.InsuredName) &&
            //                     !string.IsNullOrWhiteSpace(request.InsuredIdCard) && request.InsuredIdType > 0;
            //if (!isPosterValid && isInsuredValid)
            //{
            //    request.HolderName = request.InsuredName;
            //    request.HolderIdCard = request.HolderIdCard;
            //    request.HolderIdType = request.HolderIdType;
            //    isPosterValid = true;
            //}
            if (isCarOwnerValid && isPosterValid)
            {
                ////家庭自用
                //if (request.IdCard.IsValidIdCard())
                //{
                //    if (request.OwnerIdCardType == 1 && (request.HolderIdType == 2 || request.HolderIdType == 9))
                //    {
                //        isPublic = 2;
                //    }
                //}
                ////兼容港澳通行证
                //if ((request.OwnerIdCardType == 5 || request.OwnerIdCardType == 14) && (request.HolderIdType == 2 || request.HolderIdType == 9))
                //{
                //    isPublic = 2;
                //}

                ////家庭自用
                //if (request.HolderIdCard.IsValidIdCard())
                //{
                //    if ((request.OwnerIdCardType == 2 || request.OwnerIdCardType == 9) && request.HolderIdType == 1)
                //    {
                //        isPublic = 2;
                //    }
                //}
                //if ((request.OwnerIdCardType == 2 || request.OwnerIdCardType == 9) && (request.HolderIdType == 5 || request.HolderIdType == 14))
                //{
                //    isPublic = 2;
                //}
                ////家庭自用
                //if (request.IdCard.IsValidIdCard() && request.HolderIdCard.IsValidIdCard())
                //{
                //    if (request.OwnerIdCardType == 1 && request.HolderIdType == 1)
                //    {
                //        isPublic = 2;
                //    }
                //}
                //if ((request.OwnerIdCardType == 5 || request.OwnerIdCardType == 14) && (request.HolderIdType == 5 || request.HolderIdType == 14))
                //{
                //    isPublic = 2;
                //}
                //if ((request.OwnerIdCardType == 5 || request.OwnerIdCardType == 14) && request.HolderIdType == 1)
                //{
                //    isPublic = 2;
                //}
                //if (request.OwnerIdCardType == 1 && (request.HolderIdType == 5 || request.HolderIdType == 14))
                //{
                //    isPublic = 2;
                //}
                //公户车
                if ((request.OwnerIdCardType == 2 || request.OwnerIdCardType == 9) &&
                    (request.HolderIdType == 2 || request.HolderIdType == 9))
                {
                    isPublic = 1;

                }
                else if ((request.OwnerIdCardType == 2 || request.OwnerIdCardType == 9) &&
                         (request.InsuredIdType == 2 || request.InsuredIdType == 9) &&
                         (new List<int>() { 1, 5, 14 }).Contains(request.HolderIdType))   //投保人是个人
                {
                    isPublic = 1;
                }
                else
                {
                    isPublic = 2;
                }
            }
            return isPublic;
        }
        /// <summary>
        /// 公私车判断，20180514新改，只根据车主判断
        /// </summary>
        /// <param name="request"></param>
        /// <param name="renewalpublic"></param>
        /// <returns></returns>
        private int CheckIsPublicFieldNew(PostPrecisePriceRequest request, int renewalpublic)
        {
            var isPublic = renewalpublic;
            var isCarOwnerValid = false || !string.IsNullOrWhiteSpace(request.CarOwnersName) && !string.IsNullOrWhiteSpace(request.IdCard) &&
                                      request.OwnerIdCardType > 0;
            if (isCarOwnerValid)
            {
                //公户车
                if (request.OwnerIdCardType == 2 || request.OwnerIdCardType == 9)
                {
                    isPublic = 1;
                }
                else
                {
                    isPublic = 2;
                }
            }
            return isPublic;
        }
        private PostPrecisePriceRequest CheckCarUsedType(PostPrecisePriceRequest request, int publictype)
        {
            if (publictype == 1)
            {
                if (request.CarUsedType != 2 && request.CarUsedType != 3)
                {
                    request.CarUsedType = 3;
                }
            }
            else if (publictype == 2)
            {
                request.CarUsedType = 1;
            }
            else
            {
                request.CarUsedType = 1;
            }
            return request;
        }
        private int CheckInput(PostPrecisePriceRequest request, int publicvalue, int carusedtype)
        {
            //1、当使用性质为非营业企业或判断出公车时（CarUsedType==3||IsPublic==1）：
            //前端/接口需要校验：车主/被保险人/投保人必须至少有一个信息完整（姓名、证件类型、证件号码）且为公户，
            //中心处理逻辑：对信息不完整的关系人，默认使用接口提供的完整的公户信息，不再随机；
            //2、当使用性质为家庭自用或判断出个人时（CarUsedType==1||IsPublic==2）：
            //前端/接口需要校验：车主/被保险人/投保人不能都为公户
            var checktype = 1;
            var isCarOwnerValid = false || !string.IsNullOrWhiteSpace(request.CarOwnersName) && !string.IsNullOrWhiteSpace(request.IdCard) &&
                                      request.OwnerIdCardType > 0;
            var isPosterValid = false ||
                                !string.IsNullOrWhiteSpace(request.HolderName) &&
                                !string.IsNullOrWhiteSpace(request.HolderIdCard) && request.HolderIdType > 0;
            var isInsuredValid = false || !string.IsNullOrWhiteSpace(request.InsuredName) &&
                                 !string.IsNullOrWhiteSpace(request.InsuredIdCard) && request.InsuredIdType > 0;
            if (publicvalue == 1 || carusedtype == 3 || carusedtype == 2)
            {
                if (!((isCarOwnerValid && (request.OwnerIdCardType == 2 || request.OwnerIdCardType == 9)) || (isPosterValid && (request.HolderIdType == 2 || request.HolderIdType == 9)) ||
                    (isInsuredValid && (request.InsuredIdType == 2 || request.InsuredIdType == 9))))
                {
                    return 1;//条件1 不符合
                }
            }
            if (publicvalue == 2 || carusedtype == 1)
            {
                if ((request.OwnerIdCardType == 2 || request.OwnerIdCardType == 9) && (request.HolderIdType == 2 || request.HolderIdType == 9) && (request.InsuredIdType == 2 || request.InsuredIdType == 9))
                {
                    return 2;//条件1 不符合
                }
            }
            return 0;
        }
        /// <summary>
        /// 发送分配请求
        /// 20180820.by.gpj修改.调用刘松年新版分配，不用userinfo的moldname了
        /// </summary>
        /// <param name="businessStatus"></param>
        /// <param name="moldName"></param>
        /// <param name="buid"></param>
        /// <param name="reqAgent"></param>
        /// <param name="reqChildAgent">此处无须担心request.ChildAgent=0，在方法里会相关的判断操作</param>
        /// <param name="uiAgent"></param>
        /// <param name="reqCityCode"></param>
        /// <param name="reqLicenseNo"></param>
        /// <param name="reqRenewalType"></param>
        /// <param name="uiRenewalType"></param>
        /// <param name="businessExpireDate"></param>
        /// <param name="forceExpireDate"></param>
        private void SentDistributed(int businessStatus, string moldName, long buid, int reqAgent, int reqChildAgent, string uiAgent, int reqCityCode, string reqLicenseNo, int reqRenewalType, int uiRenewalType, string businessExpireDate, string forceExpireDate, bool needCarMoldFilter, int cameraAgent, string cameraId, bool isAdd, int repeatStatus, int roleType, string custKey, int cityCode)
        {
            try
            {
                if (businessStatus == 1)
                {
                    if (!string.IsNullOrWhiteSpace(businessExpireDate))
                    {
                        var fd = DateTime.Parse(businessExpireDate);
                        if (fd.Date == DateTime.MinValue.Date)
                        {
                            businessExpireDate = string.Empty;
                        }
                        else
                        {
                            businessExpireDate = DateTime.Parse(businessExpireDate).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(forceExpireDate))
                    {
                        var fd = DateTime.Parse(forceExpireDate);
                        if (fd.Date == DateTime.MinValue.Date)
                        {
                            forceExpireDate = string.Empty;
                        }
                        else
                        {
                            forceExpireDate = DateTime.Parse(forceExpireDate).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }

                _filterMoldNameService.FilterMoldName(moldName, reqChildAgent, reqAgent, buid, reqCityCode, businessExpireDate, forceExpireDate, cameraId, isAdd, repeatStatus, roleType, custKey, cityCode);
                //int childagent = reqChildAgent == 0 ? int.Parse(uiAgent) : reqChildAgent;
                //string _url = string.Format("{0}/api/Camera/FilterAndDistribute", _host);
                //string postDataNoSecCode =
                //    string.Format(
                //        "buId={0}&Agent={1}&CityCode={2}&LicenseNo={3}&businessExpireDate={4}&forceExpireDate={5}&childAgent={6}&CameraId{7}&carModelKey={8}&CameraAgent={9}",
                //        buid, reqAgent, reqCityCode, reqLicenseNo, businessExpireDate, forceExpireDate, childagent,cameraId, moldName, cameraAgent);
                //string res = String.Empty;
                //string secCode = postDataNoSecCode.GetMd5();
                //string postData = postDataNoSecCode + "&SecCode=" + secCode;
                ////记录请求
                //logInfo.Info(string.Format("续保调用{0}接口：Url:{1}，请求：{2}", "分配", _url, postData));
                //using (HttpClient client = new HttpClient(new HttpClientHandler()))
                //{
                //    HttpContent content = new StringContent(postData);
                //    MediaTypeHeaderValue typeHeader = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                //    typeHeader.CharSet = "UTF-8";
                //    content.Headers.ContentType = typeHeader;
                //    var response = client.PostAsync(_url, content).Result;
                //    //logInfo.Info(response.ToJson());
                //}
            }
            catch (Exception ex)
            {
                logError.Info("续保请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
        }

        /// <summary>
        /// 更改续保信息状态
        /// 以前调用crm接口的EnterpriseBatchRenewal/UpdateItemStatus
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="status"></param>
        public void UpdateBatchRenewalStatus(long buid, int status)
        {
            int isSuccess = _batchRenewalRepository.UpdateItemStatus(buid, status);
        }

        /// <summary>
        /// 续保调用第三方接口传数据
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="viewModel"></param>
        public void PostThirdPart(int agent, ViewModels.GetReInfoViewModel viewModel)
        {
            //请求第三方Url
            string strUrl = string.Empty;
            //取缓存值
            var cacheKey = string.Format("camera_url_{0}", agent);
            strUrl = CacheProvider.Get<string>(cacheKey);
            if (string.IsNullOrEmpty(strUrl))
            {
                bx_config config = _configRepository.Find(agent.ToString(), 4);
                strUrl = config != null ? config.config_value : "";
                CacheProvider.Set(cacheKey, strUrl, 10800);
            }
            //判断url是否为空
            if (string.IsNullOrEmpty(strUrl))
            {
                //logInfo.Info(string.Format("请求第三方{0}接口无Url", agent));
                return;
            }
            IBxAgent agentModel = GetCommonAgentModelFactory(agent, _agentRepository);
            string secretKey = agentModel == null ? "" : agentModel.SecretKey;
            //执行post请求方法
            Task.Factory.StartNew(() => SendPost(agent, secretKey, strUrl, viewModel));
        }



        public void SendPost(int agent, string secretKey, string strUrl, ViewModels.GetReInfoViewModel viewModel)
        {
            try
            {
                var webClientObj = new WebClient();
                var postVars = new System.Collections.Specialized.NameValueCollection();
                //返回状态内容
                postVars.Add("BusinessStatus", viewModel.BusinessStatus.ToString());
                postVars.Add("StatusMessage", viewModel.StatusMessage);
                //返回UserInfo
                postVars.Add("UserInfo", viewModel.UserInfo.ToJson());
                //返回SaveQuote
                postVars.Add("SaveQuote", viewModel.SaveQuote.ToJson());
                postVars.Add("CustKey", viewModel.CustKey);
                postVars.Add("Agent", agent.ToString());
                string secCode =
                    string.Format("Agent={0}&BusinessStatus={1}&CustKey={2}&SaveQuote={3}&StatusMessage={4}&UserInfo={5}{6}",
                        agent, viewModel.BusinessStatus, viewModel.CustKey, viewModel.SaveQuote.ToJson(), viewModel.StatusMessage, viewModel.UserInfo.ToJson(), secretKey);
                postVars.Add("SecCode", secCode.GetMd5());

                byte[] byRemoteInfo = webClientObj.UploadValues(strUrl, "POST", postVars);
                //返回值
                string remoteInfo = Encoding.UTF8.GetString(byRemoteInfo);
                logInfo.Info(string.Format("请求第三方{0}接口返回消息：{1}", agent, remoteInfo));

                ////post请求
                //if (agent == 79055 || agent == 73943 || agent == 95554)
                //{
                //    byte[] byRemoteInfo = webClientObj.UploadValues(strUrl, "POST", postVars);
                //    //返回值
                //    string remoteInfo = Encoding.UTF8.GetString(byRemoteInfo);
                //    logInfo.Info(string.Format("请求第三方{0}接口返回消息：{1}", agent, remoteInfo));
                //}
                //else
                //{
                //    var postVarsNew = new System.Collections.Specialized.NameValueCollection();
                //    postVarsNew.Add("values", postVarsNew.ToJson());
                //    byte[] byRemoteInfo = webClientObj.UploadValues(strUrl, "POST", postVarsNew);
                //    //返回值
                //    string remoteInfo = Encoding.UTF8.GetString(byRemoteInfo);
                //    logInfo.Info(string.Format("请求第三方{0}接口返回消息：{1}", agent, remoteInfo));
                //}

            }
            catch (Exception ex)
            {
                logError.Error("调用" + agent + "接口传摄像头续保信息接口异常，Url为：" + strUrl + "；\n 异常信息:" + ex.StackTrace + " \n " + ex.Message);
            }
        }

        public async Task<GetRepeatSubmitResponse> GetRepeatSubmitInfo(GetRepeatSubmitRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetRepeatSubmitResponse();
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
            //参数校验
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //微信端逻辑 次级代理
            if (request.ChildAgent > 0)
            {
                request.Agent = request.ChildAgent;
            }

            bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.Agent.ToString(), request.RenewalCarType);
            if (userinfo == null)
            {
                response.Status = HttpStatusCode.NoContent;
                return response;
            }
            var quoteCompany = new Dictionary<int, int>();
            quoteCompany.Add(1, 1);
            quoteCompany.Add(2, 0);
            quoteCompany.Add(4, 2);
            quoteCompany.Add(8, 3);
            quoteCompany.Add(32, 5);
            quoteCompany.Add(16, 4);
            quoteCompany.Add(64, 6);
            quoteCompany.Add(128, 7);
            quoteCompany.Add(256, 8);
            quoteCompany.Add(512, 9);
            quoteCompany.Add(1024, 10);
            quoteCompany.Add(2048, 11);

            //从缓存获取到期日期
            string baojiaCacheKey =
             CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);

            var expireDataCacheKey = string.Format("{0}-lastinfo-repeat-key", baojiaCacheKey);
            var expireDataCacheValue = string.Format("{0}-lastinfo-repeat", baojiaCacheKey);
            var cacheKey = CacheProvider.Get<string>(expireDataCacheKey);
            if (cacheKey == null)
            {
                for (int i = 0; i < 250; i++)
                {

                    cacheKey = CacheProvider.Get<string>(expireDataCacheKey);
                    if (!string.IsNullOrWhiteSpace(cacheKey))
                    {
                        if (cacheKey == "0" || cacheKey == "1" || cacheKey == "2")
                            break;
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
            }
            var lastinfoRecord = CacheProvider.Get<bx_lastinfo>(expireDataCacheValue);
            if (cacheKey == "1")
            {
                int repeattype = 0;
                string bizmsg = "", forcemsg = "", doubmsg = "";
                //整理每一家的到期日期
                foreach (var quote in quoteCompany)
                {
                    if ((userinfo.IsSingleSubmit.Value & quote.Key) == quote.Key)
                    {
                        var result = CacheProvider.Get<bx_submit_info>(string.Format("{0}-{1}-{2}", baojiaCacheKey,
                            quote.Value, "submitinfo"));

                        repeattype = result.is_repeat_submit.Value | repeattype;
                        if (string.IsNullOrEmpty(bizmsg) && result.is_repeat_submit.Value == 2)
                        {
                            bizmsg = result.quote_result;
                        }
                        if (string.IsNullOrEmpty(forcemsg) && result.is_repeat_submit.Value == 1)
                        {
                            forcemsg = result.quote_result;
                        }
                        if (string.IsNullOrEmpty(doubmsg) && result.is_repeat_submit.Value == 3)
                        {
                            doubmsg = result.quote_result;
                            break;
                        }
                    }

                }
                response.RepeatSubmitResult = repeattype;
                response.ForceExpireDate = string.IsNullOrWhiteSpace(lastinfoRecord.last_end_date)
                    ? string.Empty
                    : DateTime.Parse(lastinfoRecord.last_end_date).ToString("yyyy-MM-dd HH:mm:ss");
                response.BusinessExpireDate = string.IsNullOrWhiteSpace(lastinfoRecord.last_business_end_date)
                    ? string.Empty
                    : DateTime.Parse(lastinfoRecord.last_business_end_date).ToString("yyyy-MM-dd HH:mm:ss");

                if (repeattype == 3)
                {
                    response.RepeatSubmitMessage = doubmsg;
                }
                else
                {
                    response.RepeatSubmitMessage = string.Format("{0} \n {1} ", bizmsg, forcemsg);
                }
            }
            else
            {
                response.RepeatSubmitMessage = "没有重复投保";
                response.RepeatSubmitResult = 0;
                response.ForceExpireDate = string.Empty;
                response.BusinessExpireDate = string.Empty;

            }
            return response;
        }

        /// <summary>
        /// 报价请求第三方接口
        /// </summary>
        /// <param name="buid"></param>
        public void PreciseRequestThirdPart(long buid, int agent)
        {
            //修改上传图片状态
            Task.Factory.StartNew(() =>
            {
                string ret = string.Empty;
                int errCode = 0;
                try
                {
                    string url = ConfigurationManager.AppSettings["SystemSelfUrl"];
                    string seccode = string.Format("Buid={0}&Agent={1}", buid, agent);
                    seccode = seccode.GetMd5();
                    HttpWebAsk.HttpGet(string.Format("{0}/api/UploadImg/UpdateImgState?Buid={1}&Agent={2}&SecCode={3}", url, buid, agent, seccode));
                }
                catch (Exception)
                {
                    errCode = -1;
                }
                return errCode;
            });
            //修改上传图片状态
            Task.Factory.StartNew(() =>
            {
                string ret = string.Empty;
                int errCode = 0;
                try
                {
                    string url = ConfigurationManager.AppSettings["SystemSelfUrl"];
                    string seccode = string.Format("Buid={0}&Agent={1}", buid, agent);
                    seccode = seccode.GetMd5();
                    HttpWebAsk.HttpGet(string.Format("{0}/api/UploadImg/UpdateImgState?Buid={1}&Agent={2}&SecCode={3}", url, buid, agent, seccode));
                }
                catch (Exception)
                {
                    errCode = -1;
                }
                return errCode;
            });
        }
    }
}
