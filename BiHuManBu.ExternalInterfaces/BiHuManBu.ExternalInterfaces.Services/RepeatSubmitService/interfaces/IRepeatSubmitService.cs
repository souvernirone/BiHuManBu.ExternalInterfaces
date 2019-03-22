using System.Collections.Generic;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.interfaces
{
    public interface IRepeatSubmitService
    {
        Task<GetRepeatSubmitResponse> GetRepeatSubmitInfo(GetRepeatSubmitRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);
    }
    //public interface IRepeatSubmitInnerService
    //{
    //    Task<GetRepeatSubmitResponse> GetRepeatSubmitInfo(GetRepeatSubmitRequest request,
    //        IEnumerable<KeyValuePair<string, string>> pairs);
    //}
}