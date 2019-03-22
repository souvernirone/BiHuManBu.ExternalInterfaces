using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Services.AgentChannelService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class CacheChannelService//:IChannelService
    {
        private readonly IChannelService _channelService;
        private readonly ICacheHelper _cacheHelper;
        public CacheChannelService(IChannelService channelService, ICacheHelper cacheHelper)
        {
            _channelService = channelService;
            _cacheHelper = cacheHelper;
        }

        public GetChannelStatusResponse GetChannelStatus(GetChannelStatusRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var model= _channelService.GetChannelStatus(request, pairs);

            return model;
        }
    }
}
