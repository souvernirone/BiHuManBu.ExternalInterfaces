using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.AgentConfigService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class CheckMultiChannels : ICheckMultiChannels
    {
        private readonly IAgentConfigByCityService _agentConfigByCityService;
        //private readonly IAgentSelectRepository _agentSelectRepository;
        public CheckMultiChannels(IAgentConfigByCityService agentConfigByCityService
            //,IAgentSelectRepository agentSelectRepository
            )
        {
            _agentConfigByCityService = agentConfigByCityService;
            //_agentSelectRepository = agentSelectRepository;
        }
        public BaseViewModel CheckMultiChannelsUsed(string multiChannels, int agent, int citycode, long quotegroup, out string newMultiChannels)
        {
            newMultiChannels = string.Empty;
            BaseViewModel viewModel = new BaseViewModel();
            //用户未指定渠道，系统默认选择创建时间最老的渠道拼串请求
            //渠道请求模型转换
            List<MultiChannels> models = multiChannels.FromJson<List<MultiChannels>>();//请求模型转换为渠道
            List<MultiChannels> newModels = new List<MultiChannels>();//转换成需要保存到库里的集合
            //取值判断在bx_agent_config是否有值
            if (models.Any())
            {
                #region 判断前端传的渠道是否在该顶级下可用
                List<long> listChannels = models.Select(o => o.ChannelId).ToList();
                var agentconfig = _agentConfigByCityService.GetAgentConfigByCity(agent, citycode);
                List<long> listconfig = agentconfig.Select(o => o.id).ToList();
                List<long> diff = GetDiffrent(listChannels, listconfig);
                if (diff.Any())
                {//如果不同的list集合里有值，说明有数据不一样，则返回错误
                    viewModel.BusinessStatus = -10000;
                    viewModel.StatusMessage = "输入参数错误，报价渠道不可用";
                    return viewModel;
                }
                #endregion
                #region 判断前端传的请求报价渠道是否与多渠道报价的数量匹配
                List<long> listSource = models.Select(o => o.Source).ToList();
                List<long> quoteSourceGroup = SourceGroupAlgorithm.ParseNewSource(quotegroup);
                diff = new List<long>();
                diff = GetDiffrent(quoteSourceGroup, listSource);
                if (diff.Any())
                {//如果不同的list集合里有值，说明有数据不一样，则返回错误
                    viewModel.BusinessStatus = -10000;
                    viewModel.StatusMessage = "输入参数错误，请求报价渠道的QuoteGroup值与多渠道报价的MultiChannels值不匹配";
                    return viewModel;
                }
                #endregion
                foreach (var item in models)
                {
                    MultiChannels newItem = new MultiChannels();
                    newItem = item;
                    newItem.Source = SourceGroupAlgorithm.GetOldSource(item.Source);
                    newModels.Add(newItem);
                }
                newMultiChannels = newModels.ToJson();
            }
            return viewModel;
        }

        /// <summary>
        /// 第一、第二个集合取分值
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        private List<long> GetDiffrent(List<long> list1, List<long> list2)
        {
            List<long> diff = new List<long>();
            foreach (long intItem in list1)
            {
                if (!list2.Contains(intItem))
                {
                    diff.Add(intItem);
                }
            }
            return diff;
        }
    }
}
