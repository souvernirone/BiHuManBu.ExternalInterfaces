
namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetParaBhTypeRequest:BaseRequest
    {
        /// <summary>
        /// 要取的类型：1=证件类型，2=能源类型，3=车辆来历凭证，4=号牌底色，5=条款类型，6=号牌种类，7=使用性质，8=行驶区域，9=车辆种类
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 是否调用所有的数据，1所有，0只取is_support=1的数据
        /// </summary>
        public int IsAll { get; set; }
    }
}
