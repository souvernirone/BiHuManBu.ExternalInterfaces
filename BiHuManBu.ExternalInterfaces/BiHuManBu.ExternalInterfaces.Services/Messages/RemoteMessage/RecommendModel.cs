namespace BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage
{
    public class RecommendModel: CarRenewalInsuranceModel
    {
        /// <summary>
        /// 该类型总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 当前险种组合数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 当前险种组合占比
        /// </summary>
        public string Ratio { get; set; }
    }
}
