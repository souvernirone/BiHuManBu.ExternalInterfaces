using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IBjdService
    {

        long UpdateBjdInfo(CreateOrUpdateBjdInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        GetBjdItemResponse GetBjdInfo(GetBjdItemRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        List<MyBaoJiaViewModel> GetMyList(GetMyBjdListRequest request, IEnumerable<KeyValuePair<string, string>> pairs, out int TotalCount);
        MyBaoJiaViewModel GetMyBjdDetail(GetMyBjdDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        /// <summary>
        /// 从历史获取我的报价单详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        MyBaoJiaViewModel GetBjdDetailFromHistory(GetBjdDetailFromHistoryRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        ReInfoListViewModel GetReInfoList(GetReInfoListRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);

        AppGetReInfoResponse GetReInfoDetail(GetReInfoDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        /// <summary>
        /// 统计报价数量、预约单数量、已出单数量
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        BjdCountInfoViewModel BjdCountInfo(BjdCountInfoRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs);

        MyListViewModel GetMyList(GetMyListRequest request);
    }
}
