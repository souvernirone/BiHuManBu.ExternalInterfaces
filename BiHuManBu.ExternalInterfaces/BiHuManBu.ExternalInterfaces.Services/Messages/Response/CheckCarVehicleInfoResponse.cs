using System.Net;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public  class CheckCarVehicleInfoResponse
    {
        public HttpStatusCode Status { get; set; }
        public string CheckMessage { get; set; }
        public int CheckCode { get; set; }
        /// <summary>
        /// 行驶证车辆类型名称
        /// </summary>
        public string DriveLicenseCarTypeName { get; set; }

        /// <summary>
        /// 行驶证车辆类型值
        /// </summary>
        public string DriveLicenseCarTypeValue { get; set; }
        public string TypeName { get; set; }
        public string CarType { get; set; }
        public int BusinessStatus { get; set; }
        public string BusinessMessage { get; set; }
    }
}
