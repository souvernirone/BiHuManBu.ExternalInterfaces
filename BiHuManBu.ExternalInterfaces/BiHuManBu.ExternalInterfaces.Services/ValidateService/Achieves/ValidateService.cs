using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.ValidateService.Achieves
{
    public class ValidateService : IValidateService
    {
        private readonly IGetAgentInfoService _getAgentInfoService;
        private readonly ICheckGetSecCode _checkGetSecCode;
        public ValidateService(IGetAgentInfoService getAgentInfoService, ICheckGetSecCode checkGetSecCode)
        {
            _getAgentInfoService = getAgentInfoService;
            _checkGetSecCode = checkGetSecCode;
        }

        public BaseResponse Validate(BaseRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            BaseResponse response = new BaseResponse();
            IBxAgent agentModel = _getAgentInfoService.GetAgentModelFactory(request.Agent);
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                if (agentModel.endDate.HasValue && agentModel.endDate.Value < DateTime.Now)
                {
                    response.ErrMsg = string.Format("参数校验错误，账号已过期。过期时间为：{0}", agentModel.endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));                    
                    return response;
                }
                response.ErrMsg = "参数校验错误，账号已禁用。";
                return response;
            }
            if (!_checkGetSecCode.ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                response.ErrMsg = "参数校验错误，SecCode校验出错。";
                return response;
            }
            return response;
        }
        public BaseResponse ValidateAgent(BaseRequest request, bx_userinfo userinfo)
        {
            BaseResponse response = new BaseResponse();
            IBxAgent curagentModel = _getAgentInfoService.GetAgentModelFactory(int.Parse(userinfo.Agent));
            //由于前端传的是当前用户和顶级，故此处只判断顶级，否则部分数据可能显示不了
            if (curagentModel.Id == 0 || curagentModel.TopAgentId != request.Agent)
            {
                response.Status = HttpStatusCode.Forbidden;
                response.ErrMsg = "请求数据无权限";
                return response;
            }
            return response;
        }
    }
}
