
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class TempInsuredResponse
    {
        public int BusinessStatus { get; set; }
        public string StatusMessage { get; set; }
        public List<TempInsuredInfo> tempUserInfo { get; set; }
    }
    public class TempInsuredInfo
    {
        /// <summary>
        /// bx_TempInsuredInfo.Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// bx_agent.Id
        /// </summary>
        public int AgentId { get; set; }
        public bool TempUserType { get; set; }
        public string TempUserName { get; set; }
        public string TempIdCardType { get; set; }
        public string TempIdCard { get; set; }
        public string TempUserMobile { get; set; }
        public string TempUserEmail { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        //public string LicenseNo { get; set; }
        public bool Deleted { get; set; }
        //public long BuId { get; set; }
        /// <summary>
        /// 临时投保人
        /// </summary>
        public int TagTypeTempUser { get; set; }
        //public int TagType { get; set; }
        /// <summary>
        /// 临时被保险人 标签类型：0：去年投保，1：临时被保险人，2：正常值
        /// </summary>
        public int TagTypeTempInsured { get; set; }
        ///// <summary>
        ///// 被保险人详细信息
        ///// </summary>
        //public TempInsuredDetailInfo DetailInfo { get; set; }
    }

    public class TempInsuredDetailInfo
    {
        /// <summary>
        /// 临时被保险人姓名
        /// </summary>
        public string InsuredName { get; set; }
        /// <summary>
        /// 临时被保险人证件类型
        /// </summary>
        public int InsuredType { get; set; }
        /// <summary>
        /// 被保险人证件号
        /// </summary>
        public string InsuredIdCard { get; set; }
        /// <summary>
        /// 被保险人电话
        /// </summary>
        public string InsuredMobile { get; set; }
        /// <summary>
        /// 被保险人邮箱
        /// </summary>
        public string Email { get; set; }

    }
}
