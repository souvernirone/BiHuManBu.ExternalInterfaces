using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces
{
    public interface IGetMoldNameFromCenter
    {
        Task<GetMoldNameResponse> GetMoldNameService(string carVin, string moldName, int agent, int cityCode);
    }
}
