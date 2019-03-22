using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetAgentConfigByCityService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Services.GetAgentConfigByCityService.Implementations
{
    public class GetAgentConfigByCitysService : IGetAgentConfigByCitysService
    {
        private readonly IAgentConfigByCityService _agentConfigByCityService;
        private readonly IAgentRepository _agentRepository;
        private readonly ILog _logError;
        private readonly IChannelModelMapRedisService _channelModelMapRedisService;

        public GetAgentConfigByCitysService(IAgentConfigByCityService agentConfigByCityService, IAgentRepository agentRepository, IChannelModelMapRedisService channelModelMapRedisService)
        {
            _agentConfigByCityService = agentConfigByCityService;
            _agentRepository = agentRepository;
            _channelModelMapRedisService = channelModelMapRedisService;
            _logError = LogManager.GetLogger("ERROR");
        }

        /// <summary>
        /// 获取报价渠道
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public ResponseMultiQuotedChannelsViewModel GetAgentConfigByCityResponse(int agentId, int cityCode)
        {
            try
            {
                var listCcnfigModel = _agentConfigByCityService.GetAgentConfigByCity(agentId, cityCode);
                if (listCcnfigModel == null)
                {
                    return new ResponseMultiQuotedChannelsViewModel()
                    {
                        BusinessStatus = 0,
                        StatusMessage = "查询成功",
                        ListModels = new List<MultiQuotedChannelsViewModel>()
                    };
                }

                var lista = _channelModelMapRedisService.GetAgentCacheChannelList(listCcnfigModel);

                var list = lista.ConverToViewModel(agentId);

                return new ResponseMultiQuotedChannelsViewModel()
                {
                    BusinessStatus = 1,
                    StatusMessage = "查询成功",
                    ListModels = list
                };
            }
            catch (Exception ex)
            {
                _logError.Info("获取报价渠道接口请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                return new ResponseMultiQuotedChannelsViewModel()
                {
                    BusinessStatus = -10003,
                    StatusMessage = "查询失败，异常信息：" + ex.Message
                };
            }

        }

        /// <summary>
        /// 获取上一次报价渠道问题
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseMultiQuotedChannelsViewModel GetLastQuotedResponse(GetLastQuotedRequest request)
        {
            try
            {
                //获取当前代理下是佛有上次报价渠道信息
                var channelidSStr = _agentConfigByCityService.GetAgentSelectChannelidByBuid(request.ChildAgent, -1);
                //获取顶级上次可用渠道
                var channelidPStr = _agentConfigByCityService.GetAgentSelectChannelidByBuid(-1, request.Agent);
                
                //查询顶级所有可用渠道
                List<bx_agent_config> listCcnfigModelP = _agentConfigByCityService.GetAgentConfigByCity(request.Agent, request.CityCode);

                //
                List<bx_agent_config> listCcnfigModel;

                //当自己有上次报价渠道，转化自己的报价渠道，当自己没有取顶级的，当自己和顶级都没有上次的讲顶级的所有渠道给当前模拟定的上次
                if (channelidSStr.Count > 0)
                {
                    listCcnfigModel = _agentConfigByCityService.GetAgentConfigByChannelid(channelidSStr);
                }
                else if (channelidPStr.Count > 0)
                {
                    listCcnfigModel = _agentConfigByCityService.GetAgentConfigByChannelid(channelidPStr);
                }
                else
                {
                    listCcnfigModel = listCcnfigModelP;
                }

                //生成自己的上次报价渠道
                listCcnfigModel = listCcnfigModel.Where(x => x.city_id == request.CityCode).ToList();
                var listYy = _channelModelMapRedisService.GetAgentCacheChannelList(listCcnfigModel).Where(n => n.City == request.CityCode).ToList();
                var listY = listYy.ConverToViewModel(request.ChildAgent);

                //生成顶级所有的可用渠道
                var listXx = _channelModelMapRedisService.GetAgentCacheChannelList(listCcnfigModelP);
                var listX = listXx.ConverToViewModel(request.ChildAgent);

                var listNew = new List<MultiQuotedChannelsViewModel>();

                //循环当前所有渠道，给没有上次报价可用渠道的赋值给返回值
                var listSource = listX.Select(x => x.Source).ToArray().Distinct();
                if (listSource.Count() == 0)
                {
                    //listSource = listY.Select(x => x.Source).ToArray().Distinct();
                    //foreach (var item in listSource)
                    //{
                    //    var channelsViewModel = listY.OrderBy(x => x.ChannelStatus).FirstOrDefault(x => x.Source == item);
                    //    if (channelsViewModel != null)
                    //    {
                    //        listNew.Add(channelsViewModel);
                    //    }
                    //}
                }
                else
                {
                    foreach (var item in listSource)
                    {
                        var channelsViewModel = listY.OrderBy(x => x.ChannelStatus).FirstOrDefault(x => x.Source == item);
                        if (channelsViewModel != null)
                        {
                            listNew.Add(channelsViewModel);
                        }
                        else
                        {
                            channelsViewModel = listX.OrderBy(x => x.ChannelStatus).FirstOrDefault(x => x.Source == item);
                            if (channelsViewModel != null)
                            {
                                listNew.Add(channelsViewModel);
                            }
                        }
                    }
                }
                

                return new ResponseMultiQuotedChannelsViewModel()
                {
                    BusinessStatus = 1,
                    StatusMessage = "查询成功",
                    ListModels = listNew
                };
            }
            catch (Exception ex)
            {
                return new ResponseMultiQuotedChannelsViewModel()
                {
                    BusinessStatus = -10003,
                    StatusMessage = "查询失败，异常信息：" + ex.Message
                };
            }

        }
    }
}
