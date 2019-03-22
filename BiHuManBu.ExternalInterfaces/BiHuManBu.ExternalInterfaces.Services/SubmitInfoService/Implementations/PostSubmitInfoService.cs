using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Implementations
{
    public class PostSubmitInfoService : IPostSubmitInfoService
    {
        private readonly IValidateService _validateService;
        //private readonly ILog _logInfo = LogManager.GetLogger("INFO");
        //private readonly ILog _logError = LogManager.GetLogger("ERROR");
        private readonly IMessageCenter _messageCenter;
        private readonly IRemoveHeBaoKey _removeHeBaoKey;
        private readonly IPostValidate _postValidate;
        public PostSubmitInfoService(IValidateService validateService, IMessageCenter messageCenter, IRemoveHeBaoKey removeHeBaoKey, IPostValidate postValidate)
        {
            _validateService = validateService;
            _messageCenter = messageCenter;
            _removeHeBaoKey = removeHeBaoKey;
            _postValidate = postValidate;
        }

        public PostSubmitInfoResponse PostSubmitInfo(PostSubmitInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            PostSubmitInfoResponse response = new PostSubmitInfoResponse();
            //校验：1基础校验
            BaseResponse baseResponse = _validateService.Validate(request, pairs);
            if (baseResponse.Status == HttpStatusCode.Forbidden)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //校验2
            var validateResult = _postValidate.SubmitInfoValidate(request);
            if (validateResult.Item1.Status == HttpStatusCode.NotAcceptable)
            {
                response.Status = HttpStatusCode.NotAcceptable;
                return response;
            }
            //实现
            //清理缓存
            string baojiaCacheKey = string.Empty;
            try
            {
                baojiaCacheKey = _removeHeBaoKey.RemoveHeBao(request);
            }
            catch (RedisOperateException exception)
            {
                response.Status = HttpStatusCode.ExpectationFailed;
                response.ErrMsg = exception.Message;
                return response;
            }
            //中心传的商业险投保单号赋值
            string strBizNo = string.Empty;
            //中心传的商业险投保单号赋值
            string strForceNo = string.Empty;
            if (request.Source == 4|| request.Source == 1)
            {
                //人保、太保用tno
                strBizNo = validateResult.Item3.biz_tno ?? "";
                strForceNo = validateResult.Item3.force_tno ?? "";
            }
            else
            {
                //非人保、非太保的用pno
                strBizNo = validateResult.Item3.biz_pno ?? "";
                strForceNo = validateResult.Item3.force_pno ?? "";
            }
            //通知中心
            var msgBody = new
            {
                B_Uid = validateResult.Item2.Id,
                Source = SourceGroupAlgorithm.GetOldSource(request.Source),
                BiztNo = strBizNo,
                ForcetNo = strForceNo,
                LicenseNo = validateResult.Item2.LicenseNo,
                NotifyCacheKey = baojiaCacheKey
            };
            //发送重新核保消息
            try
            {
                var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                    ConfigurationManager.AppSettings["MessageCenter"],
                    ConfigurationManager.AppSettings["BxHeBaoAgainName"]);
            }
            catch (MessageException exception)
            {
                response.Status = HttpStatusCode.ExpectationFailed;
                response.ErrMsg = exception.Message;
                return response;
            }
            response.Status = HttpStatusCode.OK;
            return response;
        }
    }
}
