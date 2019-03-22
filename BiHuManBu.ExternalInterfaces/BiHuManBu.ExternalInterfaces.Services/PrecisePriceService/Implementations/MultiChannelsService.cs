
using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class MultiChannelsService : IMultiChannelsService
    {
        private readonly IUpdateAgentSelect _updateAgentSelect;
        private readonly IUpdateQuoteManySource _updateQuoteManySource;
        private readonly IGetMultiChannels _getMultiChannels;
        private readonly ILog logError = LogManager.GetLogger("ERROR");
        public MultiChannelsService(IUpdateAgentSelect updateAgentSelect, IUpdateQuoteManySource updateQuoteManySource, IGetMultiChannels getMultiChannels)
        {
            _updateAgentSelect = updateAgentSelect;
            _updateQuoteManySource = updateQuoteManySource;
            _getMultiChannels = getMultiChannels;
        }
        public void MultiChannels(string multiChannels, int childAgent, int agent, long buid, int quoteGroup, int cityCode)
        {
            try
            {
                //用户未指定渠道，系统默认选择创建时间最老的渠道拼串请求
                List<int> quoteSourceGroup = SourceGroupAlgorithm.ParseOldSource(quoteGroup);
                //过滤以后的多渠道串
                List<MultiChannels> newmodels = new List<MultiChannels>();
                //如果前端传multiChannels时，做校验并过滤
                if (!string.IsNullOrWhiteSpace(multiChannels))
                {
                    List<MultiChannels> models = multiChannels.FromJson<List<MultiChannels>>();
                    foreach (var model in models)
                    {
                        if (quoteSourceGroup.Contains((Int32)model.Source))
                        {
                            newmodels.Add(model);
                        }
                    }
                }
                //前端没传多渠道时，默认取可用渠道的第一次创建的渠道
                if (string.IsNullOrWhiteSpace(multiChannels))
                {
                    multiChannels = _getMultiChannels.GetStrMultiChannels(agent, buid, quoteSourceGroup, cityCode);
                }
                //解析前端传的多渠道处理
                if (newmodels.Any())
                {
                    _updateAgentSelect.UpdateAgentSelectMethod(newmodels.ToJson(), buid);
                    _updateQuoteManySource.UpdateQuoteManySourceMethod(childAgent, agent, "", newmodels);
                }
                else if (!string.IsNullOrWhiteSpace(multiChannels))
                {
                    _updateAgentSelect.UpdateAgentSelectMethod(multiChannels, buid);
                    _updateQuoteManySource.UpdateQuoteManySourceMethod(childAgent, agent, multiChannels, null);
                }
            }
            catch (Exception ex)
            {
                logError.Info("处理多渠道发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
        }
    }
}
