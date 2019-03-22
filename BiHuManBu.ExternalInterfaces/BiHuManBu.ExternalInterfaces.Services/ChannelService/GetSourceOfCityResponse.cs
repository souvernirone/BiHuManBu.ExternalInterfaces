using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.ChannelService
{
    public class GetSourceOfCityResponse : BaseResponse
    {
        public List<CityChannelItem> Result { get; set; }
    }
}