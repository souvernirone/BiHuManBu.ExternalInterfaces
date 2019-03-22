using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface ISetBaseInfoHistoryService
    {
        MyBaoJiaViewModel SetBaseInfoHistory(MyBaoJiaViewModel my, bx_userinfo userInfo, bx_quotehistory_related relatedhistory);
    }
}
