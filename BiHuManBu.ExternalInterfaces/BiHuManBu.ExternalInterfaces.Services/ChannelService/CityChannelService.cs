using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Repository;
using BiHuManBu.ExternalInterfaces.Repository.DbOper;
using BiHuManBu.ExternalInterfaces.Services.CommonService.interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using ServiceStack.Text;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Services.ChannelService
{
    public class CityChannelService : ICityChannelService
    {
        private IRequestValidService _validService;
        private readonly IAgentPrivilegeService _agentPrivilege;
        private ILog _logError;

        public CityChannelService(IRequestValidService validService, IAgentPrivilegeService agentPrivilege)
        {
            _validService = validService;
            _agentPrivilege = agentPrivilege;
            _logError = LogManager.GetLogger("ERROR");
        }

        public GetSourceOfCityResponse GetSourceOfCity(GetSourceOfCityRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetSourceOfCityResponse response = new GetSourceOfCityResponse();
            try
            {
                var privilegeResulst = _agentPrivilege.CanUse(request.Agent);
                if (!privilegeResulst.CheckResult)
                {
                    response.Status = HttpStatusCode.Unauthorized;
                    return response;
                }
                if (!_validService.ValidateReqest(pairs, privilegeResulst.Model.SecretKey, request.SecCode))
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                //var items =
                //    _repository.SqlQuery(@"SELECT city_id AS CityCode ,CASE WHEN source=1 THEN 1 WHEN source=0 THEN 2 ELSE POW(2,source) END AS Source FROM  bx_agent_ukey    WHERE  city_id=" + request.CityCode + " GROUP BY city_id,source;");

                var items =
                    DataContextFactory.GetDataContext()
                        .Database.SqlQuery<CityChannelItem>(
                            @"SELECT city_id AS CityCode ,CASE WHEN source is null THEN  -1 WHEN source=1 THEN 1 WHEN source=0 THEN 2 ELSE POW(2,source) END AS Source FROM  bx_agent_ukey    WHERE  city_id=" + request.CityCode + " GROUP BY city_id,source;").ToList();
                 
                response.Result = items;
                response.Status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                response.Status = HttpStatusCode.ExpectationFailed;
                _logError.Info("获取重复投保请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + ",请求对象信息：" + request.ToJson() + ";返回对象信息" + response.ToJson());
            }

            return response;
        }
    }
}