using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.CacheServices
{
    public class UserClaimCache : IUserClaimCache
    {
        public async Task<GetEscapedInfoResponse> GetList(GetEscapedInfoRequest request)
        {
            GetEscapedInfoResponse response = new GetEscapedInfoResponse();
            var chuxianCacheKey = CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);


            var chuxianKey = string.Format("{0}-claimdetai-key", chuxianCacheKey);
            var cacheValue = string.Format("{0}-claimdetail", chuxianCacheKey);
            //var chuxianKey = string.Format("{0}-claimdetail", chuxianCacheKey);
            //var cacheKey = CacheProvider.Get<IEnumerable<bx_claim_detail>>(chuxianKey);
            var cacheKey = CacheProvider.Get<string>(chuxianKey);
            if (cacheKey == null)
            {
                for (int i = 0; i < 100; i++)
                {
                    cacheKey = CacheProvider.Get<string>(chuxianKey);
                    if (!string.IsNullOrWhiteSpace(cacheKey))
                    {
                        break;
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
            }

            if (cacheKey != null)
            {
                if (cacheKey == "1")
                {
                    response.List = CacheProvider.Get<IEnumerable<bx_claim_detail>>(cacheValue).ToList();
                }
                else
                {
                    response.List = new List<bx_claim_detail>();
                }
            }
            else
            {
                response.List = null;
            }
            try
            {
                response.Lastinfo = CacheProvider.Get<bx_lastinfo>(string.Format("{0}-{1}", chuxianCacheKey, "lastinfo"));
            }
            catch (Exception ex) { response.Lastinfo = new bx_lastinfo(); }
            return response;
        }

        public async Task<GetViolationInfoResponse> GetList(GetViolationInfoRequest request)
        {
            GetViolationInfoResponse response = new GetViolationInfoResponse();
            var chuxianCacheKey = CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);


            var chuxianKey = string.Format("{0}-violation-key", chuxianCacheKey);
            ;
            var cacheValue = string.Format("{0}-violationl", chuxianCacheKey);
            var cacheKey = CacheProvider.Get<string>(chuxianKey);
            if (cacheKey == null)
            {
                for (int i = 0; i < 100; i++)
                {
                    cacheKey = CacheProvider.Get<string>(chuxianKey);
                    if (!string.IsNullOrWhiteSpace(cacheKey))
                    {
                        break;
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
            }

            if (cacheKey != null)
            {
                if (cacheKey == "1")
                {
                    response.List = CacheProvider.Get<IEnumerable<bx_violationlog>>(cacheValue).ToList();
                }
                else
                {
                    response.List = new List<bx_violationlog>();
                }
            }
            else
            {
                response.List = null;
            }

            return response;
        }
    }
}
