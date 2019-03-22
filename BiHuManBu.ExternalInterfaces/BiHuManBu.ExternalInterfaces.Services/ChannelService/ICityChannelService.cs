using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ChannelService
{
    public interface ICityChannelService
    {
        GetSourceOfCityResponse GetSourceOfCity(GetSourceOfCityRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}