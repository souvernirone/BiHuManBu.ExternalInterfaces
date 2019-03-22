using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class DriverLicenseTypeResponse : BaseResponse
    {
        public List<bx_drivelicense_cartype> List { get; set; }
    }
}
