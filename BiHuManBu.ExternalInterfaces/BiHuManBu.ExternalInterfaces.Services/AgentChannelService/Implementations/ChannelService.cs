using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using log4net;
using ServiceStack.Text;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Implementations
{
    public class ChannelService : IChannelService
    {
        private ILog logInfo;
        private ILog logError;
        private IValidateService _validateService;
        private IChannelModelMapRedisService _channelModelMapRedisService;
        private readonly IAgentConfigByCityService _agentConfigByCityService;
        private readonly IAgentUKeyService _agentUKeyService;

        public ChannelService(IChannelModelMapRedisService channelModelMapRedisService, IValidateService validateService, IAgentConfigByCityService agentConfigByCityService, IAgentUKeyService agentUKeyService)
        {
            logInfo = LogManager.GetLogger("INFO");
            logError = LogManager.GetLogger("ERROR");
            _channelModelMapRedisService = channelModelMapRedisService;
            _validateService = validateService;
            _agentConfigByCityService = agentConfigByCityService;
            _agentUKeyService = agentUKeyService;
        }

        /// <summary>
        /// 获取渠道及状态
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public GetChannelStatusResponse GetChannelStatus(GetChannelStatusRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetChannelStatusResponse response = new GetChannelStatusResponse();
            try
            {
                //参数校验
                BaseResponse baseResponse = _validateService.Validate(request, pairs);
                if (baseResponse.Status == HttpStatusCode.Forbidden)
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                if (request.CityCode == 0)
                {
                    //request.CityCode = _agentConfigByCityService.GetAgentCityCodeByChannelId(request.Agent,
                        //request.ChannelId);
                    //当CityCode=0时候，需要根据ChannelId获得CityCode。而ChannelId对应bx_agent_ukey.Id
                    request.CityCode = _agentUKeyService.GetAgentCityCodeByUKId((int)request.ChannelId);
                }
                //查询ukey信息
                List<bx_agent_config> configModel = _agentConfigByCityService.GetAgentConfigByCity(request.Agent,request.CityCode);
                //如果configModel为空，返回错误
                if (configModel == null)
                {
                    response.ErrCode = -1;
                    response.ErrMsg = "未查到代理人配置信息";
                    return response;
                }
                //取缓存渠道模型
                var list = _channelModelMapRedisService.GetAgentCacheChannelList(configModel);
                //判断是否有值
                if (list.Any())
                {
                    //如果修改成功，则保存用户名
                    response.Status = HttpStatusCode.OK;
                    response.CacheChannelList = list;
                }
                else
                {
                    //修改失败
                    response.ErrCode = 0;
                    response.ErrMsg = "获取信息失败";
                }
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("获取渠道及状态请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + ",返回对象信息：" + request.ToJson());
            }
            return response;
        }

    }
}
