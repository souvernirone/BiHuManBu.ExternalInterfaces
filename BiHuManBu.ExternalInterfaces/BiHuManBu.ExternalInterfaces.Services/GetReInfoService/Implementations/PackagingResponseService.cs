using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.LogBuriedPoint.LogCollection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class PackagingResponseService : IPackagingResponseService
    {
        private readonly IReWriteUserInfo _reWriteUserInfo;
        private static readonly string _gzcbAgentId = ConfigurationManager.AppSettings["gzcbAgentId"];
        private static readonly string _reInfoNoRelationSource = ConfigurationManager.AppSettings["ReInfoNoRelationSource"];
        private static readonly string _reInfoClearRelation = ConfigurationManager.AppSettings["ReInfoClearRelation"];
        private static readonly string _reInfoFailedSource = ConfigurationManager.AppSettings["ReInfoFailedSource"];
        private readonly IIsFalseReInfoService _isFalseReInfoService;
        private readonly IPostThirdPartService _postThirdPartService;
        public PackagingResponseService(IReWriteUserInfo reWriteUserInfo, IIsFalseReInfoService isFalseReInfoService,IPostThirdPartService postThirdPartService)
        {
            _reWriteUserInfo = reWriteUserInfo;
            _isFalseReInfoService = isFalseReInfoService;
            _postThirdPartService = postThirdPartService;
        }

        public async Task<GetReInfoViewModel> GetViewModel(GetReInfoRequest request, GetReInfoResponse response, int viewCityCode, string viewCustkey, int topAgent,string absolutori, BHFunctionLog fucnLog,string traceId)
        {
            GetReInfoViewModel viewModel = new GetReInfoViewModel();
            if (response.Status == HttpStatusCode.BadRequest || response.Status == HttpStatusCode.Forbidden)
            {
                viewModel.BusinessStatus = -10001;
                if (!string.IsNullOrWhiteSpace(response.BusinessMessage))
                {
                    viewModel.StatusMessage = response.BusinessMessage;
                }
                else
                {
                    viewModel.StatusMessage = "参数校验错误，请检查您的校验码";
                }

                return viewModel;
            }
            if (response.Status == HttpStatusCode.ExpectationFailed)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "服务发生异常";
                return viewModel;
            }
            else
            {
                viewModel.BusinessStatus = response.BusinessStatus;
                viewModel.StatusMessage = response.BusinessMessage;
                fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "续保信息的userinfo部分", "ConvertToViewModel", 1);
                AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { viewModel.UserInfo = response.UserInfo.ConvertToViewModel(response.SaveQuote, response.CarInfo, response.LastInfo, request.TimeFormat); });
                //显示商业险交强险投保单号 
                if (request.CanShowNo == 0)
                {
                    viewModel.UserInfo.BizNo = null;
                    viewModel.UserInfo.ForceNo = null;
                }
                if (request.CanShowExhaustScale == 0)
                {
                    viewModel.UserInfo.ExhaustScale = null;
                }
                viewModel.UserInfo.IsPublic = response.ReqCarinfo == null ? 0 : (response.ReqCarinfo.is_public.HasValue ? response.ReqCarinfo.is_public.Value : 0);
                if (request.ShowAutoMoldCode == 1)
                {
                    viewModel.UserInfo.AutoMoldCode = response.ReqCarinfo == null ? string.Empty : (string.IsNullOrWhiteSpace(response.ReqCarinfo.auto_model_code) ? string.Empty : response.ReqCarinfo.auto_model_code);
                }
                #region 关系人星号判断
                if (request.RenewalType == 2)
                {
                    viewModel.UserInfo = _reWriteUserInfo.ReWriteUserInfoService(viewModel.UserInfo, topAgent);
                }
                #endregion
                viewModel.UserInfo.CityCode = viewCityCode;
                viewModel.CustKey = viewCustkey;
                if (response.BusinessStatus == 1)
                {
                    viewModel.SaveQuote = response.SaveQuote.ConvetToViewModel();
                    //addbygupj 20180926 续保返回保费
                    if (request.ShowBaoFei == 1)
                    {
                        viewModel.XianZhong = response.RenewalPremium.ConvetToViewModel(response.SaveQuote);
                    }
                    if (request.Group > 0)
                    {
                        //此处原先if判断，改为调用转换方法 by.20180904.gpj
                        viewModel.SaveQuote.Source = SourceGroupAlgorithm.GetNewSource(Convert.ToInt32(viewModel.SaveQuote.Source));
                    }
                }
                else
                {
                    viewModel.SaveQuote = new SaveQuoteViewModel();
                    viewModel.SaveQuote.HcXiuLiChang = "0";
                    viewModel.SaveQuote.HcXiuLiChangType = "-1";
                    viewModel.SaveQuote.Fybc = "0";
                    viewModel.SaveQuote.FybcDays = "0";
                    viewModel.SaveQuote.SheBeis = new List<SheBei>();
                    viewModel.SaveQuote.SheBeiSunShi = "0";
                    viewModel.SaveQuote.BjmSheBeiSunShi = "0";
                    viewModel.SaveQuote.SanZheJieJiaRi = "0";
                    //addbygupj 20180926 续保返回保费
                    if (request.ShowBaoFei == 1)
                    {
                        viewModel.XianZhong = new XianZhong()
                        {
                            CheSun = new XianZhongUnit(),
                            SanZhe = new XianZhongUnit(),
                            DaoQiang = new XianZhongUnit(),
                            SiJi = new XianZhongUnit(),
                            ChengKe = new XianZhongUnit(),
                            BoLi = new XianZhongUnit(),
                            HuaHen = new XianZhongUnit(),
                            BuJiMianCheSun = new XianZhongUnit(),
                            BuJiMianSanZhe = new XianZhongUnit(),
                            BuJiMianDaoQiang = new XianZhongUnit(),
                            BuJiMianFuJia = new XianZhongUnit(),
                            BuJiMianChengKe = new XianZhongUnit(),
                            BuJiMianSiJi = new XianZhongUnit(),
                            BuJiMianHuaHen = new XianZhongUnit(),
                            BuJiMianSheShui = new XianZhongUnit(),
                            BuJiMianZiRan = new XianZhongUnit(),
                            BuJiMianJingShenSunShi = new XianZhongUnit(),
                            SheShui = new XianZhongUnit(),
                            ZiRan = new XianZhongUnit(),
                            HcSheBeiSunshi = new XianZhongUnit(),
                            HcHuoWuZeRen = new XianZhongUnit(),
                            HcJingShenSunShi = new XianZhongUnit(),
                            HcSanFangTeYue = new XianZhongUnit(),
                            HcXiuLiChang = new XianZhongUnit(),
                            Fybc = new XianZhongUnit(),
                            FybcDays = new XianZhongUnit(),
                            SheBeiSunShi = new XianZhongUnit(),
                            BjmSheBeiSunShi = new XianZhongUnit()
                        };
                    }
                }
                if (response.CenterPicCodeCacheModel != null)
                {
                    if (viewModel.PACheckCode == null)
                    {
                        viewModel.PACheckCode = new PACheckCode();
                    }
                    viewModel.PACheckCode.VerificationCode = response.CenterPicCodeCacheModel.VerificationCode;
                    viewModel.PACheckCode.RequestKey = response.CenterPicCodeCacheModel.RequestKey;
                    viewModel.PACheckCode.PAUKey = int.Parse(response.CenterPicCodeCacheModel.UKey);
                }
                if (response.BusinessStatus == 1)
                {
                    viewModel.StatusMessage = "续保成功";
                }
                else if (response.BusinessStatus == 2)
                {
                    viewModel.StatusMessage = "需要完善行驶证信息（车辆信息和险种都没有获取到）";
                }
                else if (response.BusinessStatus == 3)
                {
                    viewModel.StatusMessage = "获取车辆信息成功(车架号，发动机号，品牌型号及初登日期)，险种获取失败";
                }
                else if (response.BusinessStatus == -10002)
                {
                    viewModel.StatusMessage = "获取续保信息失败";
                }
                else if (response.BusinessStatus == 8)
                {
                    viewModel.UserInfo.ForceExpireDate = response.LastInfo.last_end_date;
                    viewModel.UserInfo.BusinessExpireDate = response.LastInfo.last_business_end_date;
                    if (!string.IsNullOrWhiteSpace(viewModel.UserInfo.ForceExpireDate))
                    {
                        var nb = DateTime.Parse(viewModel.UserInfo.ForceExpireDate);
                        if (nb.Date == DateTime.MinValue.Date)
                        {
                            viewModel.UserInfo.ForceExpireDate = "";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(viewModel.UserInfo.BusinessExpireDate))
                    {
                        var nb = DateTime.Parse(viewModel.UserInfo.BusinessExpireDate);
                        if (nb.Date == DateTime.MinValue.Date)
                        {
                            viewModel.UserInfo.BusinessExpireDate = "";
                        }
                    }
                    viewModel.StatusMessage = "投保公司：" + response.BusinessMessage + ";该车是续保期外的车或者是投保我司对接外的其他保险公司的车辆，这种情况，只能返回该车的投保日期(ForceExpireDate,BusinessExpireDate),险种取不到，不再返回";
                    viewModel.BusinessStatus = 1;
                }
                #region 广州人财保 特殊判断，屏蔽平安关系人，按一定比例不返回保司
                if (_gzcbAgentId.Equals(request.Agent.ToString()))
                {
                    #region 屏蔽关系人
                    if (_reInfoNoRelationSource.Split(new Char[] { ',' }).Contains(viewModel.SaveQuote.Source.ToString()))
                    {
                        viewModel.UserInfo.LicenseOwner = "";
                        viewModel.UserInfo.InsuredName = "";
                        viewModel.UserInfo.PostedName = "";
                        viewModel.UserInfo.IdType = 0;
                        viewModel.UserInfo.CredentislasNum = "";
                        viewModel.UserInfo.InsuredIdCard = "";
                        viewModel.UserInfo.InsuredIdType = 0;
                        viewModel.UserInfo.InsuredMobile = "";
                        viewModel.UserInfo.HolderName = "";
                        viewModel.UserInfo.HolderIdCard = "";
                        viewModel.UserInfo.HolderIdType = 0;
                        viewModel.UserInfo.HolderMobile = "";
                        viewModel.UserInfo.OwnerSex = "";
                        viewModel.UserInfo.OwnerBirthday = "";
                        viewModel.UserInfo.InsuredSex = "";
                        viewModel.UserInfo.InsuredBirthday = "";
                        viewModel.UserInfo.HolderSex = "";
                        viewModel.UserInfo.HolderBirthday = "";
                    }
                    #endregion
                    #region 重置source为-1 未取到保司
                    if (_isFalseReInfoService.IsFalseReInfo(request.Agent) &&
                        _reInfoFailedSource.Split(new Char[] { ',' }).Contains(viewModel.SaveQuote.Source.ToString()))
                    {
                        viewModel.SaveQuote.Source = -1;
                    }
                    #endregion
                }
                #endregion
                #region 爱保的只返回上年投保是人保的关系人信息
                //根据","拆分不同的代理人
                if (!string.IsNullOrEmpty(_reInfoClearRelation))
                {
                    string[] reAgent = _reInfoClearRelation.Split(';');
                    foreach (var itAgent in reAgent)
                    {
                        //获取代理人配置
                        string[] itemConfig = itAgent.Split(',');
                        //代理人配置读取，跟当前代理人比较
                        if (itemConfig[0].Equals(request.Agent.ToString()))
                        {
                            //根据.来拆分有哪些保司，不是授权的保司就置空关系人
                            if (!itemConfig[1].Split(new Char[] { '.' }).Contains(viewModel.SaveQuote.Source.ToString()))
                            {
                                viewModel.UserInfo.LicenseOwner = "";
                                viewModel.UserInfo.InsuredName = "";
                                viewModel.UserInfo.PostedName = "";
                                viewModel.UserInfo.IdType = 0;
                                viewModel.UserInfo.CredentislasNum = "";
                                viewModel.UserInfo.InsuredIdCard = "";
                                viewModel.UserInfo.InsuredIdType = 0;
                                viewModel.UserInfo.InsuredMobile = "";
                                viewModel.UserInfo.HolderName = "";
                                viewModel.UserInfo.HolderIdCard = "";
                                viewModel.UserInfo.HolderIdType = 0;
                                viewModel.UserInfo.HolderMobile = "";
                                viewModel.UserInfo.OwnerSex = "";
                                viewModel.UserInfo.OwnerBirthday = "";
                                viewModel.UserInfo.InsuredSex = "";
                                viewModel.UserInfo.InsuredBirthday = "";
                                viewModel.UserInfo.HolderSex = "";
                                viewModel.UserInfo.HolderBirthday = "";
                            }
                            break;
                        }
                    }
                }
                #endregion
                if (response.BusinessStatus != 1)
                {
                    viewModel.SaveQuote.Source = -1;
                }
                if (request.ShowXiuLiChangType == 0)
                {
                    viewModel.SaveQuote.HcXiuLiChang = null;
                    viewModel.SaveQuote.HcXiuLiChangType = null;
                }
                if (request.ShowFybc == 0)
                {
                    viewModel.SaveQuote.Fybc = null;
                    viewModel.SaveQuote.FybcDays = null;
                }
                if (request.ShowSheBei == 0)
                {
                    viewModel.SaveQuote.SheBeis = null;
                    viewModel.SaveQuote.SheBeiSunShi = null;
                    viewModel.SaveQuote.BjmSheBeiSunShi = null;
                }
                if (request.ShowSanZheJieJiaRi == 0)
                {
                    viewModel.SaveQuote.SanZheJieJiaRi = null;
                }
            }
            if (request.ShowInnerInfo == 0)
            {
                viewModel.UserInfo.Buid = null;
            }
            if (request.ShowRenewalCarType == 0)
            {
                viewModel.UserInfo.RenewalCarType = null;
            }
            if (request.ShowCarType == 0)
            {
                viewModel.UserInfo.CarType = null;
            }
            if (request.ShowOrg == 0)
            {
                viewModel.UserInfo.Organization = null;
            }
            if (request.ShowRelation == 0)
            {
                viewModel.UserInfo.OwnerBirthday = null;
                viewModel.UserInfo.OwnerSex = null;
                viewModel.UserInfo.HolderBirthday = null;
                viewModel.UserInfo.HolderSex = null;
                viewModel.UserInfo.InsuredBirthday = null;
                viewModel.UserInfo.InsuredSex = null;
            }
            if (request.ShowExpireDateNum == 1)
            {
                //计算还剩多少天
                if (viewModel.UserInfo != null)
                {
                    int dayminus = 0;
                    if (!string.IsNullOrEmpty(viewModel.UserInfo.ForceExpireDate))
                    {
                        dayminus = TimeHelper.GetDayMinus(DateTime.Parse(viewModel.UserInfo.ForceExpireDate), DateTime.Now);
                    }
                    else if (!string.IsNullOrEmpty(viewModel.UserInfo.BusinessExpireDate))
                    {
                        dayminus = TimeHelper.GetDayMinus(DateTime.Parse(viewModel.UserInfo.BusinessExpireDate), DateTime.Now);
                    }
                    viewModel.UserInfo.ExpireDateNum = dayminus.ToString();
                }
            }
            else
            {
                viewModel.UserInfo.ExpireDateNum = null;
            }
            if (request.ShowPACheckCode == 0)
            {
                viewModel.PACheckCode = null;
            }
            if (request.ShowTransferModel == 0)
            {
                viewModel.TransferModelList = null;
            }
            else
            {
                if (response.TransferModelList != null && response.TransferModelList.Any())
                {
                    viewModel.TransferModelList = response.TransferModelList.ConvertToViewModel();
                }
                else
                {
                    viewModel.TransferModelList = new List<Models.ReportModel.TransferModelNew>();
                }
            }
            //addbygupj 20180926 续保返回保费
            if (request.ShowBaoFei == 0)
            {
                viewModel.XianZhong = null;
            }
            //if (request.ShowRenewalCarModel == 1)
            //{
            //    if (response.CarModel != null)
            //    {
            //        string yearday = string.Empty;
            //        if (string.IsNullOrWhiteSpace(response.CarModel.VehicleYear))
            //        {
            //            yearday = string.Empty;
            //        }
            //        else if (response.CarModel.VehicleYear.Length == 4)
            //        {
            //            yearday = response.CarModel.VehicleYear;
            //        }
            //        else if (response.CarModel.VehicleYear.Length >= 6)
            //        {
            //            yearday = response.CarModel.VehicleYear.Substring(0, 6);
            //        }
            //        viewModel.UserInfo.RenewalCarModel = string.Format("{0}/{1}/{2}/{3}/{4}/{5}",
            //            response.CarModel.VehicleName, response.CarModel.VehicleAlias,
            //            response.CarModel.VehicleExhaust.HasValue
            //                ? response.CarModel.VehicleExhaust.Value.ToString("f3")
            //                : "0", response.CarModel.VehicleSeat,
            //            response.CarModel.PriceT.HasValue ? response.CarModel.PriceT.Value.ToString("f1") : "0"
            //            , yearday);
            //    }
            //    else
            //    {
            //        viewModel.UserInfo.RenewalCarModel = string.Empty;
            //    }
            //}


            #region 摄像头用户向第三方推送续保消息
            if (request.RenewalType == 3)
            {
                fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "续保调用第三方接口传数据", "PostThirdPart", 1);
                AspectF.Define.InfoFunctionLog(fucnLog).Do(() => { _postThirdPartService.PostThirdPart(topAgent, viewModel); });
            }
            #endregion
            #region 多家绑定摄像头

            if (request.RenewalType == 3)
            {
                if (request.Agent == 88794)
                {
                    fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "绑定摄像头，获取续保信息", "GetRenewalRequest", 1);
                    AspectF.Define.InfoFunctionLog(fucnLog).Do(() =>
                    {
                        Task.Factory.StartNew(() =>
                        {
                            GetRenewalRequest(88798, ("88798").GetMd5().ToUpper(), request.LicenseNo, request.CityCode.ToString(), request.RenewalCarType.ToString(), absolutori, 88798, request.CameraId);
                        });
                    });
                    fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "绑定摄像头，获取续保信息", "GetRenewalRequest", 1);
                    AspectF.Define.InfoFunctionLog(fucnLog).Do(() =>
                    {
                        Task.Factory.StartNew(() =>
                        {
                            GetRenewalRequest(88797, ("88797").GetMd5().ToUpper(), request.LicenseNo, request.CityCode.ToString(), request.RenewalCarType.ToString(), absolutori, 88797, request.CameraId);
                        });
                    });
                }

            }
            //改成配置
            fucnLog = LogAssistant.GenerateBHFuncLog(traceId, "更多摄像头绑定", "MoreCameraBindings", 1);
            await AspectF.Define.InfoFunctionLog(fucnLog).Return(() => { return MoreCameraBindings(request, absolutori); });

            #endregion

            return viewModel;
        }

        private async Task MoreCameraBindings(GetReInfoRequest request,string absolutori)
        {
            var apiConfig = ConfigurationManager.AppSettings["moreBindings"];
            if (!string.IsNullOrWhiteSpace(apiConfig))
            {
                var arr = apiConfig.Split(';');
                foreach (string group in arr)
                {
                    var agentlist = group.Split(',');
                    for (int i = 0; i < agentlist.Length; i++)
                    {
                        if (i == 0 && request.Agent.ToString() == agentlist[0])
                        {
                            for (int j = 1; j < agentlist.Length; j++)
                            {
                                await Task.Factory.StartNew(() =>
                                {
                                    GetRenewalRequest(int.Parse(agentlist[j]), (agentlist[j]).GetMd5().ToUpper(),
                                        request.LicenseNo, request.CityCode.ToString(),
                                        request.RenewalCarType.ToString(), absolutori, int.Parse(agentlist[j]), request.CameraId);
                                });
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        private async Task GetRenewalRequest(int agent, string custkey, string licenseno, string citycode, string renewalcartype, string absolutori, int childAgent, string cameraId)
        {
            StringBuilder sb = new StringBuilder();
            if (childAgent > 0)
            {
                sb.Append("&ChildAgent=").Append(childAgent);
            }
            if (!string.IsNullOrWhiteSpace(cameraId))
            {
                sb.Append("&CameraId=").Append(cameraId);
            }
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(string.Format("http://{0}", absolutori));
                var getUrl = string.Format("/api/CarInsurance/getreinfo?LicenseNo={0}&CityCode={3}&Agent={1}&CustKey={2}&RenewalType=3&RenewalCarType={4}&SecCode=26c36c197e012063857e0e89dbf8aaf5{5}", licenseno, agent, custkey, citycode, renewalcartype, sb.ToString());
                await client.GetAsync(getUrl);
            }
        }
    }
}
