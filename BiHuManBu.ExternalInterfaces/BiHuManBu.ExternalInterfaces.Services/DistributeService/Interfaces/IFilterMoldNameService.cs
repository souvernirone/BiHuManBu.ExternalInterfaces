namespace BiHuManBu.ExternalInterfaces.Services.DistributeService.Interfaces
{
    public interface IFilterMoldNameService
    {
        void FilterMoldName(string moldName, int cameraAgent, int agent, long buid, int citycode, string businessExpireDate, string forceExpireDate, string cameraId, bool isAdd, int repeatStatus, int roleTypestring, string custKey, int cityCode);
    }
}
