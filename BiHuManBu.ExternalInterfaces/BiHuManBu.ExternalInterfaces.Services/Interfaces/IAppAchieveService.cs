using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IAppAchieveService
    {
        /// <summary>
        /// 请求续保
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<GetReInfoNewViewModel> GetReInfo(GetReInfoRequest request,IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);

        /// <summary>
        /// 请求报价/核保
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<BaseViewModel> PostPrecisePrice(PostPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);

        /// <summary>
        /// 获取报价信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<GetPrecisePriceNewViewModel> GetPrecisePrice(GetPrecisePriceRequest request,IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);

        /// <summary>
        /// 获取核保信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<SubmitInfoNewViewModel> GetSubmitInfo(GetSubmitInfoRequest request,IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);

        /// <summary>
        /// 获取车辆出险信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<GetCreaditInfoViewModel> GetCreditInfo(GetEscapedInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);
        /// <summary>
        /// 获取车辆出险信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<CarVehicleInfoNewViewModel> GetVehicleInfo(GetCarVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);
        /// <summary>
        /// 获取车辆出险信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<CheckCarVehicleInfoViewModel> CheckVehicle(GetNewCarSecondVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);
        
        /// <summary>
        /// 续保报价列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        MyListViewModel GetMyList(GetMyListRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        /// <summary>
        /// 获取报价单列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        MyBaoJiaViewModel GetPrecisePriceDetail(GetMyBjdDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);

        /// <summary>
        /// 获取续保列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        GetReInfoNewViewModel GetReInfoDetail(GetReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs,Uri uri);

        /// <summary>
        /// 分享我的报价单
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        BaseViewModel SharePrecisePrice(CreateOrUpdateBjdInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);

        /// <summary>
        /// 查看分享的报价单
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        BaojiaItemViewModel GetShare(GetBjdItemRequest request, IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);

        /// <summary>
        /// 添加回访记录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        BaseViewModel AddReVisited(AddReVisitedRequest request,IEnumerable<KeyValuePair<string, string>> pairs);

        /// <summary>
        /// 回访记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        ReVisitedListViewModel ReVisitedList(ReVisitedListRequest request,IEnumerable<KeyValuePair<string, string>> pairs);
        
        /// <summary>
        /// 获取顶级代理的渠道资源
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        AppAgentSourceViewModel GetAgentSource(AppBaseRequest request,IEnumerable<KeyValuePair<string, string>> pairs, Uri uri);
    }
}
