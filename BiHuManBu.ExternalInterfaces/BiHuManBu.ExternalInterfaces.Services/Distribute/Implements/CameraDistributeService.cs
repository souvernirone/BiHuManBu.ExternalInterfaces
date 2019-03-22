using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.Distribute.Interfaces;
using log4net;
using ServiceStack.Text;
using BiHuManBu.ExternalInterfaces.Models.DistributeModels;
using System.Configuration;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;

namespace BiHuManBu.ExternalInterfaces.Services.Distribute.Implements
{
    public class CameraDistributeService : ICameraDistributeService
    {
        private readonly string _crmCenterHost;
        private readonly string _cistributeInterceptIds;
        private readonly ILog logInfo;
        private readonly ILog logError;
        ICameraDistributeRepository _cameraDistributeRespository;
        private ICityQuoteDayRepository _cityQuoteDayRepository;
        ICameraConfigRepository _cameraConfigRepositoty;
        private readonly string _messageCenterHost;
        private readonly string _cameraCarModelsKey;
        private IHashOperator _hashOperator;
        private readonly string _sealmanIndexRedirs;
        private readonly string _sealmanRedirs;
        private readonly string _sealmanLeaveRedirs;
        private readonly IAgentRepository _agentRepository; 
        private readonly IUserinfoRenewalInfoRepository _userinfoRenewalInfoRepository;
        private readonly ICustomerCategoriesRepository _customerCategoriesRepository;
        public CameraDistributeService(ICameraDistributeRepository cameraDistributeRespository, ICityQuoteDayRepository cityQuoteDayRepository, IHashOperator hashOperator, ICameraConfigRepository cameraConfigRepositoty, IAgentRepository agentRepository, IUserinfoRenewalInfoRepository userinfoRenewalInfoRepository, ICustomerCategoriesRepository customerCategoriesRepository)
        {
            _cameraConfigRepositoty = cameraConfigRepositoty;
            _hashOperator = hashOperator;
            _cityQuoteDayRepository = cityQuoteDayRepository;
            _agentRepository = agentRepository;
            //发送通知地址
            this._messageCenterHost = GetAppSettings("SendMessage");
            _cameraDistributeRespository = cameraDistributeRespository;
            this._cistributeInterceptIds = GetAppSettings("DistributeInterceptIds");
            this._crmCenterHost = GetAppSettings("SystemCrmUrl");
            this._cameraCarModelsKey = GetAppSettings("CameraCarModelsKey");
            this._sealmanIndexRedirs = GetAppSettings("sealmanIndexRedirs");
            this._sealmanRedirs = GetAppSettings("sealmanRedirs");
            this._sealmanLeaveRedirs = GetAppSettings("sealmanLeaveRedirs");
            logInfo = LogManager.GetLogger("INFO");
            logError = LogManager.GetLogger("ERROR");
            _userinfoRenewalInfoRepository = userinfoRenewalInfoRepository;
            _customerCategoriesRepository = customerCategoriesRepository;
        }
        public string GetAppSettings(string key)
        {
            var val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val))
                return "";
            return val;
        }
        public void Distribute(CameraDistributeModel request)
        {
            logInfo.Info("摄像头自动分配请求串参数：" + request.ToJson());
            try
            {
                //看下非顶级是否绑定了摄像头
                bool hasCamera = HasCamera(request.childAgent);
                //取代理人，如果绑定了，直接取childagent；没绑定，取agent
                int agentId = hasCamera ? request.childAgent : request.Agent;
                #region 新增逻辑 角色管理需求修改 /20171214 gpj
                //取当前代理人的角色  1是否顶级+管理 2是否顶级 3是否管理
                Tuple<bool, bool, bool> isAdmin;
                if (request.Agent == request.childAgent)
                {
                    isAdmin = new Tuple<bool, bool, bool>(true, true, false);
                }
                else
                {
                    bool tmpAdmin = IsAdmin(request.childAgent);
                    isAdmin = new Tuple<bool, bool, bool>(tmpAdmin, false, tmpAdmin);
                }
                #endregion
                //如果子集是顶级或者管理员，则改为取顶级去重//暂时先去掉，后期再加
                agentId = isAdmin.Item1 ? request.Agent : agentId;
                //检查操作是否成功
                bool isTrue;
                var isSuccess = true;
                bool carMoldTmp = true;
                var cameraBlack = GetCameraBlack(request.Agent, request.childAgent, request.licenseNo);
                if (cameraBlack != null && cameraBlack.Id > 0)
                {
                    //取该代理人下黑名单的车牌
                    var repeatList = GetUserinfoByLicenseAndAgent(request.buId, agentId, request.licenseNo);
                    var repeatId = repeatList.Select(l => l.Id).ToList();
                    //将全部黑名单的车牌删掉
                    var str = RemoveList(repeatId, 3, ref isSuccess);
                    return;
                }

                var ListUserinfo = GetUserinfoByLicenseAndAgent(request.buId, agentId, request.licenseNo);

                var userinfo = ListUserinfo.FirstOrDefault();
                //当 重复数据=1  走车型过滤  反之 重复数据 >1 肯定有老数据，不走车型过滤。
                var cistributeIntercept = false;
                if (("," + _cistributeInterceptIds + ",").Contains("," + request.Agent + ","))
                {
                    if (ListUserinfo.Count == 1)
                    {
                        // createtime是否在5分钟之内，说明肯定是新创建的记录，需要走车型过滤。
                        if (userinfo != null && userinfo.CreateTime.HasValue && userinfo.CreateTime.Value.AddMinutes(5) > DateTime.Now)
                        {
                            cistributeIntercept = true;
                        }
                    }
                }
                else
                {
                    cistributeIntercept = true;
                }
                if (cistributeIntercept)
                {
                    logInfo.Info("走车型过滤：" + request.buId + ",判断是否走车型过滤状态为：" + cistributeIntercept);
                    //设置车型
                    if (!string.IsNullOrEmpty(request.carModelKey) && request.reqRenewalType == 3)
                    {
                        logInfo.Info("走车型过滤：" + request.buId);
                        carMoldTmp = SetCarModlId(agentId, request.buId, request.carModelKey);
                        isSuccess = carMoldTmp;
                    }
                    else
                    {
                        logInfo.Info(string.Format("摄像头车型过滤 没有接受到carModelKey参数"));
                        carMoldTmp = true;
                        isSuccess = true;
                    }
                }
                logInfo.Info("分配carMoldTmp：" + carMoldTmp.ToString() + ",isSuccess =" + isSuccess.ToString() + ",isAdmin=" + isAdmin + ",agentId=" + agentId);
                //分配方法 isSuccess=true 设置成功 isSuccess=false 放入回收站，不在分配
                var result = DistributeAndSendMsg(request, carMoldTmp, agentId, isAdmin, ref isSuccess, ListUserinfo);
            }
            catch (Exception e)
            {
                logInfo.Error("摄像头>>>自动分配：", e);

            }
        }
        public bool HasCamera(int childagent)
        {
            bx_camera_config model = _cameraDistributeRespository.GetCameraConfig(childagent);
            return model != null;
        }
        public bool IsAdmin(int agentId)
        {
            var result = false;
            var roleTyoe = _cameraDistributeRespository.GetRoleTypeByAgentId(agentId);
            if (roleTyoe == 3 || roleTyoe == 4)
                result = true;
            return result;
        }
        public bx_camera_blacklist GetCameraBlack(int agent, int childAgent, string LicenseNo)
        {
            return _cameraDistributeRespository.GetCameraBlack(agent, childAgent, LicenseNo);
        }
        List<bx_userinfo> GetUserinfoByLicenseAndAgent(long buid, int agent, string licenseno)
        {
            //取出代理下面所有的经纪人
            var agentLists = GetSonsListFromRedisToString(agent);
            //_agentRepository.GetSonsList(agent);
            //根据经纪人和车牌号取最新的一条数据
            List<bx_userinfo> listuserinfo = _cameraDistributeRespository.GetUserinfoByLicenseAndAgent(buid, licenseno, agentLists);
            return listuserinfo;
        }
        public List<int> GetSonsListFromRedis(int currentAgentId, bool isHasSelf = true)
        {//redis 做了修改
            List<int> agentIds = new List<int>();
            if (ConfigurationManager.AppSettings["IsGetFromRedis"].ToString() != "1")
            {
                agentIds = _cameraDistributeRespository.GetSonListByDb(currentAgentId, isHasSelf);
            }
            else
            {
                agentIds = AddSpecifiedAgentGroupToRedis(new List<int> { currentAgentId }, () =>
                {
                    return _hashOperator.Get<List<int>>("agentGroupKey", currentAgentId.ToString());
                });
                if (!isHasSelf)
                {
                    agentIds.Remove(currentAgentId);
                }

            }
            return agentIds;
        }
        public List<int> GetSonsListFromRedisAsync(int currentAgentId, bool isHasSelf = true)
        {//redis 做了修改
            List<int> agentIds = new List<int>();
            if (ConfigurationManager.AppSettings["IsGetFromRedis"].ToString() != "1")
            {
                agentIds = _cameraDistributeRespository.GetSonListByDbAsync(currentAgentId, isHasSelf);
            }
            else
            {
                agentIds = AddSpecifiedAgentGroupToRedis(new List<int> { currentAgentId }, () =>
                {
                    return _hashOperator.Get<List<int>>("agentGroupKey", currentAgentId.ToString());
                });
                if (!isHasSelf)
                {
                    agentIds.Remove(currentAgentId);
                }

            }
            return agentIds;
        }
        private List<int> AddSpecifiedAgentGroupToRedis(List<int> currentAgentIds, Func<List<int>> GetAgentGroupFunc)
        {
            List<int> agentIds = new List<int>();
            foreach (var item in currentAgentIds)
            {
                if (_hashOperator.Get<List<int>>("agentGroupKey", item.ToString()) == null || !_hashOperator.Get<List<int>>("agentGroupKey", item.ToString()).Any())
                {
                    agentIds = _cameraDistributeRespository.GetSonListByDb(item);

                    _hashOperator.Set("agentGroupKey", item.ToString(), agentIds);
                }
                else
                {
                    agentIds = GetAgentGroupFunc();

                }
            }
            return agentIds;
        }
        /// <summary>
        /// 将GetSonsListFromRedis返回的int转换成string
        /// </summary>
        /// <param name="currentAgentId"></param>
        /// <param name="isHasSelf"></param>
        /// <returns></returns>
        public List<string> GetSonsListFromRedisToString(int currentAgentId, bool isHasSelf = true)
        {
            return GetSonsListFromRedis(currentAgentId, isHasSelf).Select(o => o.ToString()).ToList();
        }
        public List<string> GetSonsListFromRedisToStringAsync(int currentAgentId, bool isHasSelf = true)
        {
            return GetSonsListFromRedisAsync(currentAgentId, isHasSelf).Select(o => o.ToString()).ToList();
        }
        public string RemoveList(List<long> userList, int isTest, ref bool isSuccess)
        {
            string strList = string.Empty;
            if (userList.Any())
            {
                strList = String.Join(",", userList);
            }
            if (string.IsNullOrEmpty(strList))
            {
                return string.Empty;
            }
            return _cameraDistributeRespository.RemoveList(strList, isTest, ref isSuccess);
        }
        /// <summary>
        /// 设置车型 过滤值
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="userId"></param>
        /// <param name="carModelKey"></param>
        /// <returns></returns>
        public bool SetCarModlId(int agentId, int userId, string carModelKey)
        {//redis做了修改
            var data = _hashOperator.Get<List<carMold>>(_cameraCarModelsKey, agentId.ToString());
            if (data == null)
            {
                data = FindCarType(agentId);
                if (data != null && data.Count > 0)
                {
                    _hashOperator.Set(_cameraCarModelsKey, agentId.ToString(), data);
                }
            }
            var isSuccess = false;
            //该顶级代理人没有设置摄像头过滤关键词，该车的状态是有效的
            if (data != null && data.Any())
            {
                var carModelId = -1;
                foreach (var item in data)
                {
                    //是否包含该数据
                    if (carModelKey.IndexOf(item.name) > -1)
                    {
                        carModelId = item.id;
                        break;
                    }
                }
                //已设置了过滤关键词且不在之内，该车无效被过滤掉放入回收站
                if (carModelId < 0)
                {
                    Remove(userId, 3, ref isSuccess);
                    return false;
                }
                _cameraDistributeRespository.SetCarModelId(userId, carModelId);
            }
            isSuccess = RevokeUserInfo(userId);//_cameraRepository.RevokeFiles(userId);
            return true;
        }
        public List<carMold> FindCarType(int agentId)
        {
            if (agentId > 0)
                return _cameraDistributeRespository.FindCarModel(agentId);
            return _cameraDistributeRespository.FindCarModel(0);
        }
        public string Remove(int userId, int isTest, ref bool isSuccess)
        {
            return _cameraDistributeRespository.Remove(userId, isTest, ref isSuccess);
        }
        private bool RevokeUserInfo(int userId)
        {
            //调用陈亮之前写的撤销方法，并做拆分，将批续模块单独拆出来
            bool reuserinfo = _cameraDistributeRespository.RevokeFiles(userId);
            if (reuserinfo)
            {
                //执行成功之后，调用批续模块的恢复
                _cameraDistributeRespository.RevertBatchRenewalItem(userId);
                //将bx_userinfo_expand表中删除时间和删除类型恢复默认
                _cameraDistributeRespository.UpdateUserExpandByBuid(userId.ToString(), -1, DateTime.Parse("1970-01-01"));
            }
            return reuserinfo;
        }
        #region  主方法
        /// <summary>
        /// 分配人员及发送通知
        /// </summary>
        /// <param name="request"></param>
        /// <param name="carMoldTmp"></param>
        /// <param name="agentId"></param>
        /// <param name="isTopAgent"></param>
        /// <param name="isSuccess"></param>
        /// <returns></returns>
        public string DistributeAndSendMsg(CameraDistributeModel request, bool carMoldTmp, int agentId, Tuple<bool, bool, bool> isTopAgent, ref bool isSuccess, List<bx_userinfo> ListexistUserInfo)
        {
            var result = "操作成功!";
            try
            {
                string businessExpireDate = request.businessExpireDate;//商业险到期时间初始化
                string forceExpireDate = request.forceExpireDate;//交强险到期时间初始化
                var deleteAgentId = ConfigurationManager.AppSettings["crmMultipleDelete"];//crm删除记录放在哪个代理下
                int msgIsDistributed = 0;//消息用_分配状态
                bx_userinfo existUserInfo = null;//老数据，执行分配操作
                bool isExitUserinfo = false;//是否存在userifo
                bool isDistributedUserInfo = false;//userinfo是否已分配
                int endDays;//原交强险到期天数,现改为商业和交强判断
                long distributedAgentId = 0;//分配代理人
                //是否在到期时间范围内
                bool intime = IsInTime(request.cityCode, businessExpireDate, forceExpireDate, out endDays);

                //carMoldTmp为true符合进店车型标准的车辆，不符合的直接改为false
                if (request.reqRenewalType == 3)
                {
                    existUserInfo = ListexistUserInfo.Where(x => x.Id != Convert.ToInt64(request.buId)).OrderByDescending(x => x.UpdateTime).FirstOrDefault();
                    //如果已存在
                    if (existUserInfo != null && existUserInfo.Id > 0)
                    {
                        logInfo.Info(string.Format("buid为：{0}，已存在执行分配", request.buId));
                        isExitUserinfo = true;
                        //第一步，删除新数据
                        if (ListexistUserInfo.Count > 1)
                        {
                            _cameraDistributeRespository.DeleteUserinfo(request.buId, deleteAgentId);
                        }
                        //第二步，修改老数据字段
                        //车型判断：是不是期望车型
                        if (!carMoldTmp)
                        {
                            logInfo.Info(string.Format("执行分配", request.buId));
                            //模型匹配失败，放入回收站
                            existUserInfo.IsTest = 3;
                        }
                        //判断记录未分配，需要分配人
                        if (existUserInfo.IsDistributed == 0 && existUserInfo.IsTest != 3)
                        {
                            if (intime)
                            {
                                //到期范围内，取自动分配的业务员
                                distributedAgentId = GerRedirsSealman(agentId);
                            }
                            if (distributedAgentId > 0)
                            {
                                isDistributedUserInfo = true;
                                existUserInfo.Agent = distributedAgentId.ToString();
                                existUserInfo.OpenId = distributedAgentId.ToString().GetMd5();
                                existUserInfo.IsDistributed = 3;
                            }
                        }
                        existUserInfo.UpdateTime = DateTime.Now;
                        existUserInfo.IsCamera = true;
                        existUserInfo.CameraTime = DateTime.Now;
                        logInfo.Info("执行更新操作的时候用户的状态：" + existUserInfo.IsTest + ",buid=" + existUserInfo.Id);
                        var a = _cameraDistributeRespository.Update(existUserInfo);
                        //重置传入的参数
                        request.buId = (int)existUserInfo.Id;
                        request.uiRenewalType = existUserInfo.RenewalType.Value;
                        request.childAgent = int.Parse(existUserInfo.Agent);
                        //request.cityCode = string.IsNullOrEmpty(existUserInfo.CityCode) ? 1 : int.Parse(existUserInfo.CityCode);
                        request.uiCustKey = existUserInfo.OpenId;
                        msgIsDistributed = existUserInfo.IsDistributed;
                        #region 重写到期时间
                        //判断修改到期时间；
                        //上文已经将续保回来的2个时间赋值给变量，此处要做的就是判断是否需要修改这2个值
                        var renewalItem = _cameraDistributeRespository.GetItemByBuId(existUserInfo.Id);//获取批续的续保对象
                        //判断批续是否为空
                        if (renewalItem != null && renewalItem.BizEndDate.HasValue && !string.IsNullOrWhiteSpace(businessExpireDate))
                        {
                            //批续的年份>续保年份，返回批续的时间
                            if (renewalItem.BizEndDate.Value.Year != 1900 &&
                                (DateTime.Parse(businessExpireDate).Year < renewalItem.BizEndDate.Value.Year ||
                                 string.IsNullOrEmpty(businessExpireDate)))
                            {
                                businessExpireDate = renewalItem.BizEndDate.Value.ToString("yyyy-MM-dd");
                                forceExpireDate = renewalItem.ForceEndDate.HasValue ? (renewalItem.ForceEndDate.Value.ToString("yyyy-MM-dd") == "1900-01-01" ? "" : renewalItem.ForceEndDate.Value.ToString("yyyy-MM-dd")) : "";
                            }
                            intime = IsInTime(request.cityCode, businessExpireDate, forceExpireDate, out endDays);
                        }
                        #endregion
                    }
                    else
                    {
                        logInfo.Info(string.Format("buid为：{0}，未重复执行分配", request.buId));
                        //有分配的人
                        bx_userinfo noexistUserInfo = _cameraDistributeRespository.GetUserInfo(request.buId);
                        if (!carMoldTmp)
                        {
                            noexistUserInfo.IsTest = 3;
                        }
                        else
                        {
                            if (intime)
                            {
                                //到期范围内，取自动分配的业务员
                                distributedAgentId = GerRedirsSealman(agentId);
                            }
                            if (distributedAgentId > 0)
                            {
                                logInfo.Info(string.Format("buid为：{0}，分配的代理人Id是：{1}", request.buId, distributedAgentId));
                                isDistributedUserInfo = true;
                                noexistUserInfo.Agent = distributedAgentId.ToString();
                                noexistUserInfo.OpenId = distributedAgentId.ToString().ToMd5();
                                noexistUserInfo.IsCamera = true;
                                noexistUserInfo.CameraTime = DateTime.Now;
                                noexistUserInfo.IsDistributed = 3;
                                //添加回收数据
                                Task<int> resultNum2 = _cameraDistributeRespository.AddDistributedHistoryAsync(new bx_distributed_history
                                {
                                    b_uid = request.buId,
                                    batch_id = 0,
                                    now_agent_id = request.childAgent,
                                    operate_agent_id = request.childAgent,
                                    top_agent_id = request.Agent,
                                    type_id = 2,
                                    create_time = DateTime.Now
                                });
                            }
                        }

                        var updateResult = _cameraDistributeRespository.Update(noexistUserInfo);
                        //重置传入的参数
                        request.childAgent = int.Parse(noexistUserInfo.Agent);//(int)distributedAgentId;
                        request.uiCustKey = noexistUserInfo.OpenId;
                        msgIsDistributed = noexistUserInfo.IsDistributed;
                    }

                    //添加步骤记录
                    Task<int> resultNum = _cameraDistributeRespository.AddCrmStepsAsync(new bx_crm_steps { agent_id = request.childAgent, b_uid = request.buId, create_time = DateTime.Now, json_content = "{\"camertime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}", type = 6 });
                }

                int isDistributed = 0;
                #region 注释掉以前逻辑
                ////原先摄像头进来的是0；非摄像头的顶级是0，非顶级是2
                ////20180105改：摄像头->顶级+管理员是0，非顶级+管理员是3；非摄像头->顶级+管理员是0，非顶级+管理员是2
                //if (isTopAgent.Item1)
                //{//如果顶级或管理员直接改为0
                //    isDistributed = 0;
                //}
                //else
                //{
                //    //非顶级和管理员，摄像头改3，非摄像头改2
                //    isDistributed = request.reqRenewalType == 3 ? 3 : 2;
                //}
                #endregion
                //摄像头 顶级和管理员数据为未分配，其他为已分配
                if (request.reqRenewalType == 3)
                {
                    if (isTopAgent.Item1)
                    {
                        isDistributed = 0;
                    }
                    else
                    {
                        isDistributed = 2;
                    }
                }
                //非摄像头 顶级为未分配，其他为已分配
                else
                {
                    if (isTopAgent.Item2)
                    {//顶级
                        isDistributed = 0;
                    }
                    else
                    {
                        isDistributed = 2;
                    }
                }

                if (msgIsDistributed == 0)
                    msgIsDistributed = isDistributed;
                //更新数据库状态：分配状态、是否摄像头进店、进店时间
                _cameraDistributeRespository.UpdateUserRenewalTypeAndDistributed(request.buId, request.reqRenewalType, isDistributed, isExitUserinfo, isDistributedUserInfo);
                //放入回收站的数据不在分配
                if (!isSuccess)
                {
                    isSuccess = true;
                    return result;
                }

                //重新续保
                if (isExitUserinfo)
                {
                    //杰哥出马一个顶俩
                    ////Task.Factory.StartNew(() =>
                    ////{
                    ////    string strurl =
                    ////        string.Format("LicenseNo={0}&CityCode={1}&ChildAgent={2}&CustKey={3}&NeedCarMoldFilter=1&CameraAgent={4}&Agent={5}", request.licenseNo, request.cityCode, request.childAgent, request.uiCustKey, request.CameraAgent, request.Agent);
                    ////    string strseccode = strurl.GetMd5();
                    ////    string strReInfoUrl = string.Format("{0}/api/CarInsurance/GetReInfo?{1}&SecCode={2}", ApplicationSettingsFactory.GetApplicationSettings().BaoJiaJieKou, strurl, strseccode);
                    ////    HttpWebAsk.Get(strReInfoUrl);
                    ////});
                }

                if (request.reqRenewalType == 3)
                {//摄像头进店的、非顶级账号，发送signalr到期通知
                    if (!string.IsNullOrEmpty(forceExpireDate))
                    {
                        //城市Id、车牌号、商业险到期时间、交强险到期时间、下级代理人Id、顶级代理人Id、用户Id
                        AddNoticexb(request.licenseNo, request.carModelKey, request.childAgent, request.Agent, request.buId, msgIsDistributed, intime, endDays);
                    }
                }
                isSuccess = true;
            }
            catch (Exception e)
            {
                logError.Error(string.Format("发生异常：{0}\n{1}\n{2}\n{3}", e.Source, e.StackTrace, e.Message, e.InnerException));
                isSuccess = false;
                result = e.Message;
            }
            return result;
        }
        #endregion
        /// <summary>
        /// 是否到期
        /// 先判断交强险到期时间，再判断商业险到期时间；只要有任意一个时间在续保期（不含脱保）就弹出进店提醒；
        /// </summary>
        /// <param name="cityCode">城市代码</param>
        /// <param name="businessExpireDate">商业险到期时间</param>
        /// <param name="forceExpireDate">交强险到期时间</param>
        /// <param name="forceDays"></param>
        /// <returns></returns>
        public bool IsInTime(int cityCode, string businessExpireDate, string forceExpireDate, out int endDays)
        {
            endDays = 0;
            try
            {
                var cityQuoteDay = _cityQuoteDayRepository.Get(cityCode);
                if (cityQuoteDay == null)
                {
                    logError.Error("cityid=" + cityCode + " 的城市没有设置交强和商业险有效报价时间");
                    return false;
                }
                //获取代理区域的城市到期天数设置
                int forceNum = cityQuoteDay.quotedays ?? 90;
                int bizNum = cityQuoteDay.bizquotedays ?? 90;
                //先判断交强险是否符合xx天到期
                if (!string.IsNullOrWhiteSpace(forceExpireDate))
                {
                    int forceDays = 0;
                    if (DateDiff(DateTime.Parse(forceExpireDate), DateTime.Now, out forceDays))
                    {
                        endDays = forceDays;
                        if (forceDays <= forceNum)
                        {//如果交强险满足条件直接返回true
                            return true;
                        }
                    }
                }
                //再判断商业险是否符合xx天到期，既然走到此步，说明交强险不满足条件，所以开始判断商业险只要满足即可返回true
                if (!string.IsNullOrWhiteSpace(businessExpireDate))
                {
                    int bizDays = 0;
                    if (DateDiff(DateTime.Parse(businessExpireDate), DateTime.Now, out bizDays))
                    {
                        endDays = bizDays;
                        if (bizDays > bizNum)
                        {//商业险时间跟数据规定的时间比较，超过，则不满足
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                logError.Info("计算到期时间发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                return false;
            }
        }
        public bool DateDiff(DateTime dateTime1, DateTime dateTime2, out int days)
        {
            TimeSpan ts = dateTime1 - dateTime2;
            days = ts.Days;
            if (days < 0)
            {
                return false;
            }
            return true;
        }
        public long AddNoticexb(string licenseNo, string carMold, int childAgent, int agentId, long buid, int distributed, bool isInTime, int forceDays)
        {
            #region 开始发消息
            //如果不在续保期，直接退出
            if (!isInTime)
                return 0;

            //APP推 //20170722修改，只有到期才发通知
            PushApp(buid, licenseNo, forceDays, childAgent, agentId, distributed, carMold);

            if (childAgent != agentId)
            {//只有下级账号才推
                //Signal推
                PushSignal(buid, licenseNo, forceDays, childAgent);
            }
            #endregion
            return 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="licenseNo"></param>
        /// <param name="forceDays"></param>
        /// <param name="childAgent"></param>
        /// <param name="agentId"></param>
        /// <param name="distributed">分配状态</param>
        /// <param name="moldName">车型</param>
        public void PushApp(long buid, string licenseNo, int forceDays, int childAgent, int agentId, int distributed, string moldName)
        {
            if (forceDays < 0)
            {
                //如果脱保，不执行以下操作
                return;
            }
            string url = string.Format("{0}/api/MessagePush/PushMessageToApp", _crmCenterHost);
            string strgenjin = string.Empty;
            bx_agent bxAgent = _cameraDistributeRespository.GetAgent(childAgent);
            //顶级代理
            int topAgent = bxAgent != null ? bxAgent.TopAgentId : 0;
            int topAgent2 = bxAgent != null ? bxAgent.ParentAgent : 0;
            if (distributed > 0)
            {
                strgenjin = string.Format("，由业务员{0}跟进", bxAgent != null ? bxAgent.AgentName : "");
            }
            else
            {
                strgenjin = "，请点击分配给业务员";
            }
            string strmsg = string.Format("{0}已进店，车险到期还有{1}天{2}。", licenseNo, forceDays, strgenjin);
            //消息表插入消息
            //bx_message
            bx_message bxMessage = new bx_message()
            {
                Title = strmsg,
                //Body = strmsg,
                Msg_Type = 8,
                Create_Time = DateTime.Now,
                Update_Time = DateTime.Now,
                Msg_Status = 1,
                Msg_Level = 0,
                Send_Time = DateTime.Now,
                Create_Agent_Id = childAgent,
                License_No = licenseNo,
                Buid = buid,
                MsgStatus = "1"
            };
            //bx_msgindex
            int msgId = _cameraDistributeRespository.Add(bxMessage);
            if (msgId < 1)
            {
                //如果message插入失败，就不执行以下操作了
                return;
            }
            #region 第一次给直接分配人推消息
            bx_msgindex bxMsgindex = new bx_msgindex()
            {
                AgentId = childAgent,
                Deleted = 0,
                Method = 4,//APP
                MsgId = msgId,
                ReadStatus = 0,
                SendTime = DateTime.Now
            };
            long msgIdxId = _cameraDistributeRespository.AddMsgIdx(bxMsgindex);
            if (msgIdxId < 1)
            {
                //如果msgindex插入失败，就不执行以下操作了
                return;
            }
            //给APP推消息
            PushedMessage sendApp;
            string pushData = string.Empty;
            string resultMessage = string.Empty;
            bx_agent_xgaccount_relationship bxXgAccount = _cameraDistributeRespository.GetXgAccount(childAgent);
            if (bxXgAccount != null && !string.IsNullOrEmpty(bxXgAccount.Account))
            {
                //如果没有账号，不执行以下操作

                //bx_msgindex
                //消息内容
                sendApp = new PushedMessage
                {
                    Title = GetStrMsg(topAgent2, strmsg),
                    Content = GetStrMsg(topAgent2, strmsg),
                    MsgId = msgId,
                    Account = bxXgAccount.Account,
                    BuId = buid,
                    MsgType = 8
                };
                pushData = sendApp.ToJson();
                logInfo.Info(string.Format("消息发送PushMessageToApp请求串: url:{0}/api/MessagePush/PushMessageToApp ; data:{1}", _crmCenterHost, pushData));
                resultMessage = HttpWebAsk.HttpClientPostAsync(pushData, url);
                logInfo.Info(string.Format("消息发送PushMessageToApp返回值:{0}", resultMessage));
            }
            #endregion

            #region 给顶级推消息
            msgIdxId = 0;//第一次存消息自定义的参数重新初始化
            if (bxAgent != null && topAgent != bxAgent.Id && topAgent != 0)
            {
                bx_msgindex bxMsgindex2 = new bx_msgindex()
                {
                    AgentId = topAgent,
                    Deleted = 0,
                    Method = 4,//APP
                    MsgId = msgId,
                    ReadStatus = 0,
                    SendTime = DateTime.Now
                };
                msgIdxId = _cameraDistributeRespository.AddMsgIdx(bxMsgindex2);
            }
            if (msgIdxId < 1)
            {
                //如果msgindex插入失败，就不执行以下操作了
                return;
            }
            //给APP推消息
            bxXgAccount = new bx_agent_xgaccount_relationship();
            bxXgAccount = _cameraDistributeRespository.GetXgAccount(topAgent);
            if (bxXgAccount == null || string.IsNullOrEmpty(bxXgAccount.Account))
            {
                //如果没有账号，不执行以下操作
                return;
            }
            //bx_msgindex
            //消息内容
            sendApp = new PushedMessage
            {
                Title = strmsg,
                Content = strmsg,
                MsgId = msgId,
                Account = bxXgAccount.Account,
                BuId = buid,
                MsgType = 8
            };
            pushData = sendApp.ToJson();
            logInfo.Info(string.Format("消息发送PushMessageToApp请求串: url:{0}/api/MessagePush/PushMessageToApp ; data:{1}", _crmCenterHost, pushData));
            resultMessage = HttpWebAsk.HttpClientPostAsync(pushData, url);
            logInfo.Info(string.Format("消息发送PushMessageToApp返回值:{0}", resultMessage));
            #endregion
        }
        public string GetStrMsg(int topAgent, string strmsg)
        {
            if (topAgent == 0) return strmsg;
            string[] msg = strmsg.Split('，');
            if (msg.Length > 2)
            {
                return msg[0] + "，" + msg[1] + "。";
            }
            return strmsg;
        }
        /// <summary>
        /// 往Signal平台推消息
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="licenseNo"></param>
        /// <param name="forceDays"></param>
        /// <param name="childAgent"></param>
        public void PushSignal(long buid, string licenseNo, int forceDays, int childAgent)
        {
            //发消息的基础模型1
            var model = new CompositeBuIdLicenseNo
            {
                BuId = buid,
                LicenseNo = licenseNo,
                Days = forceDays
            };
            //发消息基础模型2
            var list = new List<CompositeBuIdLicenseNo> { model };
            //发消息基础模型3
            var sendModel = new DuoToNoticeViewModel
            {
                AgentId = childAgent,
                Data = list,
                BuidsString = buid.ToString()
            };
            //发消息的最终模型
            var sendList = new List<DuoToNoticeViewModel> { sendModel };
            string url = string.Format("{0}/api/Message/SendDuetToNotice", _messageCenterHost);
            string data = sendList.ToJson();
            logInfo.Info(string.Format("消息发送SendDuetToNotice请求串: url:{0}/api/Message/SendDuetToNotice ; data:{1}", _messageCenterHost, data));
            //post消息发送
            string resultMessage = HttpWebAsk.HttpClientPostAsync(data, url);
            logInfo.Info(string.Format("消息发送SendDuetToNotice返回值:{0}", resultMessage));
        }
        public long GerRedirsSealman(int agentId, long index = 0, long _index = 0)
        {
            //获取设置的下级代理人Id列表
            var data = GetRedirsSealmans(agentId);
            //var data = _agentRepository.GetUsedSons(agentId);
            if (data == null || !data.Any())
                return 0;

            //获取当前摄像头的索引值并赋初始值
            try
            {
                if (_index == 0)
                    _index = index = _hashOperator.Get<long>(_sealmanIndexRedirs, agentId.ToString());
            }
            catch
            {
                _index = index = 0;
            }
            //循环的次数超过了设置的代理人数说明没有符合的代理人，因此返回0，防止死循环
            if (index - _index > data.Count)
                return 0;

            //求余获取所分配代理人的Id
            var userId = data[(int)index % data.Count()];

            //查询该代理人今天是否请假
            if (!isLeave(userId))
            {
                ////删除摄像头索引值
                //if (_cacheClient.KeyExists(_sealmanIndexRedirs, agentId.ToString()))
                //    _cacheClient.Remove(_sealmanIndexRedirs, agentId.ToString());

                //自增1次
                _index++;
                //索引值大于2W是归0，太大了
                if (_index > 20000) _index = 0;
                //设置摄像头索引值
                _hashOperator.Set(_sealmanIndexRedirs, agentId.ToString(), _index);
                return userId;
            }
            //该代理今天请假,查询下一位代理人
            index++;
            return GerRedirsSealman(agentId, index, _index);
        }
        /// <summary>
        /// 获取缓存的销售人Id
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public List<long> GetRedirsSealmans(int agentId)
        {
            ////redis做了修改
            var data = _hashOperator.Get<List<long>>(_sealmanRedirs, agentId.ToString());
            if (data == null)
            {
                data = _cameraDistributeRespository.FindAgentIdBySealman(agentId).Distinct().ToList();
                data = _agentRepository.GetList(data).Where(ag => ag.IsUsed == 1).Select(ag => (long)ag.Id).ToList();
                _hashOperator.Set(_sealmanRedirs, agentId.ToString(), data);
            }
            return data;
        }
        /// <summary>
        /// 该业务员今天是否存在请假
        /// </summary>
        /// <param name="agentId">业务员Id</param>
        /// <returns></returns>
        public bool isLeave(long agentId)
        {
            var leaves = _cameraDistributeRespository.FindSealmanLeave((int)agentId);
            if (leaves == null)
                return false;
            //查询盖下级代理人今天是否有请假
            foreach (var item in leaves)
            {
                if (item.leave.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                    return true;
            }
            return false;
        }

        #region 新摄像头
        public int GetModelFilterId(int agentId, string carModelKey)
        {
            //redis 做了修改
            var data = _hashOperator.Get<List<carMold>>(_cameraCarModelsKey, agentId.ToString());
            if (data == null)
            {
                data = FindCarType(agentId);
                _hashOperator.Set(_cameraCarModelsKey, agentId.ToString(), data);
            }
            if (data.Any())
            {
                foreach (var item in data)
                {
                    //是否包含该数据
                    if (carModelKey.IndexOf(item.name) > -1)
                    {
                        return item.id;
                    }
                }
            }
            //如果没设置返回0
            if (data == null || data.Count() == 0)
            {
                return 0;
            }
            return -1;
        }
        public bool IsInTime(int cityCode, string businessExpireDate, string forceExpireDate, int cameraAgent, out int endDays, out int isRemind)
        {
            endDays = -1;
            isRemind = 1;
            try
            {
                var cityQuoteDay = _cityQuoteDayRepository.Get(cityCode);
                var cameraConfig = _cameraConfigRepositoty.Get(cameraAgent).FirstOrDefault();
                int forceNum = 90;
                int bizNum = 90;
                bool isTrue = false;
                if (cameraConfig == null && cityQuoteDay == null)
                {
                    logError.Error("cityid=" + cityCode + " 的城市没有设置交强和商业险有效报价时间");
                    return false;
                }
                else
                {
                    if (cameraConfig != null && cityQuoteDay != null)
                    {
                        isRemind = cameraConfig.IsRemind == null ? 0 : (int)cameraConfig.IsRemind;
                        forceNum = (cameraConfig.Days == 0 || cameraConfig.Days == null) ? (cityQuoteDay.quotedays ?? 90) : (int)cameraConfig.Days;
                        bizNum = (cameraConfig.Days == 0 || cameraConfig.Days == null) ? (cityQuoteDay.bizquotedays ?? 90) : (int)cameraConfig.Days;
                    }
                    else if (cameraConfig == null && cityQuoteDay != null)
                    {
                        forceNum = cityQuoteDay.quotedays ?? 90;
                        bizNum = cityQuoteDay.bizquotedays ?? 90;
                    }
                }
                if (string.IsNullOrWhiteSpace(forceExpireDate) && string.IsNullOrWhiteSpace(businessExpireDate))
                {
                    return false;
                }
                else if (!string.IsNullOrWhiteSpace(forceExpireDate) && string.IsNullOrWhiteSpace(businessExpireDate))
                {
                    int forceDays = 0;
                    if (DateDiff(DateTime.Parse(forceExpireDate), DateTime.Now, out forceDays))
                    {
                        endDays = forceDays;
                        if (forceDays > forceNum)
                        {//商业险时间跟数据规定的时间比较，超过，则不满足
                            isTrue = false;
                        }
                        else
                        {
                            isTrue = true;
                        }
                    }
                }
                else if (!string.IsNullOrWhiteSpace(businessExpireDate) && string.IsNullOrWhiteSpace(forceExpireDate))
                {
                    int bizDays = 0;
                    if (DateDiff(DateTime.Parse(businessExpireDate), DateTime.Now, out bizDays))
                    {
                        endDays = bizDays;
                        if (bizDays <= bizNum)
                        {//如果交强险满足条件直接返回true
                            isTrue = true;
                        }
                    }
                }
                else
                {
                    int forceDays = 0;
                    int bizDays = 0;
                    DateDiff(DateTime.Parse(forceExpireDate), DateTime.Now, out forceDays);
                    DateDiff(DateTime.Parse(businessExpireDate), DateTime.Now, out bizDays);
                    if (forceDays <= forceNum || bizDays <= bizNum)
                    {
                        isTrue = true;
                    }
                    endDays = (forceDays - forceNum) > (bizDays - bizNum) ? (bizDays) : (forceDays);
                    if (endDays < 0)
                    {
                        if (forceDays >= 0)
                        {
                            endDays = forceDays;
                            if (forceDays > forceNum)
                            {
                                isTrue = false;
                            }
                        }
                        if (bizDays >= 0)
                        {
                            endDays = bizDays;
                            if (bizDays > bizNum)
                            {
                                isTrue = false;
                            }
                        }
                    }
                }
                return isTrue;
            }
            catch (Exception ex)
            {
                logError.Info("计算到期时间发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + "\n请求参数:" + "CityCode:" + cityCode + "  businessExpireDate:" + businessExpireDate + "  forceExpireDate:" + forceExpireDate + "  cameraAgent" + cameraAgent.ToString());
                return false;
            }
        }
        public long AddNoticexb(bx_userinfo userInfo, int cameraAgent, int isRemind, int endDays)
        {
            if (userInfo == null)
            {
                return 0;
            }
            if (endDays < 0)
            {
                //如果脱保，不执行以下操作
                return 0;
            }
            else if ((userInfo.IsReView == 4 || userInfo.IsReView == 9) && isRemind == 0)
            {
                return 0;
            }
            #region 开始发消息
            bx_agent bxAgent = _agentRepository.GetAgentAsync(int.Parse(userInfo.Agent));
            //APP推 
            PushApp(userInfo, endDays, cameraAgent, bxAgent);

            //PC推
            PushSignal(userInfo, endDays, cameraAgent, bxAgent);
            #endregion
            return 1;
        }
        public void PushApp(bx_userinfo userInfo, int endDays, int cameraAgent, bx_agent bxAgent)
        {
            string url = string.Format("{0}/api/MessagePush/PushMessageToApp", _crmCenterHost);
            string strgenjin = string.Empty;
            //顶级代理
            int topAgent = bxAgent != null ? bxAgent.TopAgentId : 0;
            if (userInfo.IsDistributed > 0)
            {
                strgenjin = string.Format("，由业务员{0}跟进", bxAgent != null ? bxAgent.AgentName : "");
            }
            else
            {
                strgenjin = "，请点击分配给业务员";
            }
            var userinfoRenewalInfo = _userinfoRenewalInfoRepository.FindByBuidAsync(userInfo.Id);
            var clientName = "";
            var categoryInfo = "";
            if (userinfoRenewalInfo != null)
            {
                clientName = string.IsNullOrWhiteSpace(userinfoRenewalInfo.client_name) ? "" : userinfoRenewalInfo.client_name;
                if (userinfoRenewalInfo.CustomerType != 0)
                {
                    var customerCategories = _customerCategoriesRepository.GetAsync(userinfoRenewalInfo.CustomerType);
                    if (customerCategories != null)
                    {
                        categoryInfo = customerCategories.CategoryInfo;
                    }
                }
            }
            string strmsg = "";
            if (!string.IsNullOrWhiteSpace(clientName) && !string.IsNullOrWhiteSpace(categoryInfo))
            {
                strmsg = string.Format("{0}已进店，车险到期还有{1}天{2}。", userInfo.LicenseNo + "(" + clientName + "|" + categoryInfo + ")", endDays, strgenjin);
            }
            else if (!string.IsNullOrWhiteSpace(clientName) && string.IsNullOrWhiteSpace(categoryInfo))
            {
                strmsg = string.Format("{0}已进店，车险到期还有{1}天{2}。", userInfo.LicenseNo + "(" + clientName + ")", endDays, strgenjin);
            }
            else if (string.IsNullOrWhiteSpace(clientName) && !string.IsNullOrWhiteSpace(categoryInfo))
            {
                strmsg = string.Format("{0}已进店，车险到期还有{1}天{2}。", userInfo.LicenseNo + "(" + categoryInfo + ")", endDays, strgenjin);
            }
            else if (string.IsNullOrWhiteSpace(clientName) && string.IsNullOrWhiteSpace(categoryInfo))
            {
                strmsg = string.Format("{0}已进店，车险到期还有{1}天{2}。", userInfo.LicenseNo, endDays, strgenjin);
            }
            //消息表插入消息
            //bx_message
            bx_message bxMessage = new bx_message()
            {
                Title = strmsg,
                //Body = strmsg,
                Msg_Type = 8,
                Create_Time = DateTime.Now,
                Update_Time = DateTime.Now,
                Msg_Status = 1,
                Msg_Level = 0,
                Send_Time = DateTime.Now,
                Create_Agent_Id = bxAgent.Id,
                License_No = userInfo.LicenseNo,
                Buid = userInfo.Id,
                MsgStatus = "1"
            };
            //bx_msgindex
            int msgId = _cameraDistributeRespository.Add(bxMessage);
            if (msgId < 1)
            {
                //如果message插入失败，就不执行以下操作了
                return;
            }
            #region 给直接分配人推消息
            bx_msgindex bxMsgindex = new bx_msgindex()
            {
                AgentId = bxAgent.Id,
                Deleted = 0,
                Method = 4,//APP
                MsgId = msgId,
                ReadStatus = 0,
                SendTime = DateTime.Now
            };
            long msgIdxId = _cameraDistributeRespository.AddMsgIdx(bxMsgindex);
            if (msgIdxId < 1)
            {
                //如果msgindex插入失败，就不执行以下操作了
                return;
            }
            //给APP推消息
            PushedMessage sendApp;
            string pushData = string.Empty;
            string resultMessage = string.Empty;
            bx_agent_xgaccount_relationship bxXgAccount = _cameraDistributeRespository.GetXgAccount(bxAgent.Id);
            if (bxXgAccount != null && !string.IsNullOrEmpty(bxXgAccount.Account))
            {
                //如果没有账号，不执行以下操作

                //bx_msgindex
                //消息内容
                sendApp = new PushedMessage
                {
                    Title = GetStrMsg(strmsg),
                    Content = GetStrMsg(strmsg),
                    MsgId = msgId,
                    Account = bxXgAccount.Account,
                    BuId = userInfo.Id,
                    MsgType = 8
                };
                pushData = sendApp.ToJson();
                logInfo.Info(string.Format("消息发送PushMessageToApp请求串: url:{0}/api/MessagePush/PushMessageToApp ; data:{1}", _crmCenterHost, pushData));
                resultMessage = HttpWebAsk.HttpClientPostAsync(pushData, url);
                logInfo.Info(string.Format("消息发送PushMessageToApp返回值:{0}", resultMessage));
            }
            #endregion
            #region 给顶级推消息
            msgIdxId = 0;//第一次存消息自定义的参数重新初始化
            if (bxAgent != null && topAgent != bxAgent.Id && topAgent != 0)
            {
                bx_msgindex bxMsgindex2 = new bx_msgindex()
                {
                    AgentId = topAgent,
                    Deleted = 0,
                    Method = 4,//APP
                    MsgId = msgId,
                    ReadStatus = 0,
                    SendTime = DateTime.Now
                };
                msgIdxId = _cameraDistributeRespository.AddMsgIdx(bxMsgindex2);
            }
            if (msgIdxId < 1)
            {
                //如果msgindex插入失败，就不执行以下操作了
                return;
            }
            //给APP推消息
            bxXgAccount = new bx_agent_xgaccount_relationship();
            bxXgAccount = _cameraDistributeRespository.GetXgAccount(topAgent);
            if (bxXgAccount != null && !string.IsNullOrEmpty(bxXgAccount.Account))
            {
                sendApp = new PushedMessage
                {
                    Title = strmsg,
                    Content = strmsg,
                    MsgId = msgId,
                    Account = bxXgAccount.Account,
                    BuId = userInfo.Id,
                    MsgType = 8
                };
                pushData = sendApp.ToJson();
                logInfo.Info(string.Format("消息发送PushMessageToApp请求串: url:{0}/api/MessagePush/PushMessageToApp ; data:{1}", _crmCenterHost, pushData));
                resultMessage = HttpWebAsk.HttpClientPostAsync(pushData, url);
                logInfo.Info(string.Format("消息发送PushMessageToApp返回值:{0}", resultMessage));
            }
            //bx_msgindex
            //消息内容

            #endregion
            #region 给摄像头推消息
            msgIdxId = 0;//第一次存消息自定义的参数重新初始化
            if (bxAgent != null && cameraAgent != bxAgent.Id && cameraAgent != 0 && cameraAgent != topAgent)
            {
                bx_msgindex bxMsgindex3 = new bx_msgindex()
                {
                    AgentId = cameraAgent,
                    Deleted = 0,
                    Method = 4,//APP
                    MsgId = msgId,
                    ReadStatus = 0,
                    SendTime = DateTime.Now
                };
                msgIdxId = _cameraDistributeRespository.AddMsgIdx(bxMsgindex3);
            }
            if (msgIdxId < 1)
            {
                //如果msgindex插入失败，就不执行以下操作了
                return;
            }
            //给APP推消息
            bxXgAccount = new bx_agent_xgaccount_relationship();
            bxXgAccount = _cameraDistributeRespository.GetXgAccount(cameraAgent);
            if (bxXgAccount == null || string.IsNullOrEmpty(bxXgAccount.Account))
            {
                //如果没有账号，不执行以下操作
                return;
            }
            //bx_msgindex
            //消息内容
            sendApp = new PushedMessage
            {
                Title = strmsg,
                Content = strmsg,
                MsgId = msgId,
                Account = bxXgAccount.Account,
                BuId = userInfo.Id,
                MsgType = 8
            };
            pushData = sendApp.ToJson();
            logInfo.Info(string.Format("消息发送PushMessageToApp请求串: url:{0}/api/MessagePush/PushMessageToApp ; data:{1}", _crmCenterHost, pushData));
            resultMessage = HttpWebAsk.HttpClientPostAsync(pushData, url);
            logInfo.Info(string.Format("消息发送PushMessageToApp返回值:{0}", resultMessage));
            #endregion
        }
        /// <summary>
        /// 往signal平台推消息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="endDays"></param>
        /// <param name="cameraAgent"></param>
        public void PushSignal(bx_userinfo userInfo, int endDays, int cameraAgent, bx_agent bxAgent)
        {
            if (userInfo == null || userInfo.Id == 0)
            {
                return;
            }
            //发消息的最终模型
            var sendList = new List<DuoToNoticeViewModel>();
            //发消息的基础模型1
            var model = new CompositeBuIdLicenseNo
            {
                BuId = userInfo.Id,
                LicenseNo = userInfo.LicenseNo,
                Days = endDays
            };
            //发消息基础模型2
            var list = new List<CompositeBuIdLicenseNo> { model };
            //发消息基础模型3

            var sendModel = new DuoToNoticeViewModel
            {
                AgentId = bxAgent.Id,
                Data = list,
                BuidsString = userInfo.Id.ToString()
            };
            sendList.Add(sendModel);

            if (bxAgent.TopAgentId != bxAgent.Id && bxAgent.TopAgentId != 0)
            {
                sendModel = new DuoToNoticeViewModel
                {
                    AgentId = bxAgent.TopAgentId,
                    Data = list,
                    BuidsString = userInfo.Id.ToString()
                };
                sendList.Add(sendModel);
            }
            if (cameraAgent != bxAgent.Id && cameraAgent != bxAgent.TopAgentId && cameraAgent != 0)
            {
                sendModel = new DuoToNoticeViewModel
                {
                    AgentId = cameraAgent,
                    Data = list,
                    BuidsString = userInfo.Id.ToString()
                };
                sendList.Add(sendModel);
            }
            string url = string.Format("{0}/api/Message/SendDuetToNotice", _messageCenterHost);
            string data = sendList.ToJson();
            logInfo.Info(string.Format("消息发送SendDuetToNotice请求串: url:{0}/api/Message/SendDuetToNotice ; data:{1}", _messageCenterHost, data));
            //post消息发送
            string resultMessage = HttpWebAsk.HttpClientPostAsync(data, url);
            logInfo.Info(string.Format("消息发送SendDuetToNotice返回值:{0}", resultMessage));
        }
        public string GetStrMsg(string strmsg)
        {
            string[] msg = strmsg.Split('，');
            if (msg.Length > 2)
            {
                return msg[0] + "，" + msg[1] + "。";
            }
            return strmsg;
        }
        #endregion
    }
}
