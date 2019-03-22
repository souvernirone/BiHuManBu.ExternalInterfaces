using System;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using BiHuManBu.ExternalInterfaces.Models.DistributeModels;
using System.Text;
using System.Configuration;
using MySqlHelper = BiHuManBu.ExternalInterfaces.Infrastructure.MySqlDbHelper.MySqlHelper;
using MySql.Data.MySqlClient;
using BiHuManBu.StoreFront.Infrastructure.DbHelper;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CameraDistributeRepository : ICameraDistributeRepository
    {
        private static readonly string DbConfig = ConfigurationManager.ConnectionStrings["zb"].ConnectionString;
        private readonly MySqlHelper _dbHelper = new MySqlHelper(DbConfig);
        private ILog logError;
        private ILog logInfo;
        private EntityContext db = DataContextFactory.GetDataContext();

        public CameraDistributeRepository()
        {
            logError = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");
        }
        public bx_camera_config GetCameraConfig(int childagent)
        {
            string sql = string.Format(@"SELECT * from bx_camera_config where park_id='{0}'", childagent);
            using (var db = new EntityContext())
            {
                return db.Database.SqlQuery<bx_camera_config>(sql).FirstOrDefault();
            }
        }
        public int GetRoleTypeByAgentId(int agentId)
        {
            var roleType = GetRoleTypeByAgentId(new List<int> { agentId });
            if (roleType.Any())
            {
                return roleType.FirstOrDefault().RoleType;
            }
            return 0;
        }

        public List<AgentIdAndRoleTyoeDto> GetRoleTypeByAgentId(List<int> listAgent)
        {
            var sqlBuilder = @"
                SELECT 
                    bx_agent.Id AS AgentId,
                    manager_role_db.role_type AS RoleType
                FROM
                    bx_agent
                        INNER JOIN
                    manager_role_db ON bx_agent.ManagerRoleId = manager_role_db.id
                WHERE
                    bx_agent.id IN ({0});
                ";
            var sql = string.Format(sqlBuilder, string.Join(",", listAgent));
            return db.Database.SqlQuery<AgentIdAndRoleTyoeDto>(sql).ToList();
        }
        public bx_camera_blacklist GetCameraBlack(int agent, int childAgent, string LicenseNo)
        {
            bx_camera_blacklist response = db.bx_camera_blacklist.Where(x => x.agent_id == childAgent && x.license_no == LicenseNo && x.IsDelete == 0).FirstOrDefault();
            return response;
        }
        public List<int> GetSonListByDb(int currentAgent, bool hasSelf = true)
        {
            var listAgents = new List<int>();
            try
            {
                var parameters = new List<MySqlParameter>(){
                    new MySqlParameter
                    {
                        MySqlDbType = MySqlDbType.Int32,
                        ParameterName = "curAgent",
                        Value = currentAgent
                    }
                };
                #region SQL语句
                var strSql = new StringBuilder();
                var Agent = GetAgent(currentAgent);
                if (hasSelf && Agent.RegType != 1)
                {
                    strSql.Append("select SQL_CACHE  ?curAgent ")
                        .Append(" union ");
                }
                if (Agent.RegType == 1)
                {
                    strSql.Append(" select id from  bx_agent where group_id=?curAgent and isused=1")
                    .Append(" union")
                    .Append(@" select id from bx_agent
                            where parentagent in (select id from bx_agent where group_id=?curAgent and isused=1) ")
                    .Append(" union")
                    .Append(@" select id from bx_agent where parentagent in (
                            select id from bx_agent where parentagent in (
                            select id from  bx_agent where group_id=?curAgent and isused=1) ) ");
                }
                else
                {
                    strSql.Append(" select id from  bx_agent where parentagent=?curAgent and isused=1")
                        .Append(" union")
                        .Append(@" select id from bx_agent
                            where parentagent in (select id from bx_agent where parentagent=?curAgent and isused=1) ")
                        .Append(" union")
                        .Append(@" select id from bx_agent where parentagent in (
                            select id from bx_agent where parentagent in (
                            select id from  bx_agent where parentagent=?curAgent and isused=1) ) ");
                }
                //#第五级代理
                //union  all
                //select id from  bx_agent
                //where  parentagent in (
                //select id from bx_agent 
                //where  parentagent in (
                //select id from bx_agent
                //where  parentagent in (
                //select id from  bx_agent where  parentagent=@curAgent
                //) )
                //)
                #endregion
                //查询列表
                IList<int> reList = _dbHelper.ExecuteDataSet(strSql.ToString(), parameters.ToArray()).ToList<int>();
                listAgents = reList.ToList();
                return listAgents;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return new List<int>() { currentAgent };
        }
        public List<int> GetSonListByDbAsync(int currentAgent, bool hasSelf = true)
        {
            var listAgents = new List<int>();
            try
            {
                var parameters = new List<MySqlParameter>(){
                    new MySqlParameter
                    {
                        MySqlDbType = MySqlDbType.Int32,
                        ParameterName = "curAgent",
                        Value = currentAgent
                    }
                };
                #region SQL语句
                var strSql = new StringBuilder();
                var Agent = GetAgentAsyc(currentAgent);
                if (hasSelf && Agent.RegType != 1)
                {
                    strSql.Append("select SQL_CACHE  ?curAgent ")
                        .Append(" union ");
                }
                if (Agent.RegType == 1)
                {
                    strSql.Append(" select id from  bx_agent where group_id=?curAgent and isused=1")
                    .Append(" union")
                    .Append(@" select id from bx_agent
                            where parentagent in (select id from bx_agent where group_id=?curAgent and isused=1) ")
                    .Append(" union")
                    .Append(@" select id from bx_agent where parentagent in (
                            select id from bx_agent where parentagent in (
                            select id from  bx_agent where group_id=?curAgent and isused=1) ) ");
                }
                else
                {
                    strSql.Append(" select id from  bx_agent where parentagent=?curAgent and isused=1")
                        .Append(" union")
                        .Append(@" select id from bx_agent
                            where parentagent in (select id from bx_agent where parentagent=?curAgent and isused=1) ")
                        .Append(" union")
                        .Append(@" select id from bx_agent where parentagent in (
                            select id from bx_agent where parentagent in (
                            select id from  bx_agent where parentagent=?curAgent and isused=1) ) ");
                }
                //#第五级代理
                //union  all
                //select id from  bx_agent
                //where  parentagent in (
                //select id from bx_agent 
                //where  parentagent in (
                //select id from bx_agent
                //where  parentagent in (
                //select id from  bx_agent where  parentagent=@curAgent
                //) )
                //)
                #endregion
                //查询列表
                IList<int> reList = _dbHelper.ExecuteDataSet(strSql.ToString(), parameters.ToArray()).ToList<int>();
                listAgents = reList.ToList();
                return listAgents;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return new List<int>() { currentAgent };
        }
        public bx_agent GetAgent(int agentId)
        {
            var item = new bx_agent();
            try
            {
                item = DataContextFactory.GetDataContext().bx_agent.FirstOrDefault(x => x.Id == agentId);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return item;
        }
        public bx_agent GetAgentAsyc(int agentId)
        {
            var item = new bx_agent();
            try
            {
                item = new EntityContext().bx_agent.FirstOrDefault(x => x.Id == agentId);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return item;
        }
        public List<bx_userinfo> GetUserinfoByLicenseAndAgent(long buid, string licenseNo, List<string> agentIds)
        {
            try
            {
                var model = db.bx_userinfo
                    .Where(x => x.IsTest == 0
                        && x.RenewalCarType == 0
                        && x.LicenseNo == licenseNo
                        && agentIds.Contains(x.Agent))
                    .OrderByDescending(o => o.UpdateTime).ToList();
                return model;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return new List<bx_userinfo>();
        }
        public string RemoveList(string userIds, int isTest, ref bool isSuccess)
        {
            var result = "操作成功！";
            try
            {
                var modifySql = string.Format("update bx_userinfo set IsTest={0} where Id in ({1})", isTest, userIds);
                DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(modifySql);
                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
                result = "操作失败！";
            }
            return result;
        }
        public List<carMold> FindCarModel(int agentId)
        {
            #region 参数
            MySqlParameter[] parameters =
                        {
                               new MySqlParameter("@agentId", MySqlDbType.Int32)
                        };
            parameters[0].Value = agentId;
            #endregion

            var querySql = string.Format("select Id as id,CarType as name,'modify' as status from bx_car_mold where IsDel=0 ");
            if (agentId > 0)
                querySql += " AND AgentId=@agentId";

            return new EntityContext().Database.SqlQuery<carMold>(querySql.ToString(), parameters.ToArray()).ToList();
        }
        /// <summary>
        /// isTest=3(回收站) isTest=1(删除) 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isTest">isTest=1是删除、isTest=3 进回收站</param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public string Remove(int userId, int isTest, ref bool isSuccess)
        {
            var result = "操作成功！";
            try
            {
                #region 参数
                MySqlParameter[] parameters =
                             {
                                   new MySqlParameter("@userId", MySqlDbType.Int32),
                                   new MySqlParameter("@IsTest", MySqlDbType.Int32)
                                 };
                parameters[0].Value = userId;
                parameters[1].Value = isTest;
                #endregion
                var modifySql = string.Format("update bx_userinfo set IsTest=@IsTest where Id=@userId");
                DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(modifySql, parameters);
                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
                result = "操作失败！";
            }
            return result;
        }
        /// <summary>
        /// 设置车型Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="carModlId"></param>
        /// <returns></returns>
        public bool SetCarModelId(int userId, int carModlId)
        {
            #region 参数
            MySqlParameter[] parameters =
                        {
                               new MySqlParameter("@userId", MySqlDbType.Int32),
                               new MySqlParameter("@carModlId", MySqlDbType.Int32)
                        };
            parameters[0].Value = userId;
            parameters[1].Value = carModlId;
            #endregion

            var querySql = string.Format("update bx_userinfo set CarMoldId=@carModlId where Id=@userId");
            if (DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(querySql.ToString(), parameters.ToArray()) > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 从回收站 撤销
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool RevokeFiles(int userId)
        {
            var result = false;
            try
            {
                #region 参数
                MySqlParameter[] parameters =
                        {
                                   new MySqlParameter("@userId", MySqlDbType.VarChar)
                            };
                parameters[0].Value = userId;
                #endregion
                var modifySql = string.Format("update bx_userinfo set isTest=0 where Id=@userId");
                var effectRow = DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(modifySql, parameters);
                result = effectRow > 0;
                #region bygpj20180209将此处逻辑删除，并改到service层，并且调用东亮的恢复批续数据功能
                //if (result)
                //{
                //    new BatchRenewalRepository().RevokeDelete(userId);
                //}
                #endregion
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return result;
        }
        /// <summary>
        /// 根据Buid恢复bx_batchrenewal_item表
        /// </summary>
        /// <param name="buId"></param>
        /// <returns></returns>
        public bool RevertBatchRenewalItem(int buId)
        {
            int isSuccess = 0;
            try
            {
                //查看指定buId所有可用批次Item的Id
                var buidListSql = string.Format("SELECT bx_batchrenewal_item.Id FROM bx_batchrenewal_item INNER JOIN bx_batchrenewal ON bx_batchrenewal_item.BatchId = bx_batchrenewal.Id WHERE	bx_batchrenewal_item.BUId ={0} AND bx_batchrenewal.IsDelete = 0", buId);

                var newbuids = DataContextFactory.GetDataContext().Database.SqlQuery<long>(buidListSql).ToList();
                if (newbuids.Any())
                {
                    var sb = new StringBuilder();
                    //更新可用批次ID最大的记录为最新
                    sb.Append(string.Format("update   bx_batchrenewal_item set IsDelete=0 , IsNew=1  where ID IN ({0}); ", newbuids.Max()));
                    //更新未删除批次的Item为未删除
                    sb.Append(string.Format("UPDATE  bx_batchrenewal_item  SET IsDelete=0  WHERE  ID IN ({0})  ", string.Join(",", newbuids)));
                    isSuccess = DataContextFactory.GetDataContext().Database.ExecuteSqlCommand(sb.ToString());
                }

            }
            catch (Exception ex)
            {
                logError.Info("根据Buid恢复bx_batchrenewal_item表出错：buid:" + buId + "--异常消息:" + ex.Message + "--错误对象名称:" + ex.Source + "--堆栈信息:" + ex.StackTrace);
            }
            //判断
            if (isSuccess > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 根据buid批量更新扩展表
        /// </summary>
        /// <param name="buids"></param>
        /// <returns></returns>
        public bool UpdateUserExpandByBuid(string buids, int DeleteType, DateTime DeleteTime)
        {
            try
            {
                var param = new[]
                                {
                                    new MySqlParameter
                                    {
                                        MySqlDbType = MySqlDbType.Int32,
                                        ParameterName = "delete_type",
                                        Value = DeleteType
                                    },
                                    new MySqlParameter
                                    {
                                        MySqlDbType = MySqlDbType.DateTime,
                                        ParameterName = "delete_time",
                                        Value = DeleteTime
                                    }
                                };
                string sql = "UPDATE bx_userinfo_expand SET delete_type=@delete_type,delete_time=@delete_time WHERE b_uid IN (" + buids + ");";
                return db.Database.ExecuteSqlCommand(sql, param) > 0;
            }
            catch (Exception ex)
            {
                logError.Info("UpdateUserExpandByBuid发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return false;
        }
        /// <summary>
        /// 删除某个buid
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        public bool DeleteUserinfo(long buid, string deleteAgentId)
        {
            try
            {
                bx_userinfo bxUser = db.bx_userinfo.FirstOrDefault(c => c.Id == buid);
                bxUser.Agent = deleteAgentId;
                bxUser.OpenId = deleteAgentId.GetMd5();
                bxUser.IsTest = 3;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return false;
        }
        /// <summary>
        /// 更新模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(bx_userinfo model)
        {
            int count = 0;
            try
            {
                db.bx_userinfo.AddOrUpdate(model);
                count = db.SaveChanges();
                return count;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }
        public bx_batchrenewal_item GetItemByBuId(long buId)
        {
            return DataContextFactory.GetDataContext().bx_batchrenewal_item.Where(x => x.BUId == buId && x.IsDelete == 0 && x.IsNew == 1).FirstOrDefault();
        }
        public bx_userinfo GetUserInfo(long id)
        {
            return db.bx_userinfo.FirstOrDefault(x => x.Id == id);
        }
        public async Task<int> AddDistributedHistoryAsync(bx_distributed_history model)
        {
            try
            {
                var _db = new EntityContext();
                _db.bx_distributed_history.Add(model);
                return await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bxCrmSteps"></param>
        /// <returns></returns>
        public async Task<int> AddCrmStepsAsync(bx_crm_steps bxCrmSteps)
        {
            try
            {
                var _db = new EntityContext();
                _db.bx_crm_steps.Add(bxCrmSteps);
                return _db.SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                return 0;
            }

        }
        /// <summary>
        /// 更新续保类型&分配状态
        /// </summary>
        /// <param name="buId"></param>
        /// <param name="renewalType"></param>
        /// <param name="distributed"></param>
        /// <returns></returns>
        public int UpdateUserRenewalTypeAndDistributed(int buId, int renewalType, int distributed,
            bool exitUserinfo = false, bool isDistributedUserInfo = false)
        {
            if (exitUserinfo || isDistributedUserInfo)
            {
                return 1;//数据存在 或执行自动分配的数据无需更新
            }
            var bxUser = db.bx_userinfo.FirstOrDefault(c => c.Id == buId);
            if (bxUser.IsDistributed != 3 || distributed == 0)
            {
                //已分配的数据 如果原状态和要改的状态不一致，则状态不变
                bxUser.IsDistributed = distributed;
            }
            //摄像头进店，标记为摄像头进店，并且更新进店时间
            if (renewalType == 3)
            {
                bxUser.IsCamera = true;
                bxUser.CameraTime = DateTime.Now;
            }
            return db.SaveChanges();
        }
        public int Add(bx_message bxMessage)
        {
            int workOrderId = 0;
            try
            {
                var _db = new EntityContext();
                var t = _db.bx_message.Add(bxMessage);
                _db.SaveChanges();
                workOrderId = t.Id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                workOrderId = 0;
            }
            return workOrderId;
        }
        public long AddMsgIdx(bx_msgindex model)
        {
            long modelId = 0;
            try
            {
                var _db = new EntityContext();
                var t = _db.bx_msgindex.Add(model);
                _db.SaveChanges();
                modelId = t.Id;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                modelId = 0;
            }
            return modelId;
        }
        public bx_agent_xgaccount_relationship GetXgAccount(int agentId)
        {
            return new EntityContext().bx_agent_xgaccount_relationship.FirstOrDefault(x => x.AgentId == agentId && !x.Deleted);
        }
        public List<long> FindAgentIdBySealman(int agentId)
        {
            var querySql = string.Format(@" select AgentId from bx_Salesman group by AgentId");
            if (agentId > 0)
                querySql = string.Format(@" select UserId from bx_Salesman where  AgentId={0}", agentId);
            return new EntityContext().Database.SqlQuery<long>(querySql.ToString()).ToList();
        }
        /// <summary>
        /// 获取摄像头 业务员请假信息
        /// </summary>
        /// <param name="UserId">代理人Id</param>
        /// <param name="year">年份</param>
        /// <param name="mounth">月份</param>
        /// <returns></returns>
        public List<LeaveDate> FindSealmanLeave(int userId)
        {
            #region 查询参数
            MySqlParameter[] parameters =
                {
                    new MySqlParameter("@UserId", MySqlDbType.Int32)
                };
            parameters[0].Value = userId;
            #endregion

            var querySql = string.Format(@" select leaveTime as 'leave','modify' as status from bx_salesman_leave where UserId=@UserId ");
            return new EntityContext().Database.SqlQuery<LeaveDate>(querySql.ToString(), parameters.ToArray()).ToList();
        }
        /// <summary>
        /// 获取所有请假人的id
        /// </summary>
        /// <returns></returns>
        public List<long> FindSealmanLeave()
        {
            var querySql = string.Format(@" select UserId from bx_salesman_leave group by UserId");
            return DataContextFactory.GetDataContext().Database.SqlQuery<long>(querySql.ToString()).ToList();
        }
    }
}
