using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetLastQuotedRequest :BaseRequest2
    {
        public int CityCode { get; set; }
    }
}
