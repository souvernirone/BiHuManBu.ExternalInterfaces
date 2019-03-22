using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using log4net;
using ServiceStack.Text;
using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using System.Net;
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Services.CacheServices
{
    public class CarInsuranceCache : ICarInsuranceCache
    {
        public async Task<GetReInfoResponse> GetReInfo(GetReInfoRequest request, long? buid = null)
        {
            ILog logInfo = LogManager.GetLogger("ERROR");
            //logInfo.Info("读取缓存数据开始" + request.ToJson());
            string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(request.LicenseNo + request.RenewalCarType);

            var xuBaoKey = string.Format("{0}-xb-{1}", xuBaoCacheKey, "key");
            var cacheKey = CacheProvider.Get<string>(xuBaoKey);

            #region

            //var step1val = string.Empty;
            //var step1va2 = string.Empty;
            //var step1va3 = string.Empty;
            //var step1va4 = string.Empty;
            StringBuilder sb = new StringBuilder();
            #endregion

            if (cacheKey == null)
            {
                for (int i = 0; i < 115; i++)
                {

                    cacheKey = CacheProvider.Get<string>(xuBaoKey);
                    //step1val = xuBaoKey;
                    //step1va2 = cacheKey;
                    if (!string.IsNullOrWhiteSpace(cacheKey))
                    {
                        if (cacheKey == "0" || cacheKey == "1" || cacheKey == "2")
                            break;
                    }
                    else
                    {
                        //ExecutionContext.SuppressFlow();
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        //ExecutionContext.RestoreFlow();
                    }
                    //logInfo.Info("读取缓存数据循环"+i+"     "+ request.ToJson() + ";  缓存key是step1va2：" + step1va2 + " --  key:step1va1" + step1val);
                    //sb.AppendLine("读取缓存数据循环" + i + "     " + request.ToJson() + ";  缓存key是step1va2：" + step1va2 +
                    //              " --  key:step1va1" + step1val);
                }
            }

            GetReInfoResponse response = new GetReInfoResponse();
            try
            {
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {


                    //续保 需要的表
                    //bx_userinfo
                    //bx_renewalquote
                    //bx_carinfo

                    //步骤1  续保的时候 会发送消息队列 ，这个时候 会把 key传过去eg:aaaa。

                    //步骤2   中心在续保的时候 ，需要根据这个key 设置一个开关 eg:aaaa-key：1,放在缓存中,成功的时候要置1，刚开始是空值
                    //等续保结束后，先将上面列出的表写入缓存 
                    //其中： 键值分别是：
                    //bx_userinfo        :aaaa-userinfo
                    //bx_car_renewal    :aaaa-renewal
                    //bx_carinfo         :aaaa-carinfo
                    //步骤3： 讲开关缓存设置续保完成标识：aaaa-key：1


                    //续保缓存标示（是否成功）
                    var renwal = CommonCacheKeyFactory.CreateKeyWithLicense(request.LicenseNo + request.RenewalCarType);

                    if (cacheKey == "1")
                    {
                        response.UserInfo =
                            CacheProvider.Get<bx_userinfo>(string.Format("{0}-{1}", xuBaoCacheKey, "userinfo"));

                        response.SaveQuote =
                            CacheProvider.Get<bx_car_renewal>(string.Format("{0}-{1}", renwal, "renewal"));
                        response.CarInfo = CacheProvider.Get<bx_carinfo>(string.Format("{0}-{1}", renwal, "carinfo"));
                        response.LastInfo = CacheProvider.Get<bx_lastinfo>(string.Format("{0}-{1}", renwal, "lastinfo"));
                        //续保返回保费addbygpj20180926
                        response.RenewalPremium = CacheProvider.Get<bx_car_renewal_premium>(string.Format("{0}-{1}", renwal, "premium"));
                        response.BusinessStatus = 1;
                    }
                    else if (cacheKey == "0")
                    {
                        #region 先获取平安验证码
                        if (buid != null)
                        {
                            if (request.ShowPACheckCode == 1 && string.IsNullOrWhiteSpace(request.RequestKey) && request.PAUKey == 0 && request.YZMArea == null)
                            {
                                /*
                                 * 如果平安续保返回需要验证码，缓存平安验证码信息
                                */
                                string payzmkey = string.Format("{0}-PAYZM", buid);
                                CenterPicCodeCacheModel payzmvalue = CacheProvider.Get<CenterPicCodeCacheModel>(payzmkey);
                                if (payzmvalue != null)
                                {
                                    response = new GetReInfoResponse();
                                    response.UserInfo = new bx_userinfo();
                                    response.CenterPicCodeCacheModel = payzmvalue;
                                    if (response.CenterPicCodeCacheModel.ErrCode == -210002)
                                    {
                                        response.BusinessStatus = -10009;
                                        response.BusinessMessage = "获取续保信息失败，请输入验证码坐标";
                                    }
                                    else if (response.CenterPicCodeCacheModel.ErrCode == -210003)
                                    {
                                        response.BusinessStatus = 0;
                                        response.BusinessMessage = "输入验证码错误";
                                    }
                                    else if (response.CenterPicCodeCacheModel.ErrCode == -210004)
                                    {
                                        response.BusinessStatus = 0;
                                        response.BusinessMessage = "输入超时";
                                    }
                                    response.Status = HttpStatusCode.OK;
                                    response.UserInfo.Id = (long)buid;
                                    return response;
                                }
                            }
                        }
                        #endregion
                        response.UserInfo =
                             CacheProvider.Get<bx_userinfo>(string.Format("{0}-{1}", xuBaoCacheKey, "userinfo"));
                        response.CarInfo = CacheProvider.Get<bx_carinfo>(string.Format("{0}-{1}", renwal, "carinfo"));

                        if (response.UserInfo.NeedEngineNo == 1)
                        {
                            response.BusinessStatus = 2;
                        }
                        else if (response.UserInfo.NeedEngineNo == 0) //去掉 && response.UserInfo.QuoteStatus == 0
                        {
                            response.BusinessStatus = 3;
                        }
                        else
                        {
                            response.BusinessStatus = 4;
                        }
                    }
                    else if (cacheKey == "2")
                    {
                        response.UserInfo =
                            CacheProvider.Get<bx_userinfo>(string.Format("{0}-{1}", xuBaoCacheKey, "userinfo"));
                        //var renwal = CommonCacheKeyFactory.CreateKeyWithLicense(request.LicenseNo);
                        response.SaveQuote = new bx_car_renewal();
                        response.RenewalPremium = new bx_car_renewal_premium();
                        //CacheProvider.Get<bx_car_renewal>(string.Format("{0}-{1}", renwal, "renewal"));
                        response.BusinessStatus = 8;
                        response.LastInfo = CacheProvider.Get<bx_lastinfo>(string.Format("{0}-{1}", renwal, "lastinfo"));
                        response.BusinessMessage = CacheProvider.Get<string>(string.Format("{0}-xb-company", xuBaoCacheKey));
                    }
                    //过户车的模型
                    response.TransferModelList = CacheProvider.Get<List<TransferModel>>(string.Format("{0}-xb-key-newcarinfo", xuBaoCacheKey));
                    return response;
                }
                else
                {
                    response.UserInfo = new bx_userinfo();
                    //如果没拿到值，初始化以下基础信息字段
                    response.UserInfo.LicenseNo = request.LicenseNo;
                    response.UserInfo.CarVIN = request.CarVin;
                    response.UserInfo.EngineNo = request.EngineNo;
                    response.BusinessStatus = 0;//缓存异常
                    response.BusinessMessage = "请求超时或缓存异常,请重试";
                    return response;
                }
            }
            catch (Exception ex)
            {
                logInfo.Info("读取缓存发生异常" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }

        /// <summary>
        /// 从缓存中取续保的bussinessstatus
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <returns></returns>
        public int GetReInfoStatus(string licenseNo)
        {
            string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(licenseNo);

            var xuBaoKey = string.Format("{0}-xb-{1}", xuBaoCacheKey, "key");
            var cacheKey = CacheProvider.Get<string>(xuBaoKey);

            if (string.IsNullOrWhiteSpace(cacheKey))
                return 10000002; //缓存异常

            if (cacheKey == "1")
            {
                return 1;
            }
            var userInfo =
                CacheProvider.Get<bx_userinfo>(string.Format("{0}-{1}", xuBaoCacheKey, "userinfo"));
            switch (cacheKey)
            {
                case "0":
                    switch (userInfo.NeedEngineNo)
                    {
                        case 1:
                            return 2;
                        case 0:
                            return 3;
                        default:
                            return 4;
                    }
                case "2":
                    return 8;
            }
            return 10000002;//缓存异常
        }

        public async Task<GetPrecisePriceReponse> GetPrecisePrice(GetPrecisePriceRequest request)
        {

            //获取报价信息和 核保信息，我发送的key都是一样的 ，不能按照险种信息hash，因为我取的时候 ，请求参数是没有险种的 ，
            // 如果换成的key跟险种信息有关，导致缓存中 会有过的垃圾数据，客户端用不到 。
            // 也就是说  缓存只存放最新的一条报价信息和核保信息
            GetPrecisePriceReponse response = new GetPrecisePriceReponse();
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            try
            {
                string baojiaCacheKey =
              CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);
                var baojiaKey = string.Format("{0}-{1}-bj-{2}", baojiaCacheKey, request.IntentionCompany, "key");
                //获取报价信息
                //需要的表
                //bx_userinfo
                //bx_LastInfo
                //bx_SaveQuote
                //bx_QuoteResult
                //bx_SubmitInfo
                //bx_renewalquote

                //步骤1  续保的时候 会发送消息队列 ，这个时候 会把 key传过去eg:bbbb。

                //步骤2   中心在续保的时候 ，需要根据这个key 设置3个开关(人太平) 
                //eg:
                //bbbb-bj-0-key：,
                //bbbb-bj-1-key：,
                //bbbb-bj-2-key：,
                //放在缓存中,人太平报价成功的时候分别要置1，刚开始是空值
                //等报价结束后，先将上面列出的表写入缓存 
                //其中： 键值分别是：
                //bx_userinfo        :bbbb-userinfo
                //bx_LastInfo        :bbbb-lastinfo
                //bx_car_renewal     :bbbb-renewal
                //bx_SaveQuote       :bbbb-savequote

                //bx_QuoteResult     :bbbb-0-quoteresult
                //bx_SubmitInfo      :bbbb-0-submitinfo
                var cacheKey = CacheProvider.Get<string>(baojiaKey);
                //ExecutionContext.SuppressFlow();

                if (cacheKey == null)
                {
                    for (int i = 0; i < 210; i++)
                    {
                        cacheKey = CacheProvider.Get<string>(baojiaKey);
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
                //ExecutionContext.RestoreFlow();
                if (cacheKey == "1")
                {
                    response.UserInfo = CacheProvider.Get<bx_userinfo>(string.Format("{0}-{1}", baojiaCacheKey, "userinfo"));
                    response.LastInfo = CacheProvider.Get<bx_lastinfo>(string.Format("{0}-{1}", baojiaCacheKey, "lastinfo"));
                    var renewal = CommonCacheKeyFactory.CreateKeyWithLicense(request.LicenseNo);
                    response.Renewal = CacheProvider.Get<bx_car_renewal>(string.Format("{0}-{1}", renewal, "renewal"));
                    response.SaveQuote =
                        CacheProvider.Get<bx_savequote>(string.Format("{0}-{1}", baojiaCacheKey, "savequote"));

                    response.QuoteResult =
                        CacheProvider.Get<bx_quoteresult>(string.Format("{0}-{1}-{2}", baojiaCacheKey,
                            request.IntentionCompany, "quoteresult"));
                    response.CarInfo = CacheProvider.Get<bx_quoteresult_carinfo>(string.Format("{0}-{1}-{2}", baojiaCacheKey,
                            request.IntentionCompany, "quoteresultcarinfo"));
                    response.SubmitInfo = CacheProvider.Get<bx_submit_info>(string.Format("{0}-{1}-{2}", baojiaCacheKey,
                            request.IntentionCompany, "submitinfo"));
                    response.BusinessStatus = 1;


                }
                else if (cacheKey == "0")
                {
                    response.UserInfo = CacheProvider.Get<bx_userinfo>(string.Format("{0}-{1}", baojiaCacheKey, "userinfo"));
                    response.LastInfo = CacheProvider.Get<bx_lastinfo>(string.Format("{0}-{1}", baojiaCacheKey, "lastinfo"));
                    response.SaveQuote =
                      CacheProvider.Get<bx_savequote>(string.Format("{0}-{1}", baojiaCacheKey, "savequote"));
                    var renewal = CommonCacheKeyFactory.CreateKeyWithLicense(request.LicenseNo);
                    response.Renewal = CacheProvider.Get<bx_car_renewal>(string.Format("{0}-{1}", renewal, "renewal"));
                    response.SubmitInfo = CacheProvider.Get<bx_submit_info>(string.Format("{0}-{1}-{2}", baojiaCacheKey,
                            request.IntentionCompany, "submitinfo"));
                    response.QuoteResult =
                        CacheProvider.Get<bx_quoteresult>(string.Format("{0}-{1}-{2}", baojiaCacheKey,
                            request.IntentionCompany, "quoteresult"));
                    response.BusinessStatus = 3;
                }
                string baojiaCacheCheckCodeIdKey = string.Format("{0}CheckCodeKey", baojiaCacheKey);
                var bk = CacheProvider.Get<string>(baojiaCacheCheckCodeIdKey);
                if (!string.IsNullOrWhiteSpace(bk))
                {
                    response.CheckCode = bk;
                }
                #region 新增请求记录
                var reqkey = string.Format("{0}-reqcarinfo", response.UserInfo.Id);
                response.ReqInfo = CacheProvider.Get<bx_quotereq_carinfo>(reqkey);
                #endregion
                //addby gpj 20181122 驾意险
                response.YwxDetails = CacheProvider.Get<List<bx_ywxdetail>>(string.Format("{0}-{1}-jyxinfos", baojiaCacheKey, request.IntentionCompany));
            }
            catch (Exception ex)
            {
                //stopwatch.Stop();

                ILog logError = LogManager.GetLogger("ERROR");

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + " 请求数据:" + request.ToJson() + "  响应结果：" + response.ToJson());
                throw;
            }
            //stopwatch.Stop();
            return response;
        }

        public async Task<GetSubmitInfoResponse> GetSubmitInfo(GetSubmitInfoRequest request)
        {
            GetSubmitInfoResponse response = new GetSubmitInfoResponse();

            //获取核保信息
            //需要的表
            //bx_userinfo
            //bx_SubmitInfo

            //步骤1  续保的时候 会发送消息队列 ，这个时候 会把 key传过去eg:cccc。

            //步骤2   中心在续保的时候 ，需要根据这个key 设置1（因为最多只有一家核保）个开关 eg:cccc-hb-key：1,放在缓存中,成功的时候要置1，刚开始是空值
            //等续保结束后，先将上面列出的表写入缓存 
            //其中： 键值分别是：
            //bx_SubmitInfo      :cccc-submitinfo

            //步骤3： 讲开关缓存设置核保完成标识：cccc-hb-0-key：1

            string hebaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicenseAndAgentAndCustKey(request.LicenseNo, request.Agent, request.CustKey + request.RenewalCarType);

            var hebaoKey = string.Format("{0}-{1}-hb-{2}", hebaoCacheKey, request.IntentionCompany, "key");
            //ExecutionContext.SuppressFlow();
            var cacheKey = CacheProvider.Get<string>(hebaoKey);
            if (cacheKey == null)
            {
                for (int i = 0; i < 220; i++)
                {

                    cacheKey = CacheProvider.Get<string>(hebaoKey);
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
            //ExecutionContext.RestoreFlow();
            string baojiaCacheOrderIdKey = string.Format("{0}OrderIdKey", hebaoCacheKey);
            var ck = CacheProvider.Get<string>(baojiaCacheOrderIdKey);


            string baojiaCacheCheckCodeIdKey = string.Format("{0}CheckCodeKey", hebaoCacheKey);
            var bk = CacheProvider.Get<string>(baojiaCacheCheckCodeIdKey);

            //CacheProvider.Set(baojiaCacheOrderIdKey, request.OrderId, 10800);//orderid 滴滴用 ，缓存3小时

            if (cacheKey == "1")
            {
                response.SubmitInfo = CacheProvider.Get<bx_submit_info>(string.Format("{0}-{1}-{2}", hebaoCacheKey,
                    request.IntentionCompany, "submitinfo"));

                response.BusinessStatus = 1;

            }
            else if (cacheKey == "0")
            {
                response.SubmitInfo = CacheProvider.Get<bx_submit_info>(string.Format("{0}-{1}-{2}", hebaoCacheKey,
                   request.IntentionCompany, "submitinfo"));
                response.CustKey = request.CustKey;
                response.BusinessStatus = 3;
            }

            response.CustKey = request.CustKey;
            if (!string.IsNullOrWhiteSpace(ck))
            {
                response.OrderId = ck;
            }
            if (!string.IsNullOrWhiteSpace(bk))
            {
                response.CheckCode = bk;
            }
            return response;
        }
    }
}
