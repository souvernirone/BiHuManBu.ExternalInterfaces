using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetSpecialListRequest
    {
        public int Source { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int AgentId { get; set; }

        public string SecCode { get; set; }

        public int CityId { get; set; }
    }
}