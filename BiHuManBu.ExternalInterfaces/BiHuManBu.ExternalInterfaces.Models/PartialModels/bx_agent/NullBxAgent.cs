using System;

namespace BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent
{
    public class NullBxAgent : IBxAgent
    {
        public int Id
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string AgentName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Mobile
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string OpenId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ShareCode
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DateTime? CreateTime
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? IsBigAgent
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? FlagId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ParentAgent
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public double? ParentRate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public double? AgentRate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public double? ReviewRate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? PayType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public decimal? AgentGetPay
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? CommissionType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ParentShareCode
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? IsUsed
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string AgentAccount
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string AgentPassWord
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? IsGenJin
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? IsDaiLi
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? IsShow
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? IsShowCalc
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string SecretKey
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? IsLiPei
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? AgentType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? MessagePayType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Region
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int BatchRenewalTotalCount
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ManagerRoleId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int BatchRenewalFrequency
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string AgentAddress
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int? RegType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int IsQuote
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int IsSubmit
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int TopAgentId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 0试用账号1付费账号
        /// </summary>
        public int accountType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 试用账号到期时间
        /// </summary>
        public DateTime? endDate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 付费账号到期时间
        /// </summary>
        public DateTime? contractEndDate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 是否允许重复报价
        /// </summary>
        public int repeat_quote {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public bool AgentCanUse()
        {
            return false;
        }

        public bool AgentCanQuote()
        {
            return false;
        }

        public bool AgentCanSubmit()
        {
            return false;
        }
    }
}