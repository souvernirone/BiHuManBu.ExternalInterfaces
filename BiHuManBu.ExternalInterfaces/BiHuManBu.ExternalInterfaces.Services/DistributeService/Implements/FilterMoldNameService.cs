using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.Distribute.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.DistributeService.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure;
namespace BiHuManBu.ExternalInterfaces.Services.DistributeService.Implements
{
    public class FilterMoldNameService : IFilterMoldNameService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IBatchRenewalRepository _batchRenewalRepository;
        private readonly ICameraDistributeRepository _cameraDistributeRespository;
        private readonly ICameraDistributeService _cameraDistributeService;
        private readonly IUserinfoExpandRepository _userinfoExpandRepository;
        private readonly IReviewDetailRecordReponsitory _reviewDetailRecordReponsitory;
        private readonly string _cistributeInterceptIds;
        private readonly IFiterAndRepeatDataService _fiterAndRepeatDataService;
        private ILog logError;
        private ILog logInfo;
        public FilterMoldNameService(IUserInfoRepository userInfoRepository, IBatchRenewalRepository batchRenewalRepository, ICameraDistributeService cameraDistributeService, IUserinfoExpandRepository userinfoExpandRepository, ICameraDistributeRepository cameraDistributeRespository, IReviewDetailRecordReponsitory reviewDetailRecordReponsitory, IFiterAndRepeatDataService fiterAndRepeatDataService)
        {
            _cameraDistributeRespository = cameraDistributeRespository;
            _userinfoExpandRepository = userinfoExpandRepository;
            _userInfoRepository = userInfoRepository;
            _batchRenewalRepository = batchRenewalRepository;
            _cameraDistributeService = cameraDistributeService;
            _reviewDetailRecordReponsitory = reviewDetailRecordReponsitory;
            this._cistributeInterceptIds = GetAppSettings("DistributeInterceptIds");
            this._fiterAndRepeatDataService = fiterAndRepeatDataService;
            logError = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");
        }
        public string GetAppSettings(string key)
        {
            var val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val))
                return "";
            return val;
        }
        /// <summary>
        /// 车型过滤+消息发送
        /// </summary>
        /// <param name="moldName">品牌型号</param>
        /// <param name="cameraAgent">摄像头代理人Id</param>
        /// <param name="agent">摄像头顶级代理人Id</param>
        /// <param name="buid">userinfo.id</param>
        /// <param name="citycode">城市代码</param>
        /// <param name="businessExpireDate">商业险到期时间</param>
        /// <param name="forceExpireDate">交强险到期时间</param>
        public void FilterMoldName(string moldName, int cameraAgent, int agent, long buid, int citycode, string businessExpireDate, string forceExpireDate, string cameraId, bool isAdd, int repeatStatus, int roleType, string custKey, int cityCode)
        {
            try
            {
                bx_userinfo userInfo = _userInfoRepository.FindByBuidSync(buid);
                #region  如果是新增数据，进行车架号查重
                if (isAdd == true)
                {
                    long uid = _fiterAndRepeatDataService.GetFiterDataByCarVin(agent, cameraAgent, repeatStatus, roleType, userInfo.CarVIN, cameraId, custKey, cityCode);
                    if (uid != 0)
                    {
                        return;
                    }
                }
                #endregion

                bx_batchrenewal_item renewalItem = _batchRenewalRepository.GetItemByBuIdSync(buid);
                #region 根据批续重新赋值车型和商业交强到期时间
                if (renewalItem != null && (renewalItem.LastYearSource != -1 || renewalItem.ForceEndDate.HasValue || renewalItem.BizEndDate.HasValue))
                {
                    if (userInfo.LastYearSource > -1 && userInfo.NeedEngineNo == 0)
                    {
                        if (!string.IsNullOrWhiteSpace(businessExpireDate) || renewalItem.BizEndDate.Value.ToString("yyyy-MM-dd") != "1900-01-01")
                        {

                            if (renewalItem.BizEndDate.Value.ToString("yyyy-MM-dd") != "1900-01-01" && ((!string.IsNullOrWhiteSpace(businessExpireDate) && DateTime.Parse(businessExpireDate).Year < renewalItem.BizEndDate.Value.Year) || string.IsNullOrWhiteSpace(businessExpireDate)))
                            {
                                moldName = renewalItem.MoldName;
                                businessExpireDate = renewalItem.BizEndDate.Value.ToString("yyyy-MM-dd HH:mm");
                                forceExpireDate = renewalItem.ForceEndDate.Value.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : renewalItem.ForceEndDate.Value.ToString("yyyy-MM-dd HH:mm");
                                if (!string.IsNullOrWhiteSpace(renewalItem.MoldName))
                                {
                                    moldName = renewalItem.MoldName;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(renewalItem.MoldName) || !string.IsNullOrWhiteSpace(renewalItem.RegisterDate))
                        {
                            moldName = renewalItem.MoldName;
                        }

                        businessExpireDate = renewalItem.BizEndDate.Value.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : renewalItem.BizEndDate.Value.ToString("yyyy-MM-dd HH:mm");
                        forceExpireDate = renewalItem.ForceEndDate.Value.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : renewalItem.ForceEndDate.Value.ToString("yyyy-MM-dd HH:mm");
                    }
                }
                #endregion
                #region   增加userinfo_expand
                _userinfoExpandRepository.Add(userInfo.Id);
                #endregion
                int carModelId = 0;
                //是否走车型过滤
                var needFilter = false;
                //老逻辑 如果请求过来的顶级在配置的顶级之内，老数据不走车型过滤
                if (("," + _cistributeInterceptIds + ",").Contains("," + agent + ","))
                {
                    // 如果是新创建的记录，需要走车型过滤。
                    if (isAdd)
                    {
                        needFilter = true;
                    }
                }
                else
                {
                    needFilter = true;
                }
                if (needFilter)
                {
                    logInfo.Info("走车型过滤：" + buid + ",判断是否走车型过滤状态为：" + needFilter);
                    if (!string.IsNullOrEmpty(moldName))
                    {
                        logInfo.Info("走车型过滤：" + buid);
                        carModelId = _cameraDistributeService.GetModelFilterId(cameraAgent, moldName);
                    }
                    else
                    {
                        logInfo.Info(string.Format("摄像头车型过滤：" + buid + ", 没有接受到carModelKey参数"));
                        carModelId = 0;
                    }
                }
                if (carModelId < 0)
                {
                    if (!isAdd)
                    {
                        //老数据不过滤，直接return相当于没发生过。
                        return;
                    }
                    //新数据过滤
                    userInfo.IsTest = 3;
                    _userinfoExpandRepository.UpdateUserExpandByBuid(userInfo.Id.ToString(), 2, DateTime.Now);
                    _reviewDetailRecordReponsitory.Del(userInfo.Id);

                }
                else
                {
                    userInfo.CarMoldId = (carModelId == 0 ? -1 : carModelId);
                }
                _userinfoExpandRepository.UpdateCameraTimeByBuid(userInfo.Id.ToString(), DateTime.Now, cameraId);

                int endDays;
                int isRemind;
                bool intime = _cameraDistributeService.IsInTime(citycode, businessExpireDate, forceExpireDate, cameraAgent, out endDays, out isRemind);

                if ((userInfo.IsDistributed == 0 || (userInfo.IsDistributed == 2 && _cameraDistributeService.IsAdmin(int.Parse(userInfo.Agent))) || isAdd) && userInfo.IsTest != 3 && intime && endDays >= 0)
                {
                    var distributedAgentId = _cameraDistributeService.GerRedirsSealman(cameraAgent);
                    //判断记录是否分配，如果未分配则赋值分配人
                    if (distributedAgentId > 0)
                    {
                        userInfo.Agent = distributedAgentId.ToString();
                        userInfo.OpenId = distributedAgentId.ToString().GetMd5();
                        userInfo.IsDistributed = 3;
                        userInfo.DistributedTime = DateTime.Now;
                        //增加分配历史记录
                        Task<int> resultNum2 = _cameraDistributeRespository.AddDistributedHistoryAsync(new bx_distributed_history
                        {
                            b_uid = userInfo.Id,
                            batch_id = 0,
                            now_agent_id = (int)distributedAgentId,
                            operate_agent_id = cameraAgent,
                            top_agent_id = agent,
                            type_id = 2,
                            create_time = DateTime.Now
                        });
                    }
                }
                userInfo.UpdateTime = DateTime.Now;
                userInfo.IsCamera = true;
                userInfo.CameraTime = DateTime.Now;
                logInfo.Info("执行更新操作的时候用户的状态：" + userInfo.IsTest + ",buid=" + userInfo.Id);
                _userInfoRepository.UpdateSync(userInfo);
                //添加跟进记录（如果被过滤也会在表中增加记录）
                Task<int> resultNum = _cameraDistributeRespository.AddCrmStepsAsync(new bx_crm_steps { agent_id = int.Parse(userInfo.Agent), b_uid = buid, create_time = DateTime.Now, json_content = "{\"camertime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}", type = 6 });
                if (userInfo.IsTest == 3)
                {
                    return;
                }
                #region 判断是否在续保期,如果在续保期则推送
                if (intime)
                {
                    //到期范围内，进行提醒
                    _cameraDistributeService.AddNoticexb(userInfo, cameraAgent, isRemind, endDays);
                }
                #endregion
            }
            catch (Exception ex)
            {
                logError.Info("摄像头过滤分配提醒异常" + "\n 异常信息:" + ex.StackTrace + " \n " + ex.Message + "\n BUID" + buid.ToString() + "\n");
            }
        }

    }
}
