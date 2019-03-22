using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.GetPrecisePriceService.Interfaces
{
    public interface ICheckRequestGetPrecisePrice
    {
        /// <summary>
        /// 获取报价校验
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        BaseResponse CheckRequest(GetPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
