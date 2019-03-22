using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.CacheServices;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services
{
    /// <summary>
    /// 出险信息
    /// </summary>
    public class UserClaimService : CommonBehaviorService,IUserClaimService
    {
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly ILoginService _loginService;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IAgentRepository _agentRepository;
        private ICacheHelper _cacheHelper;
        private IUserClaimCache _userClaimCache;
       
        public UserClaimService(IUserClaimRepository userClaimRepository,ILoginService loginService,IUserInfoRepository userInfoRepository,
            IAgentRepository agentRepository,ICacheHelper cacheHelper,IUserClaimCache userClaimCache):base(agentRepository,cacheHelper)
        {
            _userClaimRepository = userClaimRepository;
            _userInfoRepository = userInfoRepository;
            _loginService = loginService;
            _agentRepository = agentRepository;
            _userClaimCache = userClaimCache;
        }

        public async Task<GetEscapedInfoResponse> GetList(GetEscapedInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetEscapedInfoResponse response = new GetEscapedInfoResponse();
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
         
            //参数校验
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            try
            {
                //微信端逻辑 次级代理
                if (request.ChildAgent > 0)
                {
                    request.Agent = request.ChildAgent;
                }
                response = await _userClaimCache.GetList(request);

                if (response == null)
                {
                    response = new GetEscapedInfoResponse();
                    response.Status = HttpStatusCode.ExpectationFailed;
                    //response.List = new List<bx_claim_detail>();
                }
              
                ////if (response == null)中心没写缓存
                ////{
                //    response = new GetEscapedInfoResponse();
                //    bx_userinfo userinfo  = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo,request.Agent.ToString());
                  
                //    if (userinfo == null)
                //    {
                //        response.Status = HttpStatusCode.BadRequest;
                //        return response;
                //    }

                //    var userclaims = _userClaimRepository.FindList(userinfo.Id);
                //    //if (userclaims != null && userclaims.Count>0)
                //    //{
                //    //    var chuxianCacheKey = CommonCacheKeyFactory.CreateKeyWithLicenseAndAgent(request.LicenseNo,request.Agent);
                //    //    var chuxianKey = string.Format("{0}-cx", chuxianCacheKey);
                //    //    CacheProvider.Set(chuxianKey, userclaims.ToJson());
                //    //}
                //    response.List = userclaims;
                //    response.Status = HttpStatusCode.OK;
                ////}
               
            }
            catch (Exception ex)
            {
                response = new GetEscapedInfoResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
               
            }
            return response;
        }

        public async Task<GetViolationInfoResponse> GetViolationList(GetViolationInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetViolationInfoResponse response = new GetViolationInfoResponse();
            //根据经纪人获取手机号 
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            //参数校验
            if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            try
            {
                //微信端逻辑 次级代理
                if (request.ChildAgent > 0)
                {
                    request.Agent = request.ChildAgent;
                }
                response = await _userClaimCache.GetList(request);

                if (response == null)
                {
                    response = new GetViolationInfoResponse();
                    response.Status = HttpStatusCode.ExpectationFailed;
                   
                }

            }
            catch (Exception ex)
            {
                response = new GetViolationInfoResponse();
                response.Status = HttpStatusCode.ExpectationFailed;

            }
            return response;
        } 

    }
}
