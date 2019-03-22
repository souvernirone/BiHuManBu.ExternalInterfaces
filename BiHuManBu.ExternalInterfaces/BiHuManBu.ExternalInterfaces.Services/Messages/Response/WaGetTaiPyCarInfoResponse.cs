using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    [Serializable]
    public class WaGetTaiPyCarInfoResponse
    {
        public TaiPyQueryVehicleByVinEngiInfo CarInfo { get; set; }
    }
}
