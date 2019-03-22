using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces
{
    public interface IUserInfoValidateService
    {
        Tuple<BaseResponse, bx_userinfo> UserInfoValidate(UserInfoValidateRequest request);
    }
}
