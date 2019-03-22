using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using ServiceStack.Text;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class CheckCarNeedDrivingInfoService : ICheckCarNeedDrivingInfoService
    {
        private IMessageCenter _messageCenter;
        public CheckCarNeedDrivingInfoService(IMessageCenter messageCenter)
        {
            _messageCenter = messageCenter;
        }
        public async Task<bool> GetInfo(bx_userinfo userinfo)
        {
            #region 修改前的方法
            //bool isSuccess = true;
            //var requestmodel = new
            //{
            //    //Mobile = userinfo.Mobile,
            //    LicenseNo = userinfo.LicenseNo,
            //    CityCode = userinfo.CityCode,
            //    RenewalIdNo = userinfo.RenewalIdNo,
            //    Agent = userinfo.Agent,
            //    Id = userinfo.Id
            //};
            //logInfo.Info("行驶证信息请求进来额:" + requestmodel.ToJson());
            //try
            //{
            //    using (var client = new HttpClient())
            //    {
            //        client.BaseAddress = new Uri(_url);
            //        HttpContent content = new StringContent(requestmodel.ToJson());
            //        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //        HttpResponseMessage responseCheck = client.PostAsync("api/userinfo/CheckNeedAdd", content).Result;
            //        var resultJson = responseCheck.Content.ReadAsStringAsync().Result;
            //        if (resultJson == "false")
            //        {
            //            logInfo.Info("太平洋check信息返回成功:" + requestmodel.ToJson());
            //        }
            //        else
            //        {
            //            isSuccess = false;
            //            logError.Info("太平洋check信息返回失败:" + requestmodel.ToJson());
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    logError.Info("checkneed 出现异常:" + requestmodel.ToJson());
            //    return false;
            //}
            //return isSuccess;
            #endregion
            bool isSuccess = false;
            string xuBaoCacheKey = userinfo.Id.ToString();
            var msgBody = new
            {
                B_Uid = userinfo.Id,
                NotifyCacheKey = xuBaoCacheKey
            };
            //发送获取车辆信息队列
            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                ConfigurationManager.AppSettings["MessageCenter"],
                 ConfigurationManager.AppSettings["BxVehicle"]);
            var cacheKey = string.Format("{0}-findvehicle", xuBaoCacheKey);
            var cacheValue = CacheProvider.Get<string>(cacheKey);
            if (cacheValue == null)
            {
                for (int i = 0; i < 60; i++)
                {
                    cacheValue = CacheProvider.Get<string>(cacheKey);
                    if (!string.IsNullOrWhiteSpace(cacheValue))
                    {
                        break;
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(500));
                    }
                }
            }
            return isSuccess;
        }
    }
}
