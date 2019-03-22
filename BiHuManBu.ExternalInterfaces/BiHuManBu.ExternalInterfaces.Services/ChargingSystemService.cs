using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class ChargingSystemService : CommonBehaviorService, IChargingSystemService
    {
        private IAgentRepository _agentRepository;
        private ICacheHelper _cacheHelper;
        private ILog logError;
        private ILog logInfo;
        private IUserInfoRepository _userInfoRepository;
        private ICarRenewalRepository _carRenewalRepository;
        private ICarInfoRepository _carInfoRepository;
        private ILastInfoRepository _lastInfoRepository;


        public ChargingSystemService(IAgentRepository agentRepository, ICacheHelper cacheHelper, IUserInfoRepository userInfoRepository, ICarRenewalRepository carRenewalRepository, ICarInfoRepository carInfoRepository, ILastInfoRepository lastInfoRepository)
            : base(agentRepository, cacheHelper)
        {
            _agentRepository = agentRepository;
            _userInfoRepository = userInfoRepository;
            _carRenewalRepository = carRenewalRepository;
            _carInfoRepository = carInfoRepository;
            _lastInfoRepository = lastInfoRepository;
        }

        public ReListViewModel GetReInfoList(GetReInfoListRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var model = new ReListViewModel();

            var list = new List<Re>();
            int totalCount = 0;

            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                model.BusinessStatus = -10001;
                model.StatusMessage = "参数校验错误，请检查您的校验码";
                return model;
            }
            //此处对续保消费系统不做校验
            //if (!AppValidateReqest(pairs, request.SecCode))
            //{
            //    model.BusinessStatus = -10001;
            //    model.StatusMessage = "参数校验错误，请检查您的校验码";
            //    return model;
            //}
            try
            {
                //拼接where语句
                var sbWhere = new StringBuilder();
                sbWhere.Append(" QuoteStatus = -1 AND LENGTH(OpenId) > 9 AND IsTest=0 ");
                
                if (request.LastYearSource > -1)
                {
                    sbWhere.Append(string.Format(" AND LastYearSource = {0} ", request.LastYearSource));
                }
                if (request.RenewalStatus == 1)
                {
                    //sbWhere.Append(" AND (( NeedEngineNo=0 AND RenewalStatus!=1 ) ")
                    //    .Append(" OR ( NeedEngineNo=0 AND LastYearSource>-1 ) ")
                    //.Append(" OR ( RenewalStatus=1 )) ");
                    sbWhere.Append(" AND (( NeedEngineNo=0 AND RenewalStatus=0 ) ")
                        .Append(" OR ( RenewalStatus=1 )) ");
                }
                else if (request.RenewalStatus == 0)
                {
                    //sbWhere.Append(" AND NeedEngineNo=1 AND RenewalStatus=0 "); //sbWhere.Append(" AND ( NeedEngineNo=1 OR RenewalStatus!=1 )");
                    sbWhere.Append(" AND (( NeedEngineNo=1 AND RenewalStatus=0 )")
                        .Append(" OR ( RenewalStatus=-1 )) ");
                }
                if (request.IsOnlyMine.HasValue)
                {
                    if (request.IsOnlyMine.Value == 0)
                    {
                        //查询当前代理人及子集的agent
                        string agentids = _agentRepository.GetSonsIdToString(request.ChildAgent, true);
                        if (!string.IsNullOrEmpty(agentids))
                        {
                            sbWhere.Append(" AND Agent in (")
                                .Append(agentids)
                                .Append(") ");
                        }
                    }
                }
                else
                {
                    sbWhere.Append(string.Format(" AND Agent ='{0}' ", request.ChildAgent));
                }
                if (!string.IsNullOrWhiteSpace(request.LicenseNo))
                {
                    sbWhere.Append(string.Format(" AND (LicenseNo like '%{0}%' OR LicenseOwner like '%{0}%') ", request.LicenseNo.ToUpper()));
                }

                //查询列表
                var userinfo = new List<bx_userinfo>();
                userinfo = _userInfoRepository.FindReList(sbWhere.ToString(), request.PageSize, request.CurPage, out totalCount);

                //续保总数
                model.TotalCount = totalCount;
                if (totalCount < 0)
                {
                    model.BusinessStatus = 0;
                    model.StatusMessage = "没有续保记录";
                    return model;
                }
                if (userinfo.Count > 0)
                {
                    Re re;
                    foreach (var item in userinfo)
                    {
                        if (string.IsNullOrWhiteSpace(item.LicenseNo))
                            continue;
                        re = new Re();
                        re.Buid = item.Id;
                        //创建时间
                        if (item.CreateTime != null)
                            re.CreateTime = item.UpdateTime.HasValue ? item.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        re.LastYearSource = item.LastYearSource;
                        //配置基础信息
                        re.UserInfo =
                            item.ConvertToViewModel(_carRenewalRepository.FindCarRenewal(item.Id),
                                _carInfoRepository.Find(item.LicenseNo),
                                _lastInfoRepository.GetByBuid(item.Id));
                        //续保状态，默认情况是0，如果是获取到车辆信息和正常续保成功，会返回1
                        re.RenewalStatus = 0;
                        //续保判断返回信息
                        if (item.RenewalStatus == 1)
                        {
                            //续保成功
                            re.BusinessStatus = 1;
                            re.StatusMessage = "续保成功";
                            re.RenewalStatus = 1;
                        }
                        else//0和-1
                        {
                            if (item.NeedEngineNo == 1)
                            {
                                //需要完善行驶证信息
                                re.BusinessStatus = 2;
                                re.StatusMessage = "需要完善行驶证信息（车辆信息和险种都没有获取到）";
                                re.UserInfo.BusinessExpireDate = "";
                                re.UserInfo.ForceExpireDate = "";
                                re.UserInfo.NextBusinessStartDate = "";
                                re.UserInfo.NextForceStartDate = "";
                            }
                            if (item.NeedEngineNo == 0)
                            {
                                //获取车辆信息成功，但获取险种失败
                                re.BusinessStatus = 3;
                                re.StatusMessage = "获取车辆信息成功(车架号，发动机号，品牌型号及初登日期)，险种获取失败";
                                re.RenewalStatus = 1;//该情况属于续保成功
                                re.UserInfo.BusinessExpireDate = "";
                                re.UserInfo.ForceExpireDate = "";
                                re.UserInfo.NextBusinessStartDate = "";
                                re.UserInfo.NextForceStartDate = "";
                            }
                            //if (item.NeedEngineNo == 0 && item.LastYearSource > -1)
                            //{
                            //    //续保成功
                            //    re.BusinessStatus = 1;
                            //    re.StatusMessage = "续保成功";
                            //    re.RenewalStatus = 1;
                            //}
                            if (item.RenewalStatus == -1)
                            {
                                re.BusinessStatus = 0;
                                re.StatusMessage = "获取续保信息失败";
                                re.UserInfo.BusinessExpireDate = "";
                                re.UserInfo.ForceExpireDate = "";
                                re.UserInfo.NextBusinessStartDate = "";
                                re.UserInfo.NextForceStartDate = "";
                            }
                        }
                        #region 以前续保状态判断 注释掉
                        //if (item.NeedEngineNo == 1 && item.RenewalStatus == 0)//if (item.NeedEngineNo == 1)
                        //{
                        //    //需要完善行驶证信息
                        //    re.BusinessStatus = 2;
                        //    re.StatusMessage = "需要完善行驶证信息（车辆信息和险种都没有获取到）";
                        //    re.UserInfo.BusinessExpireDate = "";
                        //    re.UserInfo.ForceExpireDate = "";
                        //    re.UserInfo.NextBusinessStartDate = "";
                        //    re.UserInfo.NextForceStartDate = "";
                        //}
                        //if (item.RenewalStatus == -1)//if (item.LastYearSource == -1)
                        //{
                        //    re.BusinessStatus = 0;
                        //    re.StatusMessage = "获取续保信息失败";
                        //    re.UserInfo.BusinessExpireDate = "";
                        //    re.UserInfo.ForceExpireDate = "";
                        //    re.UserInfo.NextBusinessStartDate = "";
                        //    re.UserInfo.NextForceStartDate = "";
                        //}
                        //if (item.NeedEngineNo == 0 && item.RenewalStatus == 0)
                        //{
                        //    //获取车辆信息成功，但获取险种失败
                        //    re.BusinessStatus = 3;
                        //    re.StatusMessage = "获取车辆信息成功(车架号，发动机号，品牌型号及初登日期)，险种获取失败";
                        //    re.RenewalStatus = 1;//该情况属于续保成功
                        //    re.UserInfo.BusinessExpireDate = "";
                        //    re.UserInfo.ForceExpireDate = "";
                        //    re.UserInfo.NextBusinessStartDate = "";
                        //    re.UserInfo.NextForceStartDate = "";
                        //}
                        //if ((item.NeedEngineNo == 0 && item.LastYearSource > -1) || item.RenewalStatus == 1)//if (item.RenewalStatus == 1)//
                        //{
                        //    //续保成功
                        //    re.BusinessStatus = 1;
                        //    re.StatusMessage = "续保成功";
                        //    re.RenewalStatus = 1;
                        //}
                        #endregion
                        list.Add(re);
                    }
                    model.BusinessStatus = 1;
                    model.ReList = list;
                }
            }
            catch (Exception ex)
            {
                model.BusinessStatus = -10003;
                model.StatusMessage = "服务发生异常";
                logError.Info("续保列表接口请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return model;
        }

        public AppGetReInfoResponse GetReInfoDetail(GetReInfoDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new AppGetReInfoResponse();
            //bhToken校验
            //if (!AppTokenValidateReqest(request.BhToken, request.ChildAgent))
            //{
            //    response.BusinessStatus = -300;
            //    response.BusinessMessage = "登录信息已过期，请重新登录";
            //    return response;
            //}
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //if (!AppValidateReqest(pairs, request.SecCode))
            //{
            //    response.Status = HttpStatusCode.Forbidden;
            //    return response;
            //}

            try
            {
                //取bx_userinfo对象
                var userinfo = new bx_userinfo();
                if (request.Buid.HasValue && request.Buid.Value != 0)
                {
                    //如果传buid过来，重新赋值request
                    userinfo = _userInfoRepository.FindByBuid(request.Buid.Value);
                    if (userinfo != null)
                    {
                        request.LicenseNo = userinfo.LicenseNo;
                        request.ChildAgent = int.Parse(userinfo.Agent);
                    }
                }
                //else
                //{
                //    if (request.ChildAgent.HasValue)
                //    {
                //        //根据OpenId、车牌号、代理人Id找userinfo对象
                //        userinfo = _userInfoRepository.FindByAgentLicense(request.LicenseNo, request.ChildAgent.Value.ToString());
                //    }
                //}

                if (userinfo == null)
                {
                    response.Status = HttpStatusCode.NoContent;
                    return response;
                }
                else
                {
                    response.Buid = userinfo.Id;
                    //UserInfo
                    response.UserInfo = userinfo;
                    //增加当前userinfo的代理人返回
                    response.Agent = int.Parse(userinfo.Agent);

                    response.Status = HttpStatusCode.OK;
                    if (!string.IsNullOrEmpty(userinfo.LicenseNo))
                    {
                        response.SaveQuote = _carRenewalRepository.FindByLicenseno(userinfo.LicenseNo);
                        response.CarInfo = _carInfoRepository.Find(userinfo.LicenseNo);
                    }

                    if (userinfo.NeedEngineNo == 1)
                    {
                        //需要完善行驶证信息
                        response.BusinessStatus = 2;
                        response.BusinessMessage = "需要完善行驶证信息（车辆信息和险种都没有获取到）";
                    }
                    if (userinfo.NeedEngineNo == 0 && userinfo.RenewalStatus != 1)
                    {
                        //获取车辆信息成功，但获取险种失败
                        response.BusinessStatus = 3;
                        response.BusinessMessage = "获取车辆信息成功(车架号，发动机号，品牌型号及初登日期)，险种获取失败";
                    }
                    if ((userinfo.NeedEngineNo == 0 && userinfo.LastYearSource > -1) || userinfo.RenewalStatus == 1)
                    {
                        //续保成功
                        response.BusinessStatus = 1;
                        response.BusinessMessage = "续保成功";
                    }
                    if (userinfo.LastYearSource == -1)
                    {
                        response.BusinessStatus = 0;
                        response.BusinessMessage = "获取续保信息失败";
                    }
                    return response;

                }
            }
            catch (Exception ex)
            {
                response = new AppGetReInfoResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("APP续保详情接口请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }

    }
}
