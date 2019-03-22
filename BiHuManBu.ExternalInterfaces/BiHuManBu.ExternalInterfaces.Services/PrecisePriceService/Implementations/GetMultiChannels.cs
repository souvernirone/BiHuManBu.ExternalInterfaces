using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class GetMultiChannels : IGetMultiChannels
    {
        private readonly IAgentConfigRepository _agentConfigRepository;
        private readonly ILog logError;
        private readonly IAgentConfigByCityService _agentConfigByCityService;
        public GetMultiChannels(IAgentConfigRepository agentConfigRepository, IAgentConfigByCityService agentConfigByCityService)
        {
            _agentConfigRepository = agentConfigRepository;
            logError = LogManager.GetLogger("ERROR");
            _agentConfigByCityService = agentConfigByCityService;
        }

        public string GetStrMultiChannels(int agent, long buid, List<int> quoteSourceGroup, int cityCode)
        {
            string multiChannels = string.Empty;
            //从缓存取agentconfig
            List<MultiChannels> list = _agentConfigByCityService.GetAgentConfigGroupBySource(agent, cityCode, quoteSourceGroup);
            //循环获取list里的值
            if (list.Any())
            {
                multiChannels = list.ToJson();
                //List<MultiChannels> mulList = new List<MultiChannels>();
                //foreach (var item in list)
                //{
                //    if (item.id != 0 && item.source.HasValue && item.ukey_id.HasValue)
                //    {
                //        MultiChannels model = new MultiChannels();
                //        model.ChannelId = item.id;
                //        model.Source = item.source.Value;
                //        mulList.Add(model);
                //    }
                //}
                //multiChannels = mulList.ToJson();
            }
            logError.Info(string.Format("agent:{0};cityCode:{1};buid：{2};渠道串：{3}", agent, cityCode, buid, multiChannels));
            return multiChannels;
        }
    }
}
