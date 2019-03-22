using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class MessageMapper
    {
        public static List<BxMessage> ConvertToViewModel(this List<bx_message> msgList)
        {
            List<BxMessage> list = new List<BxMessage>();
            BxMessage msg;
            foreach (var item in msgList)
            {
                msg = new BxMessage();
                msg.Id = item.Id;
                msg.MsgLevel = item.Msg_Level.HasValue ? item.Msg_Level.Value : 0;
                msg.MsgStatus = item.Msg_Status.HasValue ? item.Msg_Status.Value : 0;
                msg.MsgType = item.Msg_Type;
                msg.SendTime = item.Send_Time.HasValue ? item.Send_Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                msg.Title = item.Title;
                msg.Body = item.Body;
                msg.UpdateTime = item.Update_Time.HasValue ? item.Update_Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                msg.Url = item.Url;
                msg.AgentId = item.Agent_Id.HasValue ? item.Agent_Id.Value : 0;
                msg.CreateAgentId = item.Create_Agent_Id;
                msg.CreateTime = item.Create_Time.HasValue ? item.Create_Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                list.Add(msg);
            }
            return list;
        }

        public static List<BxMessage> ConvertToViewModel(this List<TableMessage> msgList)
        {
            List<BxMessage> list = new List<BxMessage>();
            BxMessage msg;
            foreach (var item in msgList)
            {
                msg = new BxMessage();
                msg.StrId = item.StrId;
                msg.Id = item.Id;
                msg.MsgLevel = item.Msg_Level;
                msg.MsgStatus = item.Msg_Status;
                msg.MsgType = item.Msg_Type;
                msg.SendTime = item.SendTime;
                msg.Title = item.Title;
                msg.Body = item.Body;
                msg.UpdateTime = item.Update_Time;
                msg.Url = item.Url;
                msg.AgentId = item.Agent_Id;
                msg.AgentName = item.AgentName;
                msg.CreateAgentId = item.Create_Agent_Id;
                msg.CreateAgentName =item.CreateAgentName;
                msg.CreateTime = item.Create_Time.ToString("yyyy-MM-dd HH:mm:ss");
                msg.Buid = item.Buid.HasValue?item.Buid.Value:0;
                msg.LicenseNo = item.LicenseNo;
                msg.LastForceEndDate = item.Last_Force_End_Date;
                msg.LastBizEndDate = item.Last_Biz_End_Date;
                msg.NextForceStartDate =item.Last_Force_End_Date;
                msg.NextBizStartDate = item.Last_Biz_End_Date;
                msg.Source = item.Source.HasValue?item.Source.Value:0;
                msg.Days = item.Days.HasValue?item.Days.Value:0;
                msg.OwnerAgent = item.OwnerAgent.HasValue?item.OwnerAgent.Value:0;
                list.Add(msg);
            }
            return list;
        }
    }
}
