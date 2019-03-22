using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IUserInfoRepository : IRepositoryBase<bx_userinfo>
    {
        bx_userinfo Find(int userId);
        bx_userinfo FindByBuid(long buid);
        bx_userinfo FindByBuidSync(long buid);
        bx_userinfo FindByOpenIdAndLicense(string openid, string licenseno);
        //bx_userinfo FindByOpenIdAndLicense(string openid, string licenseno, string agent,int cartype);
        bx_userinfo FindByOpenIdAndLicense(string openid, string licenseno, string agent, int cartype, int istest = 0);
        bx_userinfo FindByAgentLicense(string licenseno, string agent);
        bx_userinfo FindByCarvin(string carvin, string engineno, string openid, string agent, int cartype, int isTest = 0);
        /// <summary>
        /// 续保按车架号续保，copy FindByCarvin方法，去掉engineno
        /// 20171026
        /// </summary>
        /// <param name="carvin"></param>
        /// <param name="openid"></param>
        /// <param name="agent"></param>
        /// <param name="cartype"></param>
        /// <param name="isTest"></param>
        /// <returns></returns>
        bx_userinfo FindByCarvin(string carvin, string openid, string agent, int cartype, int isTest = 0);
        long Add(bx_userinfo userinfo);
        int Update(bx_userinfo userinfo);
        int UpdateSync(bx_userinfo userinfo);

        /// <summary>
        /// app端的续保列表
        /// </summary>
        /// <param name="isAgent"></param>
        /// <param name="sonself"></param>
        /// <param name="strPass"></param>
        /// <param name="licenseNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="curPage"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<bx_userinfo> FindReInfoList(bool isAgent, List<bx_agent> sonself, string strPass, string licenseNo, int pageSize, int curPage, out int totalCount);

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
        List<bx_userinfo> FindReList(string strWhere, int pageSize, int curPage, out int totalCount);
        //List<bx_userinfo> FindReList(bool isAgent, List<bx_agent> sonself, string strPass, string licenseNo, int? lastYearSource, int? renewalStatus, int pageSize, int curPage, out int totalCount);
        //List<bx_userinfo> FindByAgentAndLicense(bool isAgent, List<bx_agent> sonself, string strPass, string licenseNo, int pageSize, int curPage, out int totalCount);
        /// <summary>
        /// 获取我的报价单列表
        /// </summary>
        /// <param name="hasOutOrder"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="curPage"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<bx_userinfo> GetMyBjdList(bool hasOutOrder, string strWhere, int pageSize, int curPage, out int totalCount);

        List<bx_userinfo> ReportForReInfoList(List<bx_agent> sonself, string strDate, string licenseNo, out int totalCount);
        //List<long> FindBuidListByLicenseno(string licenseno);
        List<bx_userinfo> FindAgentListByLicenseNo(string lecenseNo);
        bx_userinfo FindAgentListByLicenseNo(string licenseNo, List<string> agentIds);

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
        List<UserInfoModel> FindMyList(bool isAgent, string strAgents, string strPass, string licenseNo,
            int pageSize, int curPage, int orderBy, out int totalCount);

        /// <summary>
        /// 获取我的报价单的数量
        /// </summary>
        /// <param name="isAgent"></param>
        /// <param name="sonself"></param>
        /// <param name="strPass"></param>
        /// <returns></returns>
        int CountBaojia(bool isAgent, List<bx_agent> sonself, string strPass);

        /// <summary>
        /// 根据AgentId获取用户ID
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        long FindUserIdByAgentId(long agent);

        /// <summary>
        /// 获取重复的历史数据
        /// </summary>
        /// <param name="topAgentId"></param>
        /// <param name="licenseno"></param>
        /// <returns></returns>
        List<RepeatUserInfoModel> GetLicenseRepeat(int topAgentId, string licenseno);
        List<bx_userinfo> GetUserinfoByLicenseAndAgent(string licenseNo, List<string> agentIds);
        List<bx_userinfo> GetUserinfoByCarVinAndAgent(string carVin, List<string> agentIds);
    }
}
