using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using log4net;
using ServiceStack.Text;
using System.Collections.Generic;
using System.Net;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class CheckRequestReInfo : ICheckRequestReInfo
    {
        private readonly IValidateService _validateService;
        private readonly ILog logError = LogManager.GetLogger("ERROR");

        public BaseResponse CheckRequest(GetPrecisePriceRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            BaseResponse response = new BaseResponse();
            response.Status = HttpStatusCode.OK;
            //校验：1基础校验
            BaseResponse baseResponse = _validateService.Validate(request, pairs);
            if (baseResponse.Status == HttpStatusCode.Forbidden)
            {
                response.Status = HttpStatusCode.Forbidden;
                response.ErrMsg = baseResponse.ErrMsg;
                logError.Info(request.ToJson() + ";校验错误：" + response.ErrMsg);
                return response;
            }
            return response;
        }
    }
}
