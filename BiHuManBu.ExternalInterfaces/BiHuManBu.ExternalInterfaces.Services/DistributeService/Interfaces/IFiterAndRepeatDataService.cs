using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Services.DistributeService.Interfaces
{
    public interface IFiterAndRepeatDataService
    {

        CameraBackDataViewModel GetFiterData(string licenseno, int agent, int childagent, string custkey, int citycode, int renewalcartype, int repeatStatus, int roleType);
        long GetFiterDataByCarVin(int agent, int childAgent, int repeatStatus, int roleType, string carVIN, string cameraId, string custKey, int cityCode);
    }
}
