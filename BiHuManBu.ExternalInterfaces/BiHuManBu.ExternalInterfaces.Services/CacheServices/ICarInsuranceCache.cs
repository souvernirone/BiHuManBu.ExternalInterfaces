using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.CacheServices
{
    public interface ICarInsuranceCache
    {
        Task<GetReInfoResponse> GetReInfo(GetReInfoRequest request,long? buid = null);

        /// <summary>
        /// 从缓存中取续保的bussinessstatus
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <returns></returns>
        int GetReInfoStatus(string licenseNo);

        Task<GetPrecisePriceReponse> GetPrecisePrice(GetPrecisePriceRequest request);

        Task<GetSubmitInfoResponse> GetSubmitInfo(GetSubmitInfoRequest request);
    }
}
