using System;
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.ChannelService
{
    public class CacheCityChannelService : ICityChannelService
    {
        private ICityChannelService _channelService;
        private ICacheService _cacheService;

        public CacheCityChannelService(ICityChannelService channelService,ICacheService cacheService)
        {
            _channelService = channelService;
            _cacheService = cacheService;
        }

        public GetSourceOfCityResponse GetSourceOfCity(GetSourceOfCityRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetSourceOfCityResponse response = new GetSourceOfCityResponse()
            {
               Result = new List<CityChannelItem>()
            };
            var cacheValue =
                _cacheService.Get<GetSourceOfCityResponse>(string.Format("{0}_channels_sets", request.CityCode));
            if (cacheValue != null)
            {
                response = cacheValue;
            }
            else
            {
                var cacheExpireTimeSeconds = string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["cacheExpireTimeSeconds"]) ? 1800 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["cacheExpireTimeSeconds"]);
                response = _channelService.GetSourceOfCity(request, pairs);
                _cacheService.Set(string.Format("{0}_channels_sets", request.CityCode), response.ToJson(), cacheExpireTimeSeconds);
            }
            return response;
        }
    }
}