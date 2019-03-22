
using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class AgentMapper
    {
        public static AgentModelNew ConverToViewModel(this AgentModel agentModel)
        {
            AgentModelNew vm = new AgentModelNew();
            if (agentModel != null)
                vm = new AgentModelNew
                {
                    AgentLevel = agentModel.AgentLevel,
                    TopAgentId = agentModel.TopAgentId,
                    TopAgentName = agentModel.TopAgentName,
                    TopAgentMobile = agentModel.TopAgentMobile,
                    TotalTimes = agentModel.TotalTimes,
                    AvailTimes = agentModel.AvailTimes,
                    SmsAccount = agentModel.SmsAccount,

                    Id = agentModel.Id,
                    AgentName = agentModel.AgentName,
                    Mobile = agentModel.Mobile,
                    OpenId = agentModel.OpenId,
                    ShareCode = agentModel.ShareCode,
                    CreateTime = agentModel.CreateTime,
                    IsBigAgent = agentModel.IsBigAgent,
                    FlagId = agentModel.FlagId,
                    ParentAgent = agentModel.ParentAgent,
                    ParentName = agentModel.ParentName,
                    ParentMobile = agentModel.ParentMobile,
                    ParentRate = agentModel.ParentRate,
                    AgentRate = agentModel.AgentRate,
                    ReviewRate = agentModel.ReviewRate,
                    PayType = agentModel.PayType,
                    AgentGetPay = agentModel.AgentGetPay,
                    CommissionType = agentModel.CommissionType,
                    ParentShareCode = agentModel.ParentShareCode,
                    IsUsed = agentModel.IsUsed,
                    AgentAccount = agentModel.AgentAccount,
                    AgentPassWord = agentModel.AgentPassWord,
                    IsGenJin = agentModel.IsGenJin,
                    IsDaiLi = agentModel.IsDaiLi,
                    IsShow = agentModel.IsShow,
                    IsShowCalc = agentModel.IsShowCalc,
                    SecretKey = agentModel.SecretKey,
                    IsLiPei = agentModel.IsLiPei,
                    AgentType = agentModel.AgentType.HasValue ? agentModel.AgentType.Value : 0,
                    MessagePayType = agentModel.MessagePayType.HasValue ? agentModel.MessagePayType.Value : 0
                };
            return vm;
        }

        public static List<AgentProtoModel> ConverToViewModel(this List<bx_agent> agent)
        {
            List<AgentProtoModel> AgentList = new List<AgentProtoModel>();
            AgentProtoModel proto;
            if (agent != null)
            {
                foreach (var item in agent)
                {
                    proto = new AgentProtoModel();
                    proto.Id = item.Id;
                    proto.AgentName = item.AgentName;
                    proto.Mobile = item.Mobile;
                    proto.OpenId = item.OpenId;
                    proto.ShareCode = item.ShareCode;
                    proto.CreateTime = item.CreateTime;
                    proto.IsBigAgent = item.IsBigAgent;
                    proto.FlagId = item.FlagId;
                    proto.ParentAgent = item.ParentAgent;
                    proto.ParentRate = item.ParentRate;
                    proto.AgentRate = item.AgentRate;
                    proto.ReviewRate = item.ReviewRate;
                    proto.PayType = item.PayType;
                    proto.AgentGetPay = item.AgentGetPay;
                    proto.CommissionType = item.CommissionType;
                    proto.ParentShareCode = item.ParentShareCode;
                    proto.IsUsed = item.IsUsed;
                    proto.AgentAccount = item.AgentAccount;
                    proto.AgentPassWord = item.AgentPassWord;
                    proto.IsGenJin = item.IsGenJin;
                    proto.IsDaiLi = item.IsDaiLi;
                    proto.IsShow = item.IsShow;
                    proto.IsShowCalc = item.IsShowCalc;
                    proto.SecretKey = item.SecretKey;
                    proto.IsLiPei = item.IsLiPei;
                    proto.AgentType = item.AgentType;
                    proto.MessagePayType = item.MessagePayType;
                    AgentList.Add(proto);
                }
            }
            return AgentList;
        }

        public static List<ExpireAgent> ConverToExpireAgentViewModel(this List<bx_agent> list)
        {
            List<ExpireAgent> newlist = new List<ExpireAgent>();
            ExpireAgent newmodel = new ExpireAgent();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    newmodel = new ExpireAgent()
                    {
                        AgentId = item.Id,
                        AgentName = item.AgentName,
                        EndDate = (item.endDate ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    newlist.Add(newmodel);
                }
            }
            return newlist;
        }
    }
}
