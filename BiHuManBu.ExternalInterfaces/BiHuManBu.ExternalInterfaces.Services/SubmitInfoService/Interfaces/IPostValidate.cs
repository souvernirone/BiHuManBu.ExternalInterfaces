using System;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Interfaces
{
    public interface IPostValidate
    {
        Tuple<BaseResponse, bx_userinfo, bx_submit_info> SubmitInfoValidate(PostSubmitInfoRequest request);
    }
}
