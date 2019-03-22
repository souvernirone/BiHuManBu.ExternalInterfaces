using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class BjdInfoViewModel
    {
        public bj_baodanxinxi Baodanxinxi { get; set; }

        public bj_baodanxianzhong Baodanxianzhong { get; set; }

        public bx_bj_union BjUnion { get; set; }
    }
}
