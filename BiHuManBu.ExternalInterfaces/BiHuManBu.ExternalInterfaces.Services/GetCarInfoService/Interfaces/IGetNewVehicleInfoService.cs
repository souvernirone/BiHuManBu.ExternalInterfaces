using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces
{
    public interface IGetNewVehicleInfoService
    {
        /// <summary>
        /// 获取车辆车型信息接口,第二版
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        Task<GetNewCarVehicleInfoResponse> GetCarVehicle(GetVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
