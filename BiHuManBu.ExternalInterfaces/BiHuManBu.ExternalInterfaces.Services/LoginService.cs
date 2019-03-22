using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class LoginService : ILoginService
    {
        private static readonly string _userCenter = System.Configuration.ConfigurationManager.AppSettings["UserCenter"];
        private  ILog logError ;
        private ILog logInfo ;
        private ICacheHelper _cacheHelper;

        public LoginService(ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
            logError = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");
        }

        public async Task<Account> LoginAccount(string mobile)
        {
            const int cacheTime = 360;
            string accountCacheKey = string.Format("account_cache_key_{0}", mobile);
            string accountCacheTokenKey = string.Format("{0}_{1}__", accountCacheKey, "token");
            var accountCacheToken = _cacheHelper.Get(accountCacheTokenKey);
            var account = _cacheHelper.Get(accountCacheKey) as Account;

            if (accountCacheToken != null)
            {
                return account;
            }
            lock (accountCacheTokenKey)
            {
                accountCacheToken = _cacheHelper.Get(accountCacheTokenKey);
                if (accountCacheToken != null)
                {
                    return account;
                }
                _cacheHelper.Add(accountCacheTokenKey, "1", cacheTime);

                ThreadPool.QueueUserWorkItem(async (arg) =>
                {
                    using (
               HttpClient client =
                   new HttpClient(new HttpClientHandler
                   {
                       AutomaticDecompression =
                           System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
                   }))
                    {

                        client.BaseAddress = new Uri(_userCenter);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var getLoginUrl = string.Format("api/Account/GetWapLogin?mobile={0}&code={1}", mobile, "");
                        HttpResponseMessage responseLogin = await client.GetAsync(getLoginUrl);
                        if (responseLogin.IsSuccessStatusCode)
                        {
                            var resultLogin = await responseLogin.Content.ReadAsStringAsync();
                            account = resultLogin.FromJson<Account>();
                            _cacheHelper.Add(accountCacheKey,account,cacheTime*2);
                        }
                        else
                        {
                            string loginErrorMsg = string.Format("手机号登录失败:" + mobile);
                            logError.Info(loginErrorMsg);
                        }
                    }
                });
            }

            if (account == null)
            {
                using (
              HttpClient client =
                  new HttpClient(new HttpClientHandler
                  {
                      AutomaticDecompression =
                          System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
                  }))
                {

                    client.BaseAddress = new Uri(_userCenter);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var getLoginUrl = string.Format("api/Account/GetWapLogin?mobile={0}&code={1}", mobile, "");
                    HttpResponseMessage responseLogin = await client.GetAsync(getLoginUrl);
                    if (responseLogin.IsSuccessStatusCode)
                    {
                        var resultLogin = await responseLogin.Content.ReadAsStringAsync();
                        account = resultLogin.FromJson<Account>();
                    }
                    else
                    {
                        string loginErrorMsg = string.Format("手机号登录失败:" + mobile);
                        logError.Info(loginErrorMsg);
                    }
                }
            }

           
            return account;
        }
    }
}
