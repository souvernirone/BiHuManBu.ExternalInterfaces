
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetParaBhTypeResponse:BaseResponse
    {
        public int Total { get; set; }
        public List<bx_para_bhtype> ParaBhTypeList { get; set; }
    }
}
