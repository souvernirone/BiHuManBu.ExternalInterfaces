using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface IGetDateService
    {
        string GetDate(bx_userinfo userInfo, out string postBizStartDate);
    }
}
