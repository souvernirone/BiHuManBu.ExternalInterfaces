using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class MessageListResponse:BaseResponse
    {
        public int TotalCount { get; set; }
        public List<BxMessage> MsgList { get; set; }
    }
}
