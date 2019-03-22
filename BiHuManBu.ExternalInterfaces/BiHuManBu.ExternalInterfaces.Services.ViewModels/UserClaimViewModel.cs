namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class UserClaimViewModel
    {
        /// <summary>
        /// 结案时间
        /// </summary>
        public string EndcaseTime { set; get; }
        /// <summary>
        /// 出险时间
        /// </summary>
        public string LossTime { set; get; }
        /// <summary>
        /// 出险金额
        /// </summary>
        public double PayAmount { set; get; }
        //public string PayCompanyNo { set; get; }
        /// <summary>
        /// 出险公司名称
        /// </summary>
        public string PayCompanyName { set; get; }
        //public string CreateTime { set; get; }
    }
    public class UserClaimDetailViewModel
    {
        /// <summary>
        /// 结案时间
        /// </summary>
        public string EndcaseTime { set; get; }
        /// <summary>
        /// 出险时间
        /// </summary>
        public string LossTime { set; get; }
        /// <summary>
        /// 出险金额
        /// </summary>
        public double PayAmount { set; get; }
        //public string PayCompanyNo { set; get; }
        /// <summary>
        /// 出险公司名称
        /// </summary>
        public string PayCompanyName { set; get; }
        /// <summary>
        /// 出险类别，0=商业险，1=交强险
        /// </summary>
        public int PayType { get; set; }
        //public string CreateTime { set; get; }
    }
}