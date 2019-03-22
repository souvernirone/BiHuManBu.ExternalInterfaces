
using System.Collections.Generic;
using System.Net;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetCarVehicleInfoResponse
    {
        public List<BxCarVehicleInfo> Vehicles { get; set; }
        public HttpStatusCode Status { get; set; }
        public int BusinessStatus { get; set; }
        public string BusinessMessage { get; set; }
    }
    public class GetNewCarVehicleInfoResponse
    {
        public List<NewBxCarVehicleInfo> Vehicles { get; set; }
        public HttpStatusCode Status { get; set; }
        public int BusinessStatus { get; set; }
        public string BusinessMessage { get; set; }
    }
}
