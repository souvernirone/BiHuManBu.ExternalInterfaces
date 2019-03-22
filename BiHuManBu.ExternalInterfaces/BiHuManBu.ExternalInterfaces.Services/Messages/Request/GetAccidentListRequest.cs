namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetAccidentListRequest:BaseRequest
    {
        /// <summary>
        /// 取哪家保司
        /// </summary>
        public long Source { get; set; }
        /// <summary>
        /// 城市Id
        /// </summary>
        public int CityCode { get; set; }
    }
}
