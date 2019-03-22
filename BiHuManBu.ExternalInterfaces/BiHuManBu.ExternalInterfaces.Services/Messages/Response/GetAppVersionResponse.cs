
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetAppVersionResponse:BaseResponse
    {
        public bx_appsetting AppSetting { get; set; }
    }
}
