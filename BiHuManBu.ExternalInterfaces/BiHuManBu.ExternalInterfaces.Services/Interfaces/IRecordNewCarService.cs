using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IRecordNewCarService
    {
        Task<BaseViewModel> RecordNewCar(RecordNewCarRequest request);
        Task<GetRecordNewCarViewModel> GetRecordNewCar(GetRecordNewCarRequest request);
    }
}
