using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class AgentUKeyMapper
    {
        public static List<ChannelStatusModel> ConverToViewModel(this List<CacheChannelModel> list, int agent)
        {
            List<ChannelStatusModel> view = new List<ChannelStatusModel>();
            if (list != null)
            {
                ChannelStatusModel model;
                foreach (var item in list)
                {
                    model = new ChannelStatusModel();
                    model.Agent = agent;
                    model.ChannelStatus = item.IsUse;
                    model.CityCode = item.City;
                    model.ChannelStatusMessage = item.IsUse.ToEnumDescriptionString(typeof(EnumChannelStatus));
                    model.Source = SourceGroupAlgorithm.GetNewSource(item.Source);
                    model.SourceName = item.Source.ToEnumDescriptionString(typeof(EnumSource));
                    view.Add(model);
                }

            }
            return view;
        }

        /// <summary>
        /// 获取报价渠道的时候、获取上次报价渠道
        /// </summary>
        /// <param name="list"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        public static List<MultiQuotedChannelsViewModel> ConverToViewModel(this List<AgentCacheChannelModel> list, int agent)
        {
            var view = new List<MultiQuotedChannelsViewModel>();
            if (list != null)
            {
                MultiQuotedChannelsViewModel model;
                foreach (var item in list)
                {
                    model = new MultiQuotedChannelsViewModel();
                    model.ChannelStatus = item.IsUse;
                    model.ChannelStatusMessage = item.IsUse.ToEnumDescriptionString(typeof(EnumChannelStatus));
                    model.Source = SourceGroupAlgorithm.GetNewSource(item.Source);
                    model.ChannelId = item.ChannelId;
                    model.ChannelName = item.ChannelName;
                    model.IsPaicApi = item.IsPaicApi.HasValue ? item.IsPaicApi.Value.ToString() : "0";
                    view.Add(model);
                }

            }
            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        public static List<ChannelStatusModel> ConverToChannelViewModel(this List<AgentCacheChannelModel> list, int showIsPaicApi, int agent)
        {
            List<ChannelStatusModel> view = new List<ChannelStatusModel>();
            if (list != null)
            {
                ChannelStatusModel model;
                foreach (var item in list)
                {
                    model = new ChannelStatusModel();
                    model.ChannelId = item.ChannelId;
                    model.ChannelName = item.ChannelName;
                    model.Agent = agent;
                    model.ChannelStatus = item.IsUse;
                    model.CityCode = item.City;
                    model.ChannelStatusMessage = item.IsUse.ToEnumDescriptionString(typeof(EnumChannelStatus));
                    model.Source = SourceGroupAlgorithm.GetNewSource(item.Source);
                    model.SourceName = item.Source.ToEnumDescriptionString(typeof(EnumSource));
                    if (showIsPaicApi == 1)
                    {
                        model.IsPaicApi = item.IsPaicApi.HasValue ? item.IsPaicApi.Value.ToString() : "0";
                    }
                    else
                    {
                        model.IsPaicApi = null;
                    }
                    view.Add(model);
                }

            }
            return view;
        }
    }
}
