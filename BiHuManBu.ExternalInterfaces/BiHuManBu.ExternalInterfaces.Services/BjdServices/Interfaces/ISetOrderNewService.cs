using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface ISetOrderNewService
    {
        MyBaoJiaViewModel SetOrder(MyBaoJiaViewModel my, bx_userinfo userInfo);
    }
}
