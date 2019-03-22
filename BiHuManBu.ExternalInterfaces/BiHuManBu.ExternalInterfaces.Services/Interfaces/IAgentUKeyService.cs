using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IAgentUKeyService
    {
        UKeyListResponse GetUKeyList(GetUKeyListRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        BaseResponse EditAgentUKey(EditAgentUKeyRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
        BaseResponse EditBackupPwd(EditBackupPwdRequest request);
        /// <summary>
        /// 根据UKId获取城市Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int GetAgentCityCodeByUKId(int Id);
    }
}
