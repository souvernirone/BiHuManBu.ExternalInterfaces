using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class CheckReInfoService : ICheckReInfoService
    {
        private IGetConfigValueService _getConfigValueService;
        private IAgentConfigByCityService _agentConfigByCityService;
        private readonly ILog logError = LogManager.GetLogger("ERROR");

        public CheckReInfoService(IGetConfigValueService getConfigValueService, IAgentConfigByCityService agentConfigByCityService)
        {
            _getConfigValueService = getConfigValueService;
            _agentConfigByCityService = agentConfigByCityService;
        }

        public string CheckXuBao(GetReInfoRequest request)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(request.LicenseNo) && string.IsNullOrWhiteSpace(request.CarVin))
            {
                msg = "请检查您的输入参数。1，如果按照车牌号续保，请填写 Licenseno参数；2，如果按照车架号和发动机号续保，请完善CarVin和Engineno参数（太平洋、平安只需要CarVin参数）";
            }
            //校验车牌号
            if (!string.IsNullOrWhiteSpace(request.LicenseNo) && string.IsNullOrWhiteSpace(request.CarVin))
            {
                if (!request.LicenseNo.IsValidLicenseno())
                {
                    msg = "请输入正确的车牌号";
                }
            }
            if (request.IsLastYearNewCar == 2 && string.IsNullOrWhiteSpace(request.CarVin))
            {
                msg = "请检查您的输入参数，如果按照车架号和发动机号续保，请完善CarVin和Engineno参数（太平洋、平安只需要CarVin参数）";
            }

            //续保特殊代理人只允许续保自己开通城市
            try
            {
                #region 临时
                if (request.Agent == 68612)
                {
                    if (request.CityCode != 13 && request.CityCode != 14)
                    {
                        return "代理人还未开通该城市";
                    }
                }
                #endregion
                else
                {
                    string strConfig = _getConfigValueService.GetConfigValue("ReInfoOnlyQuoteConfig", 1, "ReInfoOnlyQuoteConfig");
                    if (string.IsNullOrEmpty(strConfig))
                    {
                        //如果配置没有信息，直接返回结果
                        return msg;
                    }
                    #region 如果取到配置信息
                    //根据","拆分不同的代理人
                    string[] reAgent = strConfig.Split(',');
                    //reAgent没配置直接返回msg
                    if (reAgent.Length < 1)
                    {
                        return msg;
                    }
                    //如果代理人不在配置内，直接返回之前的msg
                    if (Array.IndexOf(reAgent, request.Agent.ToString()) == -1)
                        return msg;

                    List<int> listAgent = _agentConfigByCityService.GetConfigCityList(request.Agent);
                    //如果没拿到配置城市，直接返回msg
                    if (listAgent == null || !listAgent.Any())
                    {
                        return msg;
                    }
                    if (!listAgent.Contains(request.CityCode))
                    {
                        return "代理人还未开通该城市";
                    }
                }
            }
            catch (Exception ex)
            {
                logError.Info("续保特殊代理人只允许开通城市发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + " 请求对象：" + request.ToJson());
            }
            #endregion
            return msg;
        }
    }
}
