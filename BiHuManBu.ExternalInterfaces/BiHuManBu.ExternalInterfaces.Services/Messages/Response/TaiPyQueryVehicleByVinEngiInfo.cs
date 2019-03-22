using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class WaTaiPySubmitResponse : BaseResponse
    {
        public WaTaiPySubmitResponseInfo TaiPySubmitInfo;
    }
    public class WaTaiPySubmitResponseInfo
    {
        public string Random;
        //public string orderId;
        public string OrderNo;
        public string SessionId;
        public string engineNo { get; set; }
        public string carVin { get; set; }
        public string oriPurchasePrice;
        public string moldName;
        public string currentValue;
        public int IsNewTaiPyUser;
        public string registerDate;
        public double NonClaimDiscountRate;
        public int lastYearAccTimes;
        public double LastYearClaimAmount;
        public int lastYearClaimTimes;
        public string licenseOwner;
        /// <summary>
        /// 身份证号，只有太平洋老用户能取到
        /// </summary>
        public string InsuredIdentifyNo;
    }
    [Serializable]
    public class TaiPyQueryVehicleByVinEngiInfo
    {
        public string bodyType { get; set; }//3厢
        public string EmptyWeight { get; set; }
        public string engineCapacity { get; set; }
        public string engineDesc { get; set; }
        public string moldName { get; set; }//雪佛兰SGM7140MTB
        public double price { get; set; }
        public string rVehicleFamily { get; set; }//赛欧
        public string vehicleBrand { get; set; }//上海通用雪佛兰
    }
}
