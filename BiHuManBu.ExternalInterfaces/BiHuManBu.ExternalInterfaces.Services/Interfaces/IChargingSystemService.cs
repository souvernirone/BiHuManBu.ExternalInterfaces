using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IChargingSystemService
    {
        /// <summary>
        /// 获取续保列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        ReListViewModel GetReInfoList(GetReInfoListRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        AppGetReInfoResponse GetReInfoDetail(GetReInfoDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
