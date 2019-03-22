using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetChannelStatusByUkeyIdRequest : BaseRequest
    {
        /// <summary>
        /// UekyId
        /// </summary>
        public int UkeyId { get; set; }
        /// <summary>
        /// 前端通过isurl，判断返回url还是macurl，传过来直接查缓存
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 取Id还是取Url  1是url 2不是
        /// </summary>
        public int IsId { get; set; } 
    }
}
