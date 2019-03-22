using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public interface IAddCrmStepsService
    {
        void AddCrmSteps(int agentId, string agentName, string mobile, string licenseNo, long newSource, double bizRate,
            double forceRate, int activityId, long buid, long bxid, int cityCode);
    }
}
