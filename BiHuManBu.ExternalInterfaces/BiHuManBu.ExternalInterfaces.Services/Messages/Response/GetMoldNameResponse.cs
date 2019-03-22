using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{

    public class WaPaAutoModelResponse : WaBaseResponse
    {
        public object c51DiscountTaxMap { get; set; }
        public PaAutoModeType AutoModeType { get; set; }
    }

    public class PaAutoModeType
    {
        public string autoModelName { get; set; }
    }

    public class GetMoldNameResponse :BaseResponse
    {
        public string MoldName { get; set; }
        public int BusinessStatus { get; set; }
        public string BusinessMessage { get; set; }
    }
}
