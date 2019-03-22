
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetChannelStatusRequest:BaseRequest
    {
        [Range(0,10000)]
        public int CityCode { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        [Range(0,10000)]
        public long ChannelId { get; set; }

        /// <summary>
        /// 是否展示平安接口标识1展示0不展示
        /// </summary>
        public int ShowIsPaicApi { get; set; }
    }
}
