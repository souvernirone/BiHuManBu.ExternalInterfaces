using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.LogBuriedPoint.LogCollection;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces
{
    public interface IPackagingResponseService
    {
        Task<GetReInfoViewModel> GetViewModel(GetReInfoRequest request, GetReInfoResponse response, int viewCityCode, string viewCustkey, int topAgent, string absolutori, BHFunctionLog fucnLog, string traceId);
    }
}
