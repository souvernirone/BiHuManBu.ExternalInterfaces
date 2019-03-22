using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public  class GetCarClaimResponse:BaseResponse
    {
        public List<bx_car_claims> List { get; set; } 
        public int TotalCount { get; set; }
        public int UsedCount { get; set; }
    }
}
