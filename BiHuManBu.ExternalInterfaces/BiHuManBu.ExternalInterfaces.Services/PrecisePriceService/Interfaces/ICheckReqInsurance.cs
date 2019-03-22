using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces
{
    public interface ICheckReqInsurance
    {
        /// <summary>
        /// 校验险种相关信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BaseViewModel CheckInsurance(PostPrecisePriceRequest request);
    }
}
