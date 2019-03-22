using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.GetCenterValueService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using log4net;
using ServiceStack.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class TempDemoShowService : ITempDemoShowService
    {
        private static string _proxyAgent = ConfigurationManager.AppSettings["ProxyAgent"];
        private static string _proxyLicense = ConfigurationManager.AppSettings["ProxyLicense"];
        private readonly IGetCenterValueService _getCenterValueService;
        private readonly ILog logError = LogManager.GetLogger("ERROR");
        public TempDemoShowService(IGetCenterValueService getCenterValueService)
        {
            _getCenterValueService = getCenterValueService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool BackTempDemoShow(PostPrecisePriceRequest request, out string tempshowKey)
        {
            tempshowKey = string.Empty;
            //1是否吉利代理
            if (!_proxyAgent.Equals(request.Agent.ToString()))
            {
                return false;
            }
            if (!_proxyLicense.Contains(request.LicenseNo))
            {
                return false;
            }
            //2是否开启配置;1开启0关闭，控制权限请联系中心
            string isOpen = _getCenterValueService.GetValue("独立KV", "", "TempDemoShow");
            if (isOpen.Equals("0"))
            {
                logError.Info(string.Format("请求{0},{1},{2},中心开关开启", request.CustKey, request.LicenseNo, request.CarVin));
                return false;
            }
            //3是否存在历史报价
            PostPrecisePriceRequest req = new PostPrecisePriceRequest();
            req = request;
            //...
            req.SecCode = null;
            //...
            req.InsuredMobile = null;
            req.InsuredAddress = null;
            req.InsuredEmail = null;
            req.InsuredCertiStartdate = null;
            req.InsuredCertiEnddate = null;
            //...
            req.HolderMobile = null;
            req.HolderAddress = null;
            req.HolderEmail = null;
            req.HolderCertiStartdate = null;
            req.HolderCertiEnddate = null;
            //...
            req.InsuredSex = 0;
            req.HolderSex = 0;
            req.OwnerSex = 0;
            req.InsuredAuthority = null;
            req.HolderAuthority = null;
            req.OwnerAuthority = null;
            req.InsuredNation = null;
            req.HolderNation = null;
            req.OwnerNation = null;
            req.InsuredBirthday = null;
            req.HolderBirthday = null;
            req.OwnerBirthday = null;
            //...
            req.BhToken = null;
            //...
            req.Mobile = null;
            req.Email = null;
            req.MultiChannels = null;
            req.IsOrderChangeRelation = 0;

            tempshowKey = string.Format("{0}_{1}_{2}", request.Agent, request.CustKey.ToUpper(), req.ToJson().GetMd5());//顶级+custkey+seccode
            logError.Info(string.Format("缓存md5前的串：{0}", req.ToJson()));
            string cacheValue = CacheProvider.Get<string>(tempshowKey);
            if (string.IsNullOrWhiteSpace(cacheValue))
            {
                logError.Info(string.Format("请求{0},{1},{2},不存在历史报价", request.CustKey, request.LicenseNo, request.CarVin));
                //如果中间缓存报过价，继续往下走；如果没报过价，直接返回false
                return false;
            }
            #region 4历史报价是否报价成功&&核保成功
            //4历史报价是否报价成功&&核保成功
            if (req.ChildAgent > 0)
            {
                req.Agent = req.ChildAgent;
            }
            //报价key
            string baojiaCacheKey =
              CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, req.Agent, request.CustKey + request.RenewalCarType);
            List<int> oldquote = SourceGroupAlgorithm.ParseOldSource(request.QuoteGroup);
            if (!oldquote.Any())
            {
                //如果报价没值，直接返回false。<不过该情况不存在，quotegroup需要大于0>
                return false;
            }
            foreach (var item in oldquote)
            {
                //取不同的保司，看缓存key是否都存在
                var baojiaKey = string.Format("{0}-{1}-bj-{2}", baojiaCacheKey, item, "key");
                var cachebaojiavalue = CacheProvider.Get<string>(baojiaKey);
                if (string.IsNullOrWhiteSpace(cachebaojiavalue))
                {
                    cachebaojiavalue = "0";
                }
                if (!cachebaojiavalue.Equals("1"))
                {
                    logError.Info(string.Format("请求{0},{1},{2},报价{3},历史报价{4}失败", request.CustKey, request.LicenseNo, request.CarVin, request.QuoteGroup, item));
                    return false;
                }
            }
            List<int> oldsubmit = SourceGroupAlgorithm.ParseOldSource(request.SubmitGroup);
            if (!oldsubmit.Any())
            {
                //如果核保没值，直接返回false
                return false;
            }
            foreach (var item in oldquote)
            {
                //取不同的保司，看缓存key是否都存在
                var baojiaKey = string.Format("{0}-{1}-hb-{2}", baojiaCacheKey, item, "key");
                var cachehebaovalue = CacheProvider.Get<string>(baojiaKey);
                if (string.IsNullOrWhiteSpace(cachehebaovalue))
                {
                    cachehebaovalue = "0";
                }
                if (!cachehebaovalue.Equals("1"))
                {
                    logError.Info(string.Format("请求{0},{1},{2},核保{3},历史核保{4}失败", request.CustKey, request.LicenseNo, request.CarVin, request.SubmitGroup, item));
                    return false;
                }
            }
            #endregion
            //5如果成功，则保存seccode
            //6以上均成立返回true
            return true;
        }





    }
}
