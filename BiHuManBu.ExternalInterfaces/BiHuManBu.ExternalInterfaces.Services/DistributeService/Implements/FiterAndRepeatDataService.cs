using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.Distribute.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.DistributeService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using BiHuManBu.ExternalInterfaces.Infrastructure;

namespace BiHuManBu.ExternalInterfaces.Services.DistributeService.Implements
{
    public class FiterAndRepeatDataService : IFiterAndRepeatDataService
    {
        private ICameraDistributeRepository _cameraDistributeRespository;
        private readonly IAuthorityService _authorityService;
        private IAgentRepository _agentRepository;
        private IUserInfoRepository _userInfoRepository;
        private readonly IAgentService _agentService;
        private readonly ICameraDistributeService _cameraDistributeService;
        private ILog logError;
        private ILog logInfo;
        public FiterAndRepeatDataService(ICameraDistributeRepository cameraDistributeRespository, IAuthorityService authorityService, IAgentRepository agentRepository, IUserInfoRepository userInfoRepository, IAgentService agentService, ICameraDistributeService cameraDistributeService)
        {
            _cameraDistributeService = cameraDistributeService;
            _userInfoRepository = userInfoRepository;
            _cameraDistributeRespository = cameraDistributeRespository;
            _authorityService = authorityService;
            _agentRepository = agentRepository;
            _agentService = agentService;
            logError = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");
        }
        public long GetFiterDataByCarVin(int agent, int childAgent, int repeatStatus, int roleType, string carVIN, string cameraId, string custKey, int cityCode)
        {
            long buid = 0;
            bool isAdmin = (roleType == 3 || roleType == 4);
            if (!string.IsNullOrEmpty(carVIN))
            {
                if (repeatStatus == 0 || repeatStatus == 1)
                {
                    List<bx_userinfo> userInfoList = GetUserinfoByCarVinAndAgentAsync(agent, carVIN);
                    if (userInfoList.Count > 1)
                    {
                        buid = userInfoList[1].Id;
                        //新创建的记录还没有extend所以只需要更新IsTest，不需要管extend
                        userInfoList.FirstOrDefault().IsTest = 3;
                        userInfoList.FirstOrDefault().UpdateTime = DateTime.Now;
                        userInfoList.FirstOrDefault().IsCamera = true;
                        userInfoList.FirstOrDefault().CameraTime = DateTime.Now;
                        userInfoList[1].LicenseNo = userInfoList.FirstOrDefault().LicenseNo;
                        userInfoList[1].UpdateTime = DateTime.Now;
                        _userInfoRepository.UpdateSync(userInfoList[1]);
                        _userInfoRepository.UpdateSync(userInfoList.FirstOrDefault());
                        _cameraDistributeRespository.AddCrmStepsAsync(new bx_crm_steps { agent_id = childAgent, b_uid = userInfoList.FirstOrDefault().Id, create_time = DateTime.Now, json_content = "{\"camertime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}", type = 81 });
                        StringBuilder sb = new StringBuilder();
                        if (childAgent > 0)
                        {
                            sb.Append("&ChildAgent=").Append(childAgent);
                        }
                        if (!string.IsNullOrWhiteSpace(cameraId))
                        {
                            sb.Append("&CameraId=").Append(cameraId);
                        }
                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(string.Format("{0}", ConfigurationManager.AppSettings["SystemItUrl"]));
                            var getUrl = string.Format("LicenseNo={0}&CityCode={3}&Agent={1}&CustKey={2}&RenewalType=3&RenewalCarType={4}{5}", userInfoList[1].LicenseNo, agent, custKey, cityCode, 0, sb.ToString());

                            var Url = string.Format("/api/CarInsurance/getreinfo?") + getUrl;
                            var agentModel = _agentRepository.GetAgentAsync(agent);
                            var content = getUrl.ToString();
                            var seccode = (content + agentModel.SecretKey).GetMd5();
                            Url = Url + "&seccode=" + seccode;
                            var a = client.GetAsync(Url).Result;
                            logInfo.Info(ConfigurationManager.AppSettings["SystemItUrl"].ToString() + Url + "   车架号过滤重新请求  旧buid：" + userInfoList[1].Id.ToString() + " 新buid：" + userInfoList.FirstOrDefault().Id.ToString());
                        }
                    }
                }
                else if (repeatStatus == 2)
                {
                    List<bx_userinfo> userInfoList = new List<bx_userinfo>();
                    if (!isAdmin)
                    {
                        userInfoList = GetUserinfoByCarVinAndAgentAsync(childAgent, carVIN);
                    }
                    else
                    {
                        userInfoList = GetUserinfoByCarVinAndAgentAsync(agent, carVIN);
                    }
                    if (userInfoList.Count > 1)
                    {
                        buid = userInfoList[1].Id;
                        //新创建的记录还没有extend所以只需要更新IsTest，不需要管extend
                        userInfoList.FirstOrDefault().IsTest = 3;
                        userInfoList.FirstOrDefault().UpdateTime = DateTime.Now;
                        userInfoList.FirstOrDefault().IsCamera = true;
                        userInfoList.FirstOrDefault().CameraTime = DateTime.Now;
                        userInfoList[1].LicenseNo = userInfoList.FirstOrDefault().LicenseNo;
                        userInfoList[1].UpdateTime = DateTime.Now;
                        _userInfoRepository.UpdateSync(userInfoList[1]);
                        _userInfoRepository.UpdateSync(userInfoList.FirstOrDefault());
                        _cameraDistributeRespository.AddCrmStepsAsync(new bx_crm_steps { agent_id = childAgent, b_uid = userInfoList.FirstOrDefault().Id, create_time = DateTime.Now, json_content = "{\"camertime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}", type = 81 });
                        StringBuilder sb = new StringBuilder();
                        if (childAgent > 0)
                        {
                            sb.Append("&ChildAgent=").Append(childAgent);
                        }
                        if (!string.IsNullOrWhiteSpace(cameraId))
                        {
                            sb.Append("&CameraId=").Append(cameraId);
                        }
                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(string.Format("{0}", ConfigurationManager.AppSettings["SystemItUrl"]));
                            var getUrl = string.Format("LicenseNo={0}&CityCode={3}&Agent={1}&CustKey={2}&RenewalType=3&RenewalCarType={4}{5}", userInfoList[1].LicenseNo, agent, custKey, cityCode, 0, sb.ToString());
                            var Url = string.Format("/api/CarInsurance/getreinfo?") + getUrl;
                            var agentModel = _agentRepository.GetAgentAsync(agent);
                            var content = getUrl.ToString();
                            var seccode = (content + agentModel.SecretKey).GetMd5();
                            Url = Url + "&seccode=" + seccode;
                            var a = client.GetAsync(Url).Result;
                            logInfo.Info(ConfigurationManager.AppSettings["SystemItUrl"].ToString() + Url + "   车架号过滤重新请求  旧buid：" + userInfoList[1].Id.ToString() + " 新buid：" + userInfoList.FirstOrDefault().Id.ToString());
                        }
                    }
                }
            }
            return buid;
        }
        /// <summary>
        /// 原先crm接口黑名单过滤+获取重复数据接口GetInfoByCamera
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <param name="agent"></param>
        /// <param name="childAgent"></param>
        /// <param name="custKey"></param>
        /// <param name="cityCode"></param>
        /// <param name="renewalCarType"></param>
        /// <returns></returns>
        public CameraBackDataViewModel GetFiterData(string licenseNo, int agent, int childAgent, string custKey, int cityCode, int renewalCarType, int repeatStatus, int roleType)
        {
            bool isAdmin = (roleType == 3 || roleType == 4);
            CameraBackDataViewModel model = new CameraBackDataViewModel();
            try
            {
                //黑名单过滤
                bx_camera_blacklist blackmodel = _cameraDistributeRespository.GetCameraBlack(agent, childAgent, licenseNo);
                if (blackmodel != null && blackmodel.Id > 0)
                {
                    model.IsBlack = true;
                }
                else
                {
                    model.IsBlack = false;
                }
                //根据重复报价设置来获取老数据
                bx_userinfo userInfo = null;
                if (repeatStatus == 0 || repeatStatus == 1)
                {
                    List<bx_userinfo> userInfoList = GetUserinfoByLicenseAndAgent(agent, licenseNo);
                    if (userInfoList.Count > 0)
                    {
                        userInfo = userInfoList.FirstOrDefault();
                    }
                }
                else if (repeatStatus == 2)
                {
                    List<bx_userinfo> userInfoList = new List<bx_userinfo>();
                    if (!isAdmin)
                    {
                        userInfoList = GetUserinfoByLicenseAndAgent(childAgent, licenseNo);
                    }
                    else
                    {
                        userInfoList = GetUserinfoByLicenseAndAgent(agent, licenseNo);
                    }
                    if (userInfoList.Count > 0)
                    {
                        userInfo = userInfoList.FirstOrDefault();
                    }
                }

                if (userInfo != null)
                {
                    model.Buid = userInfo.Id.ToString();
                    model.Agent = userInfo.Agent;
                    model.OpenId = userInfo.OpenId;
                }
            }
            catch (Exception ex)
            {
                logInfo.Error("摄像头进店>>>根据摄像头获取信息：", ex);
            }
            return model;
        }
        public List<bx_userinfo> GetUserinfoByLicenseAndAgent(int agent, string licenseno)
        {
            //取出代理下面所有的经纪人
            var agentLists = _cameraDistributeService.GetSonsListFromRedisToString(agent);
            //_agentRepository.GetSonsList(agent);
            //根据经纪人和车牌号取所有数据
            List<bx_userinfo> listuserinfo = _userInfoRepository.GetUserinfoByLicenseAndAgent(licenseno, agentLists);
            return listuserinfo;
        }
        public List<bx_userinfo> GetUserinfoByCarVinAndAgent(int agent, string carVin)
        {
            //取出代理下面所有的经纪人
            var agentLists = _cameraDistributeService.GetSonsListFromRedisToString(agent);
            //_agentRepository.GetSonsList(agent);
            //根据经纪人和车架号取所有数据
            List<bx_userinfo> listuserinfo = _userInfoRepository.GetUserinfoByCarVinAndAgent(carVin, agentLists);
            return listuserinfo;
        }
        public List<bx_userinfo> GetUserinfoByCarVinAndAgentAsync(int agent, string carVin)
        {
            //取出代理下面所有的经纪人
            var agentLists = _cameraDistributeService.GetSonsListFromRedisToStringAsync(agent);
            //_agentRepository.GetSonsList(agent);
            //根据经纪人和车架号取所有数据
            List<bx_userinfo> listuserinfo = _userInfoRepository.GetUserinfoByCarVinAndAgent(carVin, agentLists);
            return listuserinfo;
        }
    }
}
