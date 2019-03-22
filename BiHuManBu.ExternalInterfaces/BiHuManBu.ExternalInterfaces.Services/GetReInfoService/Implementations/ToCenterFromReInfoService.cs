using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.CacheServices;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using ServiceStack.Text;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Common;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class ToCenterFromReInfoService : IToCenterFromReInfoService
    {
        private ICarInsuranceCache _carInsuranceCache;
        private IMessageCenter _messageCenter;

        public ToCenterFromReInfoService(ICarInsuranceCache carInsuranceCache, IMessageCenter messageCenter)
        {
            _carInsuranceCache = carInsuranceCache;
            _messageCenter = messageCenter;
        }

        public async Task<GetReInfoResponse> PushCenterService(GetReInfoRequest request, long buid, string reqCacheKey)
        {
            var response = new GetReInfoResponse();
            //续保选择其他保司续保时，只取行驶本信息，不发续保请求
            #region 发送续保消息
            string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(request.LicenseNo + request.RenewalCarType);
            var xuBaoKey = string.Format("{0}-xb-{1}", xuBaoCacheKey, "key");
            CacheProvider.Remove(xuBaoKey);
            object msgBody;
            if (request.RenewalSource <= 0)
            {
                msgBody = new
                {
                    B_Uid = buid,
                    IsCloseSms = 0,
                    NotifyCacheKey = xuBaoCacheKey,
                    IsForceRenewal = request.IsForceRenewal == 1//是否强制刷新续保
                };
            }
            else
            {
                msgBody = new
                {
                    B_Uid = buid,
                    IsCloseSms = 0,
                    //改为help类里面通用方法 by.20180904.gpj
                    RenewalSource = SourceGroupAlgorithm.ParseOldSource(request.RenewalSource).ToHashSet(),
                    NotifyCacheKey = xuBaoCacheKey,
                    IsForceRenewal = request.IsForceRenewal == 1//是否强制刷新续保
                };
                ///如果传进来的三个值都不为空  ShowPACheckCode=1
                if (!string.IsNullOrWhiteSpace(request.RequestKey) && request.PAUKey != 0 && request.YZMArea != null)
                {
                    msgBody = new
                    {
                        B_Uid = buid,
                        IsCloseSms = 0,
                        //改为help类里面通用方法 by.20180904.gpj
                        RenewalSource = SourceGroupAlgorithm.ParseOldSource(request.RenewalSource).ToHashSet(),
                        NotifyCacheKey = xuBaoCacheKey,
                        RequestKey = request.RequestKey,
                        PAUKey = request.PAUKey,
                        YZMArea = request.YZMArea,
                        IsForceRenewal = request.IsForceRenewal == 1//是否强制刷新续保
                    };
                }
            }
            //发送续保信息
            //var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(), ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["BxXuBaoName"]);
            string strurl = string.Format("{0}/service/getrenewal", ConfigurationManager.AppSettings["CenterNewUrl"]);
            string returnServerIpPort = string.Empty;
            string result = ProxyCenterHttpClient.Post(strurl, msgBody.ToJson(), 60, ref returnServerIpPort);

            #endregion
            #region 缓存读取
            ExecutionContext.SuppressFlow();
            response = await _carInsuranceCache.GetReInfo(request, buid);
            #region 改由中心 写 缓存
            response.ReqCarinfo = CacheProvider.Get<bx_quotereq_carinfo>(reqCacheKey);
            #endregion
            #endregion

            return response;
        }
    }
}
