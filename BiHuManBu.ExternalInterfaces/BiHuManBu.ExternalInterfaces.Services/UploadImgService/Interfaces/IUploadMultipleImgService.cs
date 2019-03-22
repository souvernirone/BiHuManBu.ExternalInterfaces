using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.UploadImgService.Interfaces
{
    public interface IUploadMultipleImgService
    {
        UploadMultipleViewModel UploadMultipleImg(UploadMultipleImgRequest request);
    }
}
