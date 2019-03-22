using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces
{
    public interface IReWriteUserInfo
    {
        UserInfoViewModel ReWriteUserInfoService(UserInfoViewModel userinfo, int topAgentId);
    }
}
