using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.UploadImgService.Interfaces
{
    public interface ICheckUploadImgTimes
    {
        BaseViewModel ValidateTimes(UploadMultipleImgRequest request);
    }
}
