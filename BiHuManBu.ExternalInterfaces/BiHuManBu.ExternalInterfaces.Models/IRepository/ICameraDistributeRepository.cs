using BiHuManBu.ExternalInterfaces.Models.DistributeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ICameraDistributeRepository
    {
        bx_camera_config GetCameraConfig(int childagent);
        int GetRoleTypeByAgentId(int agentId);
        bx_camera_blacklist GetCameraBlack(int agent, int childAgent, string LicenseNo);
        List<int> GetSonListByDb(int currentAgent, bool hasSelf = true);
        List<int> GetSonListByDbAsync(int currentAgent, bool hasSelf = true);
        List<bx_userinfo> GetUserinfoByLicenseAndAgent(long buid, string licenseNo, List<string> agentIds);
        string RemoveList(string userIds, int isTest, ref bool isSuccess);
        List<carMold> FindCarModel(int agentId);
        string Remove(int userId, int isTest, ref bool isSuccess);
        bool SetCarModelId(int userId, int carModlId);
        bool RevokeFiles(int userId);
        bool RevertBatchRenewalItem(int buId);
        bool UpdateUserExpandByBuid(string buids, int DeleteType, DateTime DeleteTime);
        bool DeleteUserinfo(long buid, string deleteAgentId);
        int Update(bx_userinfo model);
        bx_batchrenewal_item GetItemByBuId(long buId);
        bx_userinfo GetUserInfo(long id);
        Task<int> AddDistributedHistoryAsync(bx_distributed_history model);
        Task<int> AddCrmStepsAsync(bx_crm_steps model);
        int UpdateUserRenewalTypeAndDistributed(int buId, int reqRenewalType, int distributed, bool exitUserinfo = false, bool isDistributedUserInfo = false);
        bx_agent GetAgent(int agentId);
        int Add(bx_message bxMessage);
        long AddMsgIdx(bx_msgindex model);
        bx_agent_xgaccount_relationship GetXgAccount(int agentId);
        List<long> FindAgentIdBySealman(int agentId = 0);
        /// <summary>
        /// 获取摄像头 业务员请假信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<LeaveDate> FindSealmanLeave(int userId);
        List<long> FindSealmanLeave();
    }
}
