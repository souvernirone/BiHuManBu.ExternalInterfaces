using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using System;
using System.Net;

namespace BiHuManBu.ExternalInterfaces.Services.ValidateService.Achieves
{
    public class CheckQuoteValidService : ICheckQuoteValidService
    {
        /// <summary>
        /// 校验该数据的报价或核保状态是否正常
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="source"></param>
        /// <param name="quoteType">1报价2核保</param>
        /// <returns></returns>
        public BaseResponse Validate(bx_userinfo userinfo, long source, int quoteType)
        {
            BaseResponse response = new BaseResponse();
            if (userinfo != null)
            {
                if (!userinfo.UpdateTime.HasValue)
                {
                    response.Status = HttpStatusCode.NotAcceptable;
                    response.ErrMsg = "上次报价/核保值缓存到当天23:59分，请重新请求报价/核保再获取详情。";
                    return response;
                }
                if (userinfo.UpdateTime.Value.Date != DateTime.Now.Date)
                {
                    response.Status = HttpStatusCode.NotAcceptable;
                    response.ErrMsg = "上次报价/核保值缓存到当天23:59分，请重新请求报价/核保再获取详情。";
                    return response;
                }
                if (quoteType == 1)
                {
                    if ((userinfo.IsSingleSubmit & source) == 0)
                    {
                        response.Status = HttpStatusCode.NotAcceptable;
                        response.ErrMsg = "上次报价没有报价该渠道，请核实QuoteGroup。";
                        return response;
                    }
                }
                //临时注释掉，需要单独提测，平安有特殊的逻辑处理
                else if (quoteType == 2)
                {
                    if (source != 2 && ((userinfo.Source & source) == 0))
                    {
                        response.Status = HttpStatusCode.NotAcceptable;
                        response.ErrMsg = "上次核保没有核保该渠道，请核实SubmitGroup。";
                        return response;
                    }
                }
            }
            return response;
        }
    }
}
