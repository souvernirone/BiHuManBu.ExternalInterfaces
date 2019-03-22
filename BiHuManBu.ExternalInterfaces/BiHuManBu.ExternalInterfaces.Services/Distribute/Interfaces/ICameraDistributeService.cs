using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.DistributeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Distribute.Interfaces
{
    public interface ICameraDistributeService
    {
        void Distribute(CameraDistributeModel request);
        List<string> GetSonsListFromRedisToString(int currentAgentId, bool isHasSelf = true);
        List<string> GetSonsListFromRedisToStringAsync(int currentAgentId, bool isHasSelf = true);
        int GetModelFilterId(int agentId, string carModelKey);
        long GerRedirsSealman(int agentId, long index = 0, long _index = 0);
        bool IsInTime(int cityCode, string businessExpireDate, string forceExpireDate, int cameraAgent, out int endDays, out int isRemind);
        long AddNoticexb(bx_userinfo userInfo, int cameraAgent, int isRemind, int endDays);
        bool IsAdmin(int agentId);
    }
}
