using BiHuManBu.ExternalInterfaces.Services.GetSubmitInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using log4net;
using ServiceStack.Text;
using System.Collections.Generic;
using System.Net;

namespace BiHuManBu.ExternalInterfaces.Services.GetSubmitInfoService.Implementations
{
    public class CheckRequestGetSubmitInfo: ICheckRequestGetSubmitInfo
    {
        private readonly IValidateService _validateService;
        private readonly IUserInfoValidateService _userInfoValidateService;
        private readonly ICheckQuoteValidService _checkQuoteValidService;
        private readonly ILog logError = LogManager.GetLogger("ERROR");
        public CheckRequestGetSubmitInfo(IValidateService validateService, IUserInfoValidateService userInfoValidateService, ICheckQuoteValidService checkQuoteValidService)
        {
            _validateService = validateService;
            _userInfoValidateService = userInfoValidateService;
            _checkQuoteValidService = checkQuoteValidService;
        }

        public BaseResponse CheckRequest(GetSubmitInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
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
            //校验：2报价基础信息
            UserInfoValidateRequest validateRequest = new UserInfoValidateRequest()
            {
                LicenseNo = request.LicenseNo,
                CustKey = request.CustKey,
                ChildAgent = request.ChildAgent == 0 ? request.Agent : request.ChildAgent,
                RenewalCarType = request.RenewalCarType
            };
            //校验2
            var validateResult = _userInfoValidateService.UserInfoValidate(validateRequest);
            if (validateResult.Item1.Status == HttpStatusCode.NotAcceptable)
            {
                response.Status = HttpStatusCode.Forbidden;
                response.ErrMsg = validateResult.Item1.ErrMsg;
                logError.Info(request.ToJson() + ";校验错误：" + response.ErrMsg);
                return response;
            }
            //校验3：核保数据
            baseResponse = _checkQuoteValidService.Validate(validateResult.Item2, request.SubmitGroup, 2);
            if (baseResponse.Status == HttpStatusCode.NotAcceptable)
            {
                response.Status = HttpStatusCode.Forbidden;
                response.ErrMsg = baseResponse.ErrMsg;
                logError.Info(request.ToJson() + ";校验错误：" + validateResult.Item2.Id + ":" + response.ErrMsg);
                return response;
            }
            response.ErrMsg = baseResponse.ErrMsg;
            return response;
        }
    }
}
