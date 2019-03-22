using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces
{
    public interface ICheckQuoteValidService
    {
        /// <summary>
        /// 校验该数据的报价或核保状态是否正常
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="source"></param>
        /// <param name="quoteType">1报价2核保</param>
        /// <returns></returns>
        BaseResponse Validate(bx_userinfo userinfo, long source, int quoteType);
    }
}
