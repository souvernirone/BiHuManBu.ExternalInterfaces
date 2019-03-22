using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces
{
    public interface IValidateService
    {
        /// <summary>
        /// 代理人和SecCode参数校验
        /// 如果返回HttpStatusCode.Forbidden，则校验失败
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        BaseResponse Validate(BaseRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        BaseResponse ValidateAgent(BaseRequest request, bx_userinfo userinfo);
    }
}
