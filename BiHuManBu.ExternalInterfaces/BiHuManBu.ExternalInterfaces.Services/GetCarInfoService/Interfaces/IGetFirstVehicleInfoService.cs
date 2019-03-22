using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces
{
    public interface IGetFirstVehicleInfoService
    {
        /// <summary>
        /// 获取新车车辆车型信息接口
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        Task<GetCarVehicleInfoResponse> GetNewCarVehicle(GetNewCarVehicleRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
