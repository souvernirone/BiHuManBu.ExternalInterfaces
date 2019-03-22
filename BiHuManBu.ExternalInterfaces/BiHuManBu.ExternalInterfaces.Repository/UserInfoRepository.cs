using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using log4net;
using MySql.Data.MySqlClient;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class UserInfoRepository : EFRepositoryBase<bx_userinfo>, IUserInfoRepository
    {
        // private EntityContext context = new EntityContext();
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");

        public bx_userinfo Find(int userId)
        {
            bx_userinfo userinfo = new bx_userinfo();
            try
            {
                userinfo = DataContextFactory.GetDataContext().bx_userinfo.First(x => x.UserId == userId);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return userinfo;

        }

        public bx_userinfo FindByBuid(long buid)
        {
            bx_userinfo userinfo = new bx_userinfo();
            try
            {
                userinfo = DataContextFactory.GetDataContext().bx_userinfo.AsNoTracking().FirstOrDefault(x => x.Id == buid);
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return userinfo;
        }
        public bx_userinfo FindByBuidSync(long buid)
        {
            bx_userinfo userinfo = new bx_userinfo();
            try
            {
                userinfo = new EntityContext().bx_userinfo.AsNoTracking().FirstOrDefault(x => x.Id == buid);
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return userinfo;
        }
        public bx_userinfo FindByOpenIdAndLicense(string openid, string licenseno)
        {
            bx_userinfo tt = new bx_userinfo();
            try
            {
                tt = DataContextFactory.GetDataContext().bx_userinfo.FirstOrDefault(x => x.LicenseNo == licenseno && x.OpenId == openid);
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return tt;
        }

        public bx_userinfo FindByOpenIdAndLicense(string openid, string licenseno, string agent, int cartype, int isTest = 0)//修改,int isTest=0
        {
            bx_userinfo tt = new bx_userinfo();
            try
            {
                var result = DataContextFactory.GetDataContext().bx_userinfo.Where(x => x.Agent == agent && x.LicenseNo == licenseno && x.OpenId == openid && x.RenewalCarType == cartype && x.IsTest == isTest);//&&x.IsTest==isTest
                tt = result.OrderByDescending(l => l.UpdateTime).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return tt;
        }

        /// <summary>
        /// app端用查询userinfo，由于无openid限制，故3个条件不用openid查
        /// 顶级代理下的车辆有可能重复，以最新的一条记录来处理，即使查看老数据，也展示最新的一条数据
        /// </summary>
        /// <param name="licenseno"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        public bx_userinfo FindByAgentLicense(string licenseno, string agent)
        {
            bx_userinfo tt = new bx_userinfo();
            try
            {
                tt = DataContextFactory.GetDataContext().bx_userinfo.OrderByDescending(o => o.UpdateTime.HasValue ? o.UpdateTime : o.CreateTime).ThenByDescending(o => o.CreateTime).FirstOrDefault(x => x.LicenseNo == licenseno && x.Agent == agent);// && x.OpenId.Length > 9// && (!x.IsSingleSubmit.HasValue)
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return tt;
        }
        public bx_userinfo FindByCarvin(string carvin, string engineno, string openid, string agent, int cartype, int istest = 0)//修改,int istest=0
        {
            bx_userinfo tt = new bx_userinfo();
            try
            {
                tt = DataContextFactory.GetDataContext().bx_userinfo.FirstOrDefault(x => x.Agent == agent && x.CarVIN == carvin.ToUpper() && x.EngineNo == engineno.ToUpper() && x.OpenId == openid && x.RenewalCarType == cartype && x.IsTest == istest);//&&x.IsTest==istest
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return tt;
        }
        /// <summary>
        /// 根据车架号发动机号查询bx_userinfo记录
        /// 20171026去掉发动机号by.gpj
        /// </summary>
        /// <param name="carvin"></param>
        /// <param name="openid"></param>
        /// <param name="agent"></param>
        /// <param name="cartype"></param>
        /// <param name="istest"></param>
        /// <returns></returns>
        public bx_userinfo FindByCarvin(string carvin, string openid, string agent, int cartype, int istest = 0)
        {
            bx_userinfo tt = new bx_userinfo();
            try
            {
                var result = DataContextFactory.GetDataContext().bx_userinfo.Where(x => x.Agent == agent && x.CarVIN == carvin.ToUpper() && x.OpenId == openid && x.RenewalCarType == cartype && x.IsTest == istest);
                tt = result.OrderByDescending(l => l.UpdateTime).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return tt;
        }

        public long Add(bx_userinfo userinfo)
        {
            bx_userinfo item = new bx_userinfo();
            try
            {
                item = DataContextFactory.GetDataContext().bx_userinfo.Add(userinfo);
                var returnResult = DataContextFactory.GetDataContext().SaveChanges();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return item.Id;
        }

        public int Update(bx_userinfo userinfo)
        {
            int count = 0;
            try
            {
                DataContextFactory.GetDataContext().bx_userinfo.AddOrUpdate(userinfo);
                count = DataContextFactory.GetDataContext().SaveChanges();

            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }
        public int UpdateSync(bx_userinfo userinfo)
        {
            int count = 0;
            try
            {
                var _db = new EntityContext();
                _db.bx_userinfo.AddOrUpdate(userinfo);
                count = _db.SaveChanges();
            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }
        public List<bx_userinfo> GetMyBjdList(bool hasOutOrder, string strWhere, int pageSize, int curPage, out int totalCount)
        {
            totalCount = 0;
            var listuserinfo = new List<bx_userinfo>();
            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@pagebegin", MySqlDbType.Int32),
                    new MySqlParameter("@pageend", MySqlDbType.Int32)
                };
                parameters[0].Value = (curPage - 1) * pageSize;
                parameters[1].Value = pageSize;
                //列表
                var sbSqlList = new StringBuilder();
                sbSqlList.Append(" SELECT ui.* FROM bx_userinfo AS ui");
                sbSqlList.Append(" LEFT JOIN (SELECT buid,order_status,MAX(create_time) AS createdate FROM bx_car_order");
                if (hasOutOrder)
                {//是否已收单
                    sbSqlList.Append(" WHERE order_status =-3");
                }
                else
                {
                    sbSqlList.Append(" GROUP BY buid,order_status,create_time limit 1) co ON co.buid=ui.id");
                }
                sbSqlList.Append(" WHERE QuoteStatus > -1 ");
                if (!hasOutOrder)
                {//是否已收单
                    sbSqlList.Append(" AND (co.order_status!=-3 OR co.order_status IS NULL)");
                }
                else
                {
                    sbSqlList.Append(" AND co.order_status IS NOT NULL");
                }
                sbSqlList.Append(strWhere)
                 .Append(" ORDER BY ui.UpdateTime DESC limit @pagebegin,@pageend ");
                //数量
                var sbSqlCount = new StringBuilder();
                sbSqlCount.Append(" SELECT COUNT(1) FROM bx_userinfo AS ui")
                    .Append(" LEFT JOIN (SELECT buid,order_status,MAX(create_time) AS createdate FROM bx_car_order GROUP BY buid,order_status,create_time limit 1) co ON co.buid=ui.id")
                    .Append(" WHERE QuoteStatus > -1 ")
                    .Append(" AND (co.order_status!=-3 OR co.order_status IS NULL)")
                    .Append(strWhere);
                //查询总条数
                totalCount = DataContextFactory.GetDataContext().Database.SqlQuery<int>(sbSqlCount.ToString(), Enumerable.ToArray(parameters)).FirstOrDefault();
                //查询列表
                listuserinfo = DataContextFactory.GetDataContext().Database.SqlQuery<bx_userinfo>(sbSqlList.ToString(), Enumerable.ToArray(parameters)).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return listuserinfo;
        }

        /// <summary>
        /// app端的续保列表，根据bx_transferrecord获取列表
        /// 旧版app，目前还在用
        /// </summary>
        /// <param name="isAgent"></param>
        /// <param name="sonself"></param>
        /// <param name="strPass"></param>
        /// <param name="licenseNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="curPage"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<bx_userinfo> FindReInfoList(bool isAgent, List<bx_agent> sonself, string strPass, string licenseNo, int pageSize, int curPage, out int totalCount)
        {
            totalCount = 0;
            //sonself取bxagent中的id，方便下文的in查询
            var listStr = new List<long>();
            sonself.ForEach(i => listStr.Add(i.Id));

            var listuserinfo = new List<bx_userinfo>();
            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@agentId", MySqlDbType.Int32),
                    new MySqlParameter("@pagebegin", MySqlDbType.Int32),
                    new MySqlParameter("@pageend", MySqlDbType.Int32)
                };
                parameters[0].Value = listStr.First();
                parameters[1].Value = (curPage - 1) * pageSize;
                parameters[2].Value = pageSize;
                //列表
                var sbSqlList = new StringBuilder();
                sbSqlList.Append(
                    "SELECT * FROM bx_userinfo WHERE id IN (")
                    .Append(
                    "SELECT DISTINCT(buid) FROM bx_transferrecord WHERE FromAgentId = @agentId OR ToAgentId = @agentId")
                    .Append(")")
                    .Append(" ORDER BY UpdateTime DESC limit @pagebegin,@pageend ");
                //数量
                var sbSqlCount = new StringBuilder();
                sbSqlCount.Append(
                    "SELECT COUNT(1) FROM bx_userinfo WHERE id IN (")
                    .Append(
                        "SELECT DISTINCT(buid) FROM bx_transferrecord WHERE FromAgentId = @agentId OR ToAgentId = @agentId")
                    .Append(")");
                //查询总条数
                totalCount = DataContextFactory.GetDataContext().Database.SqlQuery<int>(sbSqlCount.ToString(), Enumerable.ToArray(parameters)).FirstOrDefault();
                //查询列表
                listuserinfo = DataContextFactory.GetDataContext().Database.SqlQuery<bx_userinfo>(sbSqlList.ToString(), Enumerable.ToArray(parameters)).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return listuserinfo;
        }

        /// <summary>
        /// 续保计费系统的续保列表
        /// 只查续保
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="isAgent"></param>
        /// <param name="sonself"></param>
        /// <param name="strPass"></param>
        /// <param name="licenseNo"></param>
        /// <param name="renewalStatus"></param>
        /// <param name="pageSize"></param>
        /// <param name="curPage"></param>
        /// <param name="totalCount"></param>
        /// <param name="lastYearSource"></param>
        /// <returns></returns>
        public List<bx_userinfo> FindReList(string strWhere, int pageSize, int curPage, out int totalCount)
        {
            totalCount = 0;
            var listuserinfo = new List<bx_userinfo>();
            try
            {
                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@pagebegin", MySqlDbType.Int32),
                    new MySqlParameter("@pageend", MySqlDbType.Int32)
                };
                parameters[0].Value = (curPage - 1) * pageSize;
                parameters[1].Value = pageSize;
                //列表
                string strSqlList = string.Format("SELECT * FROM bx_userinfo WHERE {0} ORDER BY UpdateTime DESC limit @pagebegin,@pageend", strWhere);
                //数量
                string strSqlCount = string.Format("SELECT COUNT(1) FROM bx_userinfo WHERE {0}", strWhere);
                //查询总条数
                totalCount = DataContextFactory.GetDataContext().Database.SqlQuery<int>(strSqlCount).FirstOrDefault();
                //查询列表
                listuserinfo = DataContextFactory.GetDataContext().Database.SqlQuery<bx_userinfo>(strSqlList, Enumerable.ToArray(parameters)).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return listuserinfo;
            //try
            //{
            //    //查询我的报价单列表，搜索bx_car_order中未出单的记录
            //    var list = from ui in context.bx_userinfo
            //               where (isAgent ? listStr.Contains(ui.Agent) : ui.OpenId.Equals(strPass))
            //               && (!lastYearSource.HasValue || ui.LastYearSource == lastYearSource.Value)
            //               && (!renewalStatus.HasValue || (renewalStatus.Value == 1 ?
            //                   ((ui.NeedEngineNo == 0 && ui.RenewalStatus != 1) || (ui.NeedEngineNo == 0 && ui.LastYearSource > -1) || ui.RenewalStatus == 1) : ui.NeedEngineNo == 1))
            //               && ui.QuoteStatus == -1
            //               && ui.OpenId.Length > 9
            //               && (string.IsNullOrEmpty(licenseNo) || (ui.LicenseNo.Contains(licenseNo.ToUpper()) || ui.LicenseOwner.Contains(licenseNo.ToUpper())))
            //               select ui;

            //    listuserinfo = list.ToList();
            //    totalCount = listuserinfo.Count;
            //}
            //catch (Exception ex)
            //{
            //    logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            //}
            //return listuserinfo.OrderByDescending(i => (i.UpdateTime.HasValue ? i.UpdateTime : i.CreateTime)).ThenByDescending(i => i.CreateTime).Take(pageSize * curPage).Skip(pageSize * (curPage - 1)).ToList();
        }

        /// <summary>
        /// app端统计用，续保报表
        /// </summary>
        /// <param name="sonself"></param>
        /// <param name="strDate"></param>
        /// <param name="licenseNo"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<bx_userinfo> ReportForReInfoList(List<bx_agent> sonself, string strDate, string licenseNo, out int totalCount)
        {
            totalCount = 0;
            var ids = new StringBuilder();
            //sonself取bxagent中的id，方便下文的in查询
            var listStr = new List<int>();
            //sonself.ForEach(i => listStr.Add(i.Id));
            sonself.ForEach(i => ids.Append(i.Id).Append(','));
            if (ids.Length > 2)
            {
                ids = ids.Remove(ids.Length - 1, 1);
            }

            var listuserinfo = new List<bx_userinfo>();
            try
            {
                string startdt = strDate + " 00:00:00";
                string enddt = strDate + " 23:59:59";
                var sql = new StringBuilder(@"SELECT * FROM bx_userinfo WHERE LENGTH(OpenId)>9 AND id IN(
                                                        SELECT DISTINCT buid FROM bx_transferrecord WHERE ( FromAgentId IN (" + ids.ToString() + @") OR ToAgentId IN (" + ids.ToString() + @")))
                                                         AND id IN ( SELECT DISTINCT b_uid FROM bx_consumer_review WHERE result_status=1 AND create_time >= '" + startdt + @"' AND create_time <= '" + enddt + @"')");
                listuserinfo = DataContextFactory.GetDataContext().Database.SqlQuery<bx_userinfo>(sql.ToString()).ToList();
                totalCount = listuserinfo.Count;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return listuserinfo.OrderByDescending(i => (i.UpdateTime.HasValue ? i.UpdateTime : i.CreateTime)).ThenByDescending(i => i.CreateTime).ToList();
        }

        /// <summary>
        /// 查询userinfo的agent
        /// </summary>
        /// <param name="lecenseNo"></param>
        /// <returns></returns>
        public List<bx_userinfo> FindAgentListByLicenseNo(string lecenseNo)
        {//length>=10,排除老数据
            var list = DataContextFactory.GetDataContext().bx_userinfo.Where(x => x.LicenseNo == lecenseNo && x.OpenId.Length > 9 && x.IsTest != 1).ToList();
            return list;
        }

        /// <summary>
        /// 根据车牌号和代理人信息查询是否有子集数据
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <param name="agentIds"></param>
        /// <returns></returns>
        public bx_userinfo FindAgentListByLicenseNo(string licenseNo, List<string> agentIds)
        {
            try
            {
                //去掉istest=1的测试数据
                var list = DataContextFactory.GetDataContext().bx_userinfo
                    .Where(x => x.LicenseNo == licenseNo
                        && x.OpenId.Length > 9
                        && (!x.IsTest.HasValue || x.IsTest == 0)
                        && agentIds.Contains(x.Agent))
                        .OrderByDescending(o => o.UpdateTime).FirstOrDefault();
                return list;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return new bx_userinfo();
        }

        /// <summary>
        /// 报价续保综合查询列表
        /// </summary>
        /// <param name="isAgent"></param>
        /// <param name="sonself"></param>
        /// <param name="strAgents"></param>
        /// <param name="strPass"></param>
        /// <param name="licenseNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="curPage"></param>
        /// <param name="orderBy"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<UserInfoModel> FindMyList(bool isAgent, string strAgents, string strPass, string licenseNo, int pageSize, int curPage, int orderBy, out int totalCount)
        {
            totalCount = 0;
            var listuserinfo = new List<UserInfoModel>();
            try
            {
                #region 查询条件
                var strWhere = new StringBuilder();
                //where 开始，不用加 AND
                strWhere.Append(" LENGTH(ui.OpenId)>9 ")
                    .Append(isAgent ? strAgents : " AND ui.OpenId in @openId ");
                if (!string.IsNullOrWhiteSpace(licenseNo))
                {
                    strWhere.Append(" AND ui.LicenseNo like @licenseNo ");
                }

                MySqlParameter[] parameters =
                {
                    new MySqlParameter("@openId", MySqlDbType.String),
                    new MySqlParameter("@licenseNo", MySqlDbType.String),
                    new MySqlParameter("@pagebegin", MySqlDbType.Int32),
                    new MySqlParameter("@pageend", MySqlDbType.Int32)
                };
                parameters[0].Value = strPass;
                parameters[1].Value = "%" + (string.IsNullOrEmpty(licenseNo) ? "" : licenseNo.ToUpper()) + "%";
                parameters[2].Value = (curPage - 1) * pageSize;
                parameters[3].Value = pageSize;
                #endregion
                #region SQL语句
                string strSql = @" FROM bx_car_renewal cr 
                                RIGHT JOIN bx_userinfo_renewal_index uri ON cr.Id=uri.car_renewal_id 
                                RIGHT JOIN bx_userinfo ui ON ui.Id =uri.b_uid WHERE ";
                var sqlCount = new StringBuilder(string.Format("SELECT COUNT(1) {0}{1}", strSql, strWhere));
                var sqlList = new StringBuilder(
                    string.Format(@"SELECT ui.Id,ui.licenseno,ui.openid,ui.citycode,ui.moldname,
                                    ui.createtime,ui.updatetime,
                                    ui.RenewalStatus,
                                    ui.quotestatus,
                                    IF(YEAR(cr.LastForceEndDate)!='0001',1,0) AS ValueLastForceEndDate,
                                    ui.agent,ui.issinglesubmit,cr.LastForceEndDate,
                                    cr.LastBizEndDate {0}{1}", strSql, strWhere));
                //IF(ui.RenewalStatus!=NULL,ui.RenewalStatus,-1) AS RenewalStatus,
                switch (orderBy)
                {
                    case 2:
                        sqlList.Append(" ORDER BY RenewalStatus DESC,ValueLastForceEndDate DESC,LastForceEndDate ASC,ui.Id ASC limit @pagebegin,@pageend ");
                        break;
                    default:
                        sqlList.Append(" ORDER BY ui.UpdateTime DESC,ui.Id DESC limit @pagebegin,@pageend ");
                        break;
                }
                #endregion
                //查询总条数
                totalCount = DataContextFactory.GetDataContext().Database.SqlQuery<int>(sqlCount.ToString(), Enumerable.ToArray(parameters)).FirstOrDefault();
                //查询列表
                listuserinfo = DataContextFactory.GetDataContext().Database.SqlQuery<UserInfoModel>(sqlList.ToString(), Enumerable.ToArray(parameters)).ToList();
                return listuserinfo;
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return new List<UserInfoModel>();
        }

        /// <summary>
        /// 获取我的报价单总数
        /// </summary>
        /// <param name="isAgent"></param>
        /// <param name="sonself"></param>
        /// <param name="strPass"></param>
        /// <returns></returns>
        public int CountBaojia(bool isAgent, List<bx_agent> sonself, string strPass)
        {
            int count = 0;
            //sonself取bxagent中的id，方便下文的in查询
            var listStr = new List<string>();
            sonself.ForEach(i => listStr.Add(i.Id.ToString()));
            try
            {
                //查询我的报价单列表，搜索bx_car_order中未出单的记录
                var list = from ui in DataContextFactory.GetDataContext().bx_userinfo
                           where (isAgent ? listStr.Contains(ui.Agent) : ui.OpenId.Equals(strPass))
                           && ui.QuoteStatus > -1
                          && (!(from bco in DataContextFactory.GetDataContext().bx_car_order where bco.order_status == -2 || bco.order_status == -4 || (bco.order_status == -3 && (!ui.UpdateTime.HasValue || bco.create_time > ui.UpdateTime)) select bco.buid).Contains(ui.Id))
                           select 1;

                count = list.Count();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return count;
        }

        public long FindUserIdByAgentId(long agent)
        {
            string agentStr = agent.ToString();
            var user = DataContextFactory.GetDataContext().bx_userinfo.Where(x => x.Agent == agentStr && x.QuoteStatus > -1).OrderByDescending(x => x.UpdateTime).FirstOrDefault();
            return user == null ? 0 : user.Id;
        }

        /// <summary>
        /// 获取重复的历史数据
        /// </summary>
        /// <param name="topAgentId"></param>
        /// <param name="licenseno"></param>
        /// <returns></returns>
        public List<RepeatUserInfoModel> GetLicenseRepeat(int topAgentId, string licenseno)
        {
            try
            {
                StringBuilder sbId = new StringBuilder();
                sbId.Append("SELECT bx_userinfo.id as Buid,bx_userinfo.SixDigitsAfterIdCard,bx_userinfo.UpdateTime ");
                sbId.Append(" FROM bx_agent INNER JOIN bx_userinfo ON bx_agent.id=bx_userinfo.agent ");
                sbId.Append(" WHERE bx_agent.TopAgentId=@TopAgentId AND bx_userinfo.IsTest=0 AND bx_agent.IsUsed=1 ");
                if (!string.IsNullOrEmpty(licenseno))
                {
                    sbId.Append(" AND bx_userinfo.LicenseNo=@LicenseNo ");
                }
                #region 参数
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter()
                    {
                         ParameterName="TopAgentId",
                        Value=topAgentId,
                        MySqlDbType=MySqlDbType.Int32
                     },
                    new MySqlParameter()
                    {
                         ParameterName="LicenseNo",
                         Value=licenseno,
                         MySqlDbType=MySqlDbType.VarChar
                     }
                };
                #endregion
                return DataContextFactory.GetDataContext().Database.SqlQuery<RepeatUserInfoModel>(sbId.ToString(), sqlParams.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return new List<RepeatUserInfoModel>();
        }
        public List<bx_userinfo> GetUserinfoByLicenseAndAgent(string licenseNo, List<string> agentIds)
        {
            try
            {
                var model = DataContextFactory.GetDataContext().bx_userinfo
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
        public List<bx_userinfo> GetUserinfoByCarVinAndAgent(string carVin, List<string> agentIds)
        {
            try
            {
                var model = new EntityContext().bx_userinfo
                    .Where(x => x.IsTest == 0
                        && x.RenewalCarType == 0
                        && x.CarVIN == carVin
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
    }
}
