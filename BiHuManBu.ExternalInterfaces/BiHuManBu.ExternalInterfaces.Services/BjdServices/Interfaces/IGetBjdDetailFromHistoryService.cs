using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface IGetBjdDetailFromHistoryService
    {
        MyBaoJiaViewModel GetMyBjdDetail(GetBjdDetailFromHistoryRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
