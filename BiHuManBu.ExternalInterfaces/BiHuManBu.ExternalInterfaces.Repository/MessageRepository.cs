using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;
using MySql.Data.MySqlClient;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");

        #region bx_message
        public int Add(bx_message bxMessage)
        {
            int workOrderId = 0;
            try
            {
                var t = DataContextFactory.GetDataContext().bx_message.Add(bxMessage);
                DataContextFactory.GetDataContext().SaveChanges();
                workOrderId = t.Id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                workOrderId = 0;
            }
            return workOrderId;
        }

        public int Update(bx_message bxMessage)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_message.AddOrUpdate(bxMessage);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }
        public bx_message Find(int msgId)
        {
            bx_message bxMessage = new bx_message();
            try
            {
                bxMessage = DataContextFactory.GetDataContext().bx_message.FirstOrDefault(i => i.Id == msgId && i.Msg_Status == 0);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return bxMessage;
        }
        public bx_message FindById(int msgId)
        {
            bx_message bxMessage = new bx_message();
            try
            {
                bxMessage = DataContextFactory.GetDataContext().bx_message.FirstOrDefault(i => i.Id == msgId);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return bxMessage;
        }

        public List<bx_message> FindMsgList(int agentId, int pageSize, int curPage, out int total)
        {
            List<bx_message> list = new List<bx_message>();
            try
            {
                list = DataContextFactory.GetDataContext().bx_message.Where(i => i.Agent_Id == agentId && i.Msg_Status == 0).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            total = list.Count;
            return list.OrderByDescending(o => o.Msg_Status).ThenByDescending(o => o.Update_Time).Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList();
        }
        /// <summary>
        /// 获取所有未读的msg表记录，未排序，消息列表用
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<bx_message> FindNoReadList(int agentId, out int total)
        {
            List<bx_message> list = new List<bx_message>();
            try
            {
                list = DataContextFactory.GetDataContext().bx_message.Where(i => i.Agent_Id == agentId && i.Msg_Status == 0 && i.Msg_Type != 2 && i.Msg_Type != 1 && i.Send_Time.Value <= DateTime.Now).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            total = list.Count;
            return list;
        }

        public List<TableMessage> FindNoReadAllList(int agentId, out int total)
        {
            List<TableMessage> list = new List<TableMessage>();
            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@agentId", MySqlDbType.Int64)
                };
                parameters[0].Value = agentId;
                string strSql = @"SELECT CONCAT('Msg_',Id) AS StrId,Id,Msg_Level,Msg_Status,Msg_Type,Send_Time,Title,Body,
                                update_time,url,
                                agent_id,(SELECT AgentName FROM bx_agent WHERE id=agent_id) AS AgentName,
                                create_agent_id,(SELECT AgentName FROM bx_agent WHERE id=agent_id) AS CreateAgentName,
                                create_Time,(select licenseno from bx_userinfo where id=buid) AS licenseno,'' AS last_force_end_date,'' AS Last_biz_end_date,'' AS next_force_start_date,'' AS next_biz_start_date,
                                (select LastYearSource from bx_userinfo where id=buid) AS source,0 AS days,buid,
                                (select Agent from bx_userinfo where id=buid) AS OwnerAgent FROM bx_message 
                                WHERE Agent_Id=@agentId
                                AND Msg_Status=0 AND msg_type!=2 AND msg_type!=1 AND Send_Time <= NOW()
                                UNION ALL 
                                SELECT CONCAT('Xb_',Id) AS StrId,id,1,bx_notice_xb.stauts,1,create_time,'到期通知',CONCAT(license_no,'车险',day_num,'天后到期') AS Body,
                                '' AS update_time,'' AS url,
                                agent_id,(SELECT AgentName FROM bx_agent WHERE id=agent_id) AS AgentName,
                                agent_id AS create_agent_id,(SELECT AgentName FROM bx_agent WHERE id=agent_id) AS CreateAgentName,
                                create_time,license_no as licenseno,last_force_end_date,Last_biz_end_date,next_force_start_date,next_biz_start_date,
                                source,days,b_uid,
                                (select Agent from bx_userinfo where id=b_uid) AS OwnerAgent FROM bx_notice_xb 
                                WHERE agent_id=@agentId AND bx_notice_xb.stauts=0
                                UNION ALL 
                                SELECT CONCAT('Rw_',Id) AS StrId,id,1 AS Msg_Level,read_status,2 AS Msg_Type,create_time,'回访通知',CONCAT(date_format(next_review_date,'%m月%d日 %H:%I'),'需回访',(select LicenseNo from bx_userinfo where id=b_uid)) AS Body,
                                '' AS update_time,'' AS url,
                                operatorid,(SELECT AgentName FROM bx_agent WHERE id=operatorid) AS AgentName,
                                operatorid AS create_agent_id,(SELECT AgentName FROM bx_agent WHERE id=operatorid) AS CreateAgentName,
                                create_time,(select licenseno from bx_userinfo where id=b_uid) AS licenseno,'' AS last_force_end_date,'' AS Last_biz_end_date,'' AS next_force_start_date,'' AS next_biz_start_date,
                                (select LastYearSource from bx_userinfo where id=b_uid) AS source,0 AS days,b_uid,
                                (select Agent from bx_userinfo where id=b_uid) AS OwnerAgent FROM bx_consumer_review
                                WHERE operatorid=@agentId AND read_status=0 AND next_review_date IS NOT NULL AND next_review_date!=NULL
                                AND next_review_date <= NOW()";
                list = DataContextFactory.GetDataContext().Database.SqlQuery<TableMessage>(strSql, parameters.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            total = list.Count;
            return list;
        }
        #region 3表关联查询注释
        //var querylist =
        //    (from msg in DataContextFactory.GetDataContext().bx_message
        //     where
        //         msg.Agent_Id == agentId && msg.Msg_Status == 0 && msg.Msg_Type != 2 && msg.Msg_Type != 1 &&
        //         msg.Send_Time.Value <= DateTime.Now
        //     select new TableMessage()
        //     {
        //         StrId = "Msg_" + msg.Id,
        //         Id = (long)msg.Id,
        //         MsgLevel = msg.Msg_Level.HasValue ? msg.Msg_Level.Value : 0,
        //         MsgStatus = msg.Msg_Status.HasValue ? msg.Msg_Status.Value : 0,
        //         MsgType = msg.Msg_Type.HasValue ? msg.Msg_Type.Value : 0,
        //         SendTime = msg.Send_Time.HasValue ? msg.Send_Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
        //         Title = msg.Title,
        //         Body = msg.Body,
        //         UpdateTime =
        //             msg.Update_Time.HasValue ? msg.Update_Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
        //         Url = msg.Url,
        //         AgentId = msg.Agent_Id.HasValue ? msg.Agent_Id.Value : 0,
        //         CreateAgentId = msg.Create_Agent_Id,
        //         CreateTime =
        //             msg.Create_Time.HasValue ? msg.Create_Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
        //         LicenseNo = "",
        //         LastForceEndDate = "",
        //         LastBizEndDate = "",
        //         NextForceStartDate = "",
        //         NextBizStartDate = "",
        //         Source = 0,
        //         Days = 0,
        //         Buid = msg.Buid.HasValue ? msg.Buid.Value : 0
        //     })
        //.Concat(from nxb in DataContextFactory.GetDataContext().bx_notice_xb
        //        where
        //            nxb.agent_id == agentId && nxb.stauts == 0
        //        select new TableMessage()
        //        {
        //            StrId = "Xb_" + nxb.id,
        //            Id = nxb.id,
        //            MsgLevel = 1,//一般紧急
        //            MsgStatus = nxb.stauts,
        //            MsgType = 1,//1，续保消息
        //            SendTime = nxb.create_time.ToString("yyyy-MM-dd HH:mm:ss"),
        //            Title = "到期通知",
        //            Body = string.Format("{0}，车险{1}天后到期", nxb.license_no, nxb.day_num.Value.ToString()),
        //            UpdateTime = "",
        //            Url = "",
        //            AgentId = nxb.agent_id,
        //            CreateAgentId = nxb.agent_id,
        //            CreateTime = nxb.create_time.ToString("yyyy-MM-dd HH:mm:ss"),
        //            LicenseNo = nxb.license_no,
        //            LastForceEndDate = nxb.last_force_end_date.HasValue ? nxb.last_force_end_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
        //            LastBizEndDate = nxb.Last_biz_end_date.HasValue ? nxb.Last_biz_end_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
        //            NextForceStartDate = nxb.next_force_start_date.HasValue ? nxb.next_force_start_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
        //            NextBizStartDate = nxb.next_biz_start_date.HasValue ? nxb.next_biz_start_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
        //            Source = nxb.source,
        //            Days = nxb.days.HasValue ? nxb.days.Value : 0,
        //            Buid = nxb.b_uid
        //        }).Concat(from cr in DataContextFactory.GetDataContext().bx_consumer_review
        //                  where cr.operatorId == agentId && cr.read_status == 0 && cr.next_review_date.HasValue && cr.next_review_date.Value <= DateTime.Now
        //                  select new TableMessage()
        //                  {
        //                      StrId = "Rw_" + cr.id,
        //                      Id = (long)cr.id,
        //                      MsgLevel = 1,//一般紧急
        //                      MsgStatus = cr.read_status.Value,
        //                      MsgType = 2,//2，回访通知
        //                      SendTime = cr.create_time.Value.ToString("yyyy-MM-dd HH:mm:ss"),
        //                      Title = "回访通知",
        //                      Body = string.Format("{0}月{1}日{2} 需回访{3}", cr.next_review_date.Value.Month, cr.next_review_date.Value.Day, cr.next_review_date.Value.ToString("hh:mm"), ""),//msg.LicenseNo
        //                      UpdateTime = "",
        //                      Url = "",
        //                      AgentId = cr.operatorId.HasValue ? cr.operatorId.Value : 0,
        //                      CreateAgentId = cr.operatorId.Value,
        //                      CreateTime = cr.create_time.Value.ToString("yyyy-MM-dd HH:mm:ss"),
        //                      LicenseNo = "",
        //                      LastForceEndDate = "",
        //                      LastBizEndDate = "",
        //                      NextForceStartDate = "",
        //                      NextBizStartDate = "",
        //                      Source = 0,
        //                      Days = 0,
        //                      Buid = (long)cr.b_uid.Value
        //                  });

        //list = querylist.ToList();
        #endregion

        #endregion

        #region bx_system_message
        public int AddSysMessage(bx_system_message bxSystemMessage)
        {
            int workOrderId = 0;
            try
            {
                var t = DataContextFactory.GetDataContext().bx_system_message.Add(bxSystemMessage);
                DataContextFactory.GetDataContext().SaveChanges();
                workOrderId = t.Id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                workOrderId = 0;
            }
            return workOrderId;
        }

        public int UpdateSysMessage(bx_system_message bxSystemMessage)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_system_message.AddOrUpdate(bxSystemMessage);
                count = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }

        public List<bx_system_message> FindSysMessage()
        {
            List<bx_system_message> list = new List<bx_system_message>();
            try
            {
                list = DataContextFactory.GetDataContext().bx_system_message.OrderByDescending(o => o.Create_Time).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return list;
        }
        #endregion
    }
}
