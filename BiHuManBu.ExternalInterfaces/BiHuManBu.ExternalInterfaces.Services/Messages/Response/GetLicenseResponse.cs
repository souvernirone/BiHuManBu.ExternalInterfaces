using System.Net;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public  class GetLicenseResponse:BaseResponse
    {
        public bx_userinfo UserInfo { get; set; }
        public bx_carinfo Carinfo { get; set; }
        public int BusinessStatus { get; set; }
        public string BusinessMessage { get; set; }
    }
}
