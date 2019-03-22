using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class CacheChannelMapper
    {
        public static ChannelStatusBaseModel ConverToChannelViewModel(this AgentCacheChannelModel model, int agent)
        {
            return new ChannelStatusBaseModel()
            {
                ChannelId = model.ChannelId,
                ChannelName = model.ChannelName,
                Agent = agent,
                ChannelStatus = model.IsUse,
                CityCode = model.City,
                ChannelStatusMessage = model.IsUse.ToEnumDescriptionString(typeof(EnumChannelStatus)),
                Source = SourceGroupAlgorithm.GetNewSource(model.Source),
                SourceName = model.Source.ToEnumDescriptionString(typeof(EnumSource))
            };
        }
    }
}
