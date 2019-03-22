using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface ICarInsuranceService
    {
        /// <summary>
        /// 获取续保信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<GetReInfoResponse> GetReInfo(GetReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        /// <summary>
        /// 获取核保信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<GetSubmitInfoResponse> GetSubmitInfo(GetSubmitInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        /// <summary>
        /// 报价，插入险种信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        Task<PostPrecisePriceResponse> InsertUserInfo(PostPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        /// <summary>
        /// 报价，更新行驶证信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PostPrecisePriceResponse> UpdateDrivingLicense(PostPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs);


        Task<PostPrecisePriceResponse> UpdateDrivingLicenseAgain(PostPrecisePriceRequestAgain request, IEnumerable<KeyValuePair<string, string>> pairs);
        /// <summary>
        /// 获取报价
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        Task<GetPrecisePriceReponse> GetPrecisePrice(GetPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        /// <summary>
        /// 返回用户行驶证信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        Task<GetLicenseResponse> GetVenchileLincenseInfo(GetReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        /// <summary>
        /// 返回用户续保信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        Task<GetReInfoResponse> GetRenewalInfo(GetReInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        /// <summary>
        /// 取消核保功能
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        Task<GetCancelSubmitResponse> GetCancelSubmit(GetCancelSubmitRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        /// <summary>
        /// 获取车辆车型信息接口
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        Task<GetCarVehicleInfoResponse> GetCarVehicle(GetCarVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        
        /// <summary>
        /// 报价车型校验
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        Task<CheckCarVehicleInfoResponse> CheckCarVehicle(GetNewCarSecondVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        Task<GetMoldNameResponse> GetMoldName(GetMoldNameRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        void Test(int num);

        /// <summary>
        /// 续保调用第三方接口传数据
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="viewModel"></param>
        void PostThirdPart(int agent, ViewModels.GetReInfoViewModel viewModel);

        Task<GetRepeatSubmitResponse> GetRepeatSubmitInfo(GetRepeatSubmitRequest request, IEnumerable<KeyValuePair<string, string>> pairs);

        //void AddSpecialOptionList(PostPrecisePriceRequest request);
    }
}
