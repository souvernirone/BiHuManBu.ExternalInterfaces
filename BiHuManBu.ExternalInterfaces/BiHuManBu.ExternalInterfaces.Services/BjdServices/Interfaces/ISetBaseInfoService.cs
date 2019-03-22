using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface ISetBaseInfoService
    {
        MyBaoJiaViewModel SetBaseInfo(MyBaoJiaViewModel my, bx_userinfo userInfo);
    }
}
