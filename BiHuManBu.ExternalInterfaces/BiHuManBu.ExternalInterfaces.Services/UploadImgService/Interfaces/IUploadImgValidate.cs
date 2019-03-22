using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.UploadImgService.Interfaces
{
    public interface IUploadImgValidate
    {
        BaseViewModel Validate(UploadMultipleImgRequest request);
    }
}
