using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces
{
    public interface IGetSecondVehicleInfoService
    {
        /// <summary>
        /// 获取新车车辆车型信息接口第二步
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        Task<GetCarVehicleInfoResponse> GetSecondCarVehicle(GetNewCarSecondVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
