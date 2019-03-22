using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class ChargeService:CommonBehaviorService,IChargeService
    {
        private static readonly string _url = System.Configuration.ConfigurationManager.AppSettings["BaoxianCenter"];
        private static readonly string _baobeigscurl = System.Configuration.ConfigurationManager.AppSettings["BaoBeiGscUrl"];
        private static readonly string _baobeizhlhurl = System.Configuration.ConfigurationManager.AppSettings["BaoBeiZhlhUrl"];

        private IChargeRepository _chargeRepository;
        private IChargeHistoryRepository _chargeHistoryRepository;
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        private IAgentRepository _agentRepository;
        private ICacheHelper _cacheHelper;
        private IReportClaimRepository _reportClaimRepository;
        private IHistoryClaimRepository _historyClaimRepository;
        public ChargeService(IChargeRepository repository, IChargeHistoryRepository historyRepository, ICacheHelper cacheHelper, IAgentRepository agentRepository,
            IReportClaimRepository reportClaimRepository,IHistoryClaimRepository historyClaimRepository
            )
            : base(agentRepository, cacheHelper)
        {
            _chargeRepository = repository;
            _chargeHistoryRepository = historyRepository;
            _agentRepository = agentRepository;
            _cacheHelper = cacheHelper;
            _reportClaimRepository = reportClaimRepository;
            _historyClaimRepository = historyClaimRepository;
        }

        public int Add(CreateChargeRequest request)
        {
            int count = 0;
            try
            {
                bx_charge item = new bx_charge()
                {
                    charge_type = request.ChargeType,
                    total_price = request.TotalPrice,
                    unit_price = request.UnitPirce,
                    used_count = 0,
                    agent = request.Agent
                };
                count = _chargeRepository.Add(item);
                logInfo.Info("创建消费用户成功:"+item.ToJson());
            }
            catch (Exception ex)
            {
                logError.Info("创建消费用户发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                
            }
           
            return count;
        }

        public async Task<UpdateChargeResponse> Update(UpdateChargeRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new UpdateChargeResponse();

            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //logInfo.Info("获取到的经纪人信息:"+agentModel.ToJson());
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {

                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            bx_charge charge = _chargeRepository.Find(request.Agent, request.BusyKey);
            if (charge == null)
            {
                response.Status=HttpStatusCode.BadRequest;
                return response;
            }
            try
            {
                //发送请求 checkneed
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_url);
                    var requestmodel = new
                    {
                        LicenseNo = request.LicenseNo,
                        Agent = request.Agent
                    };

                    HttpResponseMessage res = await client.PostAsJsonAsync("api/userinfo/FindCarInfoBy", requestmodel);
                    if (res.IsSuccessStatusCode)
                    {
                        WaFindCarInfoResponse result = await res.Content.ReadAsAsync<WaFindCarInfoResponse>();
                        if (result.ErrCode == 0)
                        {
                            //更新bx_charge

                            if (charge.total_count == charge.used_count)
                            {
                                //余额不足
                                response.Status = HttpStatusCode.OK;
                                response.ErrCode = -2;
                                return response;
                            }

                            charge.used_count += 1;
                            charge.update_time = DateTime.Now;
                            var chargeCount = _chargeRepository.Update(charge);
                            if (chargeCount == 1)
                            {
                                //插入bx_charge_history
                                var history = new bx_charge_history
                                {
                                    charge_id = charge.id,
                                    license_no = request.LicenseNo,
                                    result_status = 1 //成功

                                };
                                _chargeHistoryRepository.Add(history);
                            }

                            response.TotalCount = charge.total_count.HasValue ? charge.total_count.Value : 0;
                            response.UsedCount = charge.used_count.HasValue ? charge.used_count.Value : 0;
                            response.MoldName = result.CarInfo.MoldName;
                            response.CarVin = result.CarInfo.CarVin;
                            response.RegisterDate = result.CarInfo.RegisteDate;
                            response.LicenseNo = result.CarInfo.PlateNo;
                            response.EngineNo = result.CarInfo.EngineNo;
                            response.LicenseOwner = result.CarInfo.LicenseOwner;
                            response.OwnerIdNo = result.CarInfo.OwnerIdNo;
                            response.Status = HttpStatusCode.OK;
                            response.ErrCode = 0;
                            return response;


                        }
                        else
                        {

                            //插入bx_charge_history
                            var history = new bx_charge_history
                            {
                                charge_id = charge.id,
                                license_no = request.LicenseNo,
                                result_status = 0 //失败

                            };
                            _chargeHistoryRepository.Add(history);
                            response.Status = HttpStatusCode.OK;
                            response.ErrCode = -1;
                            return response;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("收费服务-获取车辆信息发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return response;
        }


        public async Task<GetCarClaimResponse> UpdateClaim(UpdateChargeRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetCarClaimResponse();

            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //logInfo.Info("获取到的经纪人信息:"+agentModel.ToJson());
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {

                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            bx_charge charge = _chargeRepository.Find(request.Agent, request.BusyKey);
            if (charge == null)
            {
                response.Status = HttpStatusCode.BadRequest;
                return response;
            }
           
            try
            {
                //发送请求 checkneed
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_url);
                    var requestmodel = new
                    {
                        LicenseNo = request.LicenseNo
                    };
                    string geturl = string.Format("api/claim/getclaimhistory?licenseno={0}", request.LicenseNo);
                    HttpResponseMessage res = await client.GetAsync(geturl);
                    if (res.IsSuccessStatusCode)
                    {
                        WaGetClaimHistoryResponse result = await res.Content.ReadAsAsync<WaGetClaimHistoryResponse>();
                        if (result.ErrCode == 0)
                        {

                            //更新bx_charge

                            if (charge.total_count == charge.used_count)
                            {
                                //余额不足
                                response.Status = HttpStatusCode.OK;
                                response.ErrCode = -2;
                                return response;
                            }

                            charge.used_count += 1;
                            charge.update_time = DateTime.Now;
                            var chargeCount = _chargeRepository.Update(charge);
                            if (chargeCount == 1)
                            {
                                //插入bx_charge_history
                                var history = new bx_charge_history
                                {
                                    charge_id = charge.id,
                                    license_no = request.LicenseNo,
                                    result_status = 1 //成功

                                };
                                _chargeHistoryRepository.Add(history);
                            }
                            response.TotalCount = charge.total_count.HasValue ? charge.total_count.Value : 0;
                            response.UsedCount = charge.used_count.HasValue ? charge.used_count.Value : 0;
                            response.List = result.ClaimList;
                            response.Status = HttpStatusCode.OK;
                            response.ErrCode = 0;
                            return response;


                        }
                        else
                        {

                            //插入bx_charge_history
                            var history = new bx_charge_history
                            {
                                charge_id = charge.id,
                                license_no = request.LicenseNo,
                                result_status = 0 //失败

                            };
                            _chargeHistoryRepository.Add(history);
                            response.Status = HttpStatusCode.OK;
                            response.ErrCode = -1;
                            return response;
                        }
                    }
                    else
                    {
                        logError.Info("收费服务-获取车辆出险信息发生异常:调用接口getclaimhistory失败");
                    }

                }
            }
            catch (Exception ex)
            {

                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("收费服务-获取车辆出险信息发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return response;
        }

        public async Task<GetReportClaimResponse> GetReportClaim(GetReportClaimRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetReportClaimResponse();

            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //logInfo.Info("获取到的经纪人信息:"+agentModel.ToJson());
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            if (agentModel.IsUsed!=1)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {

                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            bool isSucceed = false;
            bx_charge charge = _chargeRepository.Find(request.Agent,request.BusyKey);  //2 :报备类型
            if (charge == null)
            {
                response.Status = HttpStatusCode.BadRequest;
                return response;
            }

            try
            {
                string reportClaimFlagKey = string.Format("{0}_claim_key", request.LicenseNo);
                string reportClaimCacheKey = string.Format("{0}_{1}", request.LicenseNo, "reportClaim");
                string historyContractCacheKey = string.Format("{0}_{1}", request.LicenseNo, "historyContract");
                //CacheProvider.Remove(reportClaimCacheKey);
                //CacheProvider.Remove(historyContractCacheKey);
                //CacheProvider.Remove(reportClaimFlagKey);
                var reportClaimFlagCache = CacheProvider.Get<string>(reportClaimFlagKey);
                if (reportClaimFlagCache!=null)
                {
                    isSucceed = true;
                    var reportClaimCache = CacheProvider.Get<List<bx_report_claim>>(reportClaimCacheKey);
                    var historyContractCache = CacheProvider.Get<List<bx_history_contract>>(historyContractCacheKey);
                    response.ClaimList = reportClaimCache;
                    response.ContractList = historyContractCache;
                }
                else
                {
                    //发送请求 checkneed
                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromMinutes(1);
                        string _choosebaobei = System.Configuration.ConfigurationManager.AppSettings["ChooseBaoBei"];
                        var choseVal = string.IsNullOrWhiteSpace(_choosebaobei) ? 0 : int.Parse(_choosebaobei);
                        var baobeiUrl = string.Empty;
                        string getUrl = string.Empty;
                        if (choseVal == 0)
                        {
                            baobeiUrl = _baobeizhlhurl;
                            getUrl =
                           string.Format(
                               "/api/ZhongHua/GetUnderwriteClaimsInfo?licenceCode={0}&engineNo={1}&carVin={2}&type={3}",
                               request.LicenseNo, request.EngineNo, request.CarVin, request.Type);
                        }
                        else
                        {
                            baobeiUrl = _baobeigscurl;
                            getUrl =
                           string.Format(
                               "/api/GuoShouCai/GetUnderwriteClaimsInfo?licenceCode={0}&engineNo={1}&carVin={2}&type={3}",
                               request.LicenseNo, request.EngineNo, request.CarVin, request.Type);
                        }
                        client.BaseAddress = new Uri(baobeiUrl);
                         //getUrl =
                         //   string.Format(
                         //       "/api/GuoShouCai/GetUnderwriteClaimsInfo?licenceCode={0}&engineNo={1}&carVin={2}&type={3}",
                         //       request.LicenseNo, request.EngineNo, request.CarVin, request.Type);
                         HttpResponseMessage res = await client.GetAsync(getUrl);
                        if (res.IsSuccessStatusCode)
                        {
                            WaGuoShouCaiQuoteResponse result =
                                await res.Content.ReadAsAsync<WaGuoShouCaiQuoteResponse>();
                            if (result.ErrCode == 0)
                            {
                                var reportClaimList = result.GuoShouCaiQuoteResult.ClaimsLinsInfo;
                                var existList = _reportClaimRepository.FindList(request.LicenseNo);
                                List<bx_report_claim> claims = new List<bx_report_claim>();
                                if (reportClaimList.Count != existList.Count)
                                {
                                    if (reportClaimList.Count > 0)
                                    {
                                        _reportClaimRepository.Remove(request.LicenseNo);
                                        claims =
                                            reportClaimList.Select(claimsListInfo => new bx_report_claim
                                            {
                                                AccidentPlace = claimsListInfo.AccidentPlace,
                                                AccidentPsss = claimsListInfo.AccidentPsss,
                                                DriverName = claimsListInfo.DriverName,
                                                IsCommerce = claimsListInfo.IsCommerce,
                                                IsOwners = claimsListInfo.IsOwners,
                                                IsThreecCar = claimsListInfo.IsThreecCar,
                                                LicenseNo = request.LicenseNo,
                                                ReportDate =
                                                    string.IsNullOrWhiteSpace(claimsListInfo.ReportDate)
                                                        ? DateTime.MinValue
                                                        : DateTime.Parse(claimsListInfo.ReportDate),
                                            }).ToList();
                                        _reportClaimRepository.Add(claims);
                                    }

                                }
                                else
                                {
                                    claims = existList;
                                }
                                //放入缓存
                                CacheProvider.Set(reportClaimCacheKey, claims, 86400);
                                response.ClaimList = claims;

                                var historyList = result.GuoShouCaiQuoteResult.HistoryContractInfo;
                                var existHistoryList = _historyClaimRepository.FindList(request.LicenseNo);
                                List<bx_history_contract> historyContracts = new List<bx_history_contract>();
                                if (historyList!=null&&existHistoryList.Count != historyList.Count)
                                {
                                    if (historyList.Count > 0)
                                    {
                                        _historyClaimRepository.Remove(request.LicenseNo);
                                        historyContracts = historyList.Select(info => new bx_history_contract
                                        {
                                            Enddate = DateTime.Parse(info.Enddate),
                                            InsureCompanyName = info.InsureCompanyName,
                                            IsCommerce = info.IsCommerce,
                                            LicenseNo = request.LicenseNo,
                                            PolicyNo = info.PolicyNo,
                                            Strdate = DateTime.Parse(info.Strdate),
                                        }).ToList();
                                        _historyClaimRepository.Add(historyContracts);

                                    }

                                }
                                else
                                {
                                    historyContracts = existHistoryList;
                                }
                                //放入缓存
                                CacheProvider.Set(historyContractCacheKey, historyContracts, 86400);
                                response.ContractList = historyContracts;


                                CacheProvider.Set(reportClaimFlagKey, 1, 86000);
                                isSucceed = true;
                                //#region

                                //////更新bx_charge
                                //if (charge.total_count == charge.used_count)
                                //{
                                //    //余额不足
                                //    response.Status = HttpStatusCode.OK;
                                //    response.ErrCode = -2;
                                //    response.ClaimList = new List<bx_report_claim>();
                                //    response.ContractList = new List<bx_history_contract>();
                                //    return response;
                                //}

                                //charge.used_count += 1;
                                //charge.update_time = DateTime.Now;
                                //var chargeCount = _chargeRepository.Update(charge);
                                //if (chargeCount == 1)
                                //{
                                //    //插入bx_charge_history
                                //    var history = new bx_charge_history
                                //    {
                                //        charge_id = charge.id,
                                //        license_no = request.LicenseNo,
                                //        result_status = 1 //成功

                                //    };
                                //    _chargeHistoryRepository.Add(history);
                                //}
                                //response.TotalCount = charge.total_count.HasValue ? charge.total_count.Value : 0;
                                //response.UsedCount = charge.used_count.HasValue ? charge.used_count.Value : 0;
                                ////response.List = result.ClaimList;
                                //response.Status = HttpStatusCode.OK;
                                //response.ErrCode = 0;
                                //return response;

                                //#endregion

                            }
                            else
                            {
                                CacheProvider.Remove(reportClaimFlagKey);
                                //插入bx_charge_history
                                var history = new bx_charge_history
                                {
                                    charge_id = charge.id,
                                    license_no = request.LicenseNo,
                                    result_status = 0 //失败

                                };
                                _chargeHistoryRepository.Add(history);
                                response.Status = HttpStatusCode.OK;
                                response.ErrCode = result.ErrCode; //-2:查询车辆信息失败    -1202：请提供正确的车架号和发动机号 -3:获取历史承保信息信息失败  -4:获取理赔信息失败
                                return response;
                            }
                            
                        }
                        else
                        {
                            logError.Info("收费服务-获取车辆出险信息发生异常:调用接口getclaimhistory失败");
                        }

                    }
                }
                if (isSucceed)
                {
                    //更新bx_charge
                    if (charge.total_count <= charge.used_count)
                    {
                        //余额不足
                        response.Status = HttpStatusCode.OK;
                        response.ClaimList = new List<bx_report_claim>();
                        response.ContractList = new List<bx_history_contract>();
                        response.TotalCount = charge.total_count.HasValue ? charge.total_count.Value : 0;
                        response.UsedCount = charge.used_count.HasValue ? charge.used_count.Value : 0;
                        response.ErrCode = -5;
                        return response;
                    }

                    charge.used_count += 1;
                    charge.update_time = DateTime.Now;
                    var chargeCount = _chargeRepository.Update(charge);
                    if (chargeCount == 1)
                    {
                        //插入bx_charge_history
                        var history = new bx_charge_history
                        {
                            charge_id = charge.id,
                            license_no = request.LicenseNo,
                            result_status = 1 //成功

                        };
                        _chargeHistoryRepository.Add(history);
                    }
                    response.TotalCount = charge.total_count.HasValue ? charge.total_count.Value : 0;
                    response.UsedCount = charge.used_count.HasValue ? charge.used_count.Value : 0;
                    //response.List = result.ClaimList;
                    response.Status = HttpStatusCode.OK;
                    response.ErrCode = 0;
                }
                else
                {
                    CacheProvider.Remove(reportClaimFlagKey);
                }
                return response;

            }
            catch (TaskCanceledException ex)
            {
                response.Status = HttpStatusCode.RequestTimeout;
            }
            catch (Exception ex)
            {

                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("收费服务-获取车辆出险信息发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return response;
        }
    }
}
