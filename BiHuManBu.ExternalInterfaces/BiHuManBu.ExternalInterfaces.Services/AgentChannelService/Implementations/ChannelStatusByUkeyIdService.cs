using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Implementations
{
    public class ChannelStatusByUkeyIdService : IChannelStatusByUkeyIdService
    {
        private readonly IAgentUKeyRepository _agentUKeyRepository;
        private readonly IChannelModelMapRedisService _channelModelMapRedisService;
        private readonly IValidateService _validateService;
        private ILog logError;
        public ChannelStatusByUkeyIdService(IAgentUKeyRepository agentUKeyRepository, IChannelModelMapRedisService channelModelMapRedisService, IValidateService validateService)
        {
            logError = LogManager.GetLogger("ERROR");
            _agentUKeyRepository = agentUKeyRepository;
            _channelModelMapRedisService = channelModelMapRedisService;
            _validateService = validateService;
        }
        /// <summary>
        /// 获取渠道及状态
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public GetSingelChannelStatusResponse GetChannelStatusByUkeyId(GetChannelStatusByUkeyIdRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetSingelChannelStatusResponse response = new GetSingelChannelStatusResponse();
            try
            {
                //参数校验
                BaseResponse baseResponse = _validateService.Validate(request, pairs);
                if (baseResponse.Status == HttpStatusCode.Forbidden)
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                string url = request.Url;//mac地址和url前端判断
                bx_agent_ukey agentukey = new bx_agent_ukey();
                if (request.IsId == 1)
                {
                    agentukey = _agentUKeyRepository.GetModel(request.UkeyId);
                    if (agentukey == null || agentukey.id == 0)
                    {
                        response.Status = HttpStatusCode.Forbidden;
                        return response;
                    }
                    if (agentukey != null && agentukey.isurl.HasValue)
                    {
                        url = agentukey.isurl == 1 ? agentukey.url : agentukey.macurl;
                    }
                }
                //取缓存渠道模型
                AgentCacheChannelModel channelmodel = new AgentCacheChannelModel();
                channelmodel = _channelModelMapRedisService.GetAgentCacheChannel(url);
                //判断是否有值
                if (channelmodel != null)//&& channelmodel.ChannelId!=0)
                {
                    //如果修改成功，则保存用户名
                    response.Status = HttpStatusCode.OK;
                    response.CacheChannel = channelmodel;
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
