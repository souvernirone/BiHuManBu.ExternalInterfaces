using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using System;
using System.Configuration;

namespace BiHuManBu.ExternalInterfaces.Services.Factories
{
    public class UserinfoMakeFactory
    {
        private static readonly string _isAddIdCardBack6 = System.Configuration.ConfigurationManager.AppSettings["IsAddIdCardBack6"];
        public static bx_userinfo Save(GetReInfoRequest request, int roleType, IUserInfoRepository infoRepository, int topAgentId = 0)
        {
            var insertUserinfo = new bx_userinfo
            {
                top_agent_id = topAgentId,
                agent_id = request.Agent,
                IsLastYear = 0,
                LastYearSource = -1,
                //Source = -1,
                //Mobile = agentModel.Mobile,
                LicenseNo = request.LicenseNo,
                Agent = request.Agent.ToString(),
                //UserId = account.UserId,//此字段废弃
                OpenId = request.CustKey,
                CityCode = request.CityCode.ToString(),
                //IsPublic = request.IsPublic,
                QuoteStatus = -1,
                RenewalStatus = -1,
                CreateTime = DateTime.Now,
                RenewalType = request.RenewalType,
                //IsLastNewCar = request.IsLastYearNewCar,
                //EngineNo = request.EngineNo,
                //CarVIN = request.CarVin,
                NeedEngineNo = 1,
                IsDistributed = 0,
                UpdateTime = DateTime.Now,
                IsSingleSubmit = 0,
                Source = 0,
                OwnerIdCardType = -1,
                IsReView = 0,
                IsTest = 0

            };
            insertUserinfo.RenewalCarType = request.RenewalCarType;
            if (!string.IsNullOrWhiteSpace(request.SixDigitsAfterIdCard))
            {
                insertUserinfo.SixDigitsAfterIdCard = request.SixDigitsAfterIdCard;
            }
            if (request.IsLastYearNewCar == 2)
            {
                insertUserinfo.NeedEngineNo = 1;
                if (!string.IsNullOrWhiteSpace(request.EngineNo))
                {
                    insertUserinfo.EngineNo = request.EngineNo.ToUpper();
                }
                if (!string.IsNullOrWhiteSpace(request.CarVin))
                {
                    insertUserinfo.CarVIN = request.CarVin.ToUpper();
                }
            }
            //车架号及发动机号全部都有的情况 ，没有测试 ，先注释
            //if (request.IsLastYearNewCar == 1 &
            //   (!string.IsNullOrWhiteSpace(request.CarVin) && !string.IsNullOrWhiteSpace(request.EngineNo)))
            //{
            //    insertUserinfo.LicenseNo = request.LicenseNo;
            //    insertUserinfo.MoldName = string.Empty;
            //    insertUserinfo.RegisterDate = string.Empty;
            //    if (!string.IsNullOrWhiteSpace(request.EngineNo))
            //    {
            //        insertUserinfo.EngineNo = request.EngineNo.ToUpper();
            //    }

            //    if (!string.IsNullOrWhiteSpace(request.CarVin))
            //    {
            //        insertUserinfo.CarVIN = request.CarVin.ToUpper();
            //    }
            //    insertUserinfo.NeedEngineNo = 1;
            //}
            if (!string.IsNullOrWhiteSpace(request.CarOwnersName))
            {
                insertUserinfo.UserName = request.CarOwnersName;
                insertUserinfo.LicenseOwner = request.CarOwnersName;
                insertUserinfo.InsuredName = request.CarOwnersName;
            }
            if (!string.IsNullOrWhiteSpace(request.IdCard))
            {
                insertUserinfo.IdCard = request.IdCard.ToUpper();
                insertUserinfo.InsuredIdCard = request.IdCard.ToUpper();
            }
            /* 摄像头进店修改进店参数
             * addbygpj20180828
             * 20180918删除该逻辑，刘松年在下文会改
             */
            //if (request.RenewalType == 3)
            //{
            //    insertUserinfo.IsCamera = true;
            //    insertUserinfo.CameraTime = DateTime.Now;
            //}
            /*
             * 如果老数据，分配状态都按以前的走。
             * 如果摄像头进店，顶级和管理员是未分配，其他为自己试算
             * 非摄像头进店，顶级是未分配，其他为已分配
             * 此处老数据不用考虑，不执行下文的update方法即可
             * addbygpj20180828
             */
            bool distributedRoleType = false;
            if (request.RenewalType == 3)
            {
                distributedRoleType = (roleType == 3 || roleType == 4);//管理员或顶级
                insertUserinfo.IsDistributed = distributedRoleType ? 0 : 2;
            }
            else
            {
                distributedRoleType = roleType == 3;//顶级
                insertUserinfo.IsDistributed = distributedRoleType ? 0 : 2;
            }
            insertUserinfo.IsSingleLicenseno = 0;
            //20181009中心要求修改是否单车牌标识
            if (string.IsNullOrEmpty(insertUserinfo.CarVIN) || string.IsNullOrEmpty(insertUserinfo.EngineNo))
            {
                insertUserinfo.IsSingleLicenseno = 1;
            }

            #region 车店连呼新增userinfo时权限相关特殊操作，bygpj
            //业务call liuzhenlong,qixinbo,lvshaobo
            if (request.ZuoXiId > 0)
            {
                //新增续保报价选择数据来源时用 20181218
                insertUserinfo.zs_zuoxi_id = request.ZuoXiId;
            }
            else
            {
                //20181126去掉配置//20181127新增的需要加上
                //车店连呼特殊配置CLHZuoXiAgentId
                var strzuoxiagent = ConfigurationManager.AppSettings["CLHZuoXiAgentId"];
                if (!string.IsNullOrEmpty(strzuoxiagent))
                {
                    string[] reAgent = strzuoxiagent.Split(',');
                    //取配置里的数组，如果匹配上，则将子集代理id赋值给zuoxiid
                    if (Array.IndexOf(reAgent, topAgentId.ToString()) != -1)
                        insertUserinfo.zs_zuoxi_id = request.Agent;
                }
            }
            #endregion

            var buid = infoRepository.Add(insertUserinfo);

            insertUserinfo.Id = buid;
            return insertUserinfo;
        }

        public static bx_userinfo Update(GetReInfoRequest request, bx_userinfo userinfo, IUserInfoRepository infoRepository, int topAgentId = 0)
        {
            userinfo.IsLastYear = 0;
            userinfo.LastYearSource = -1;
            //userinfo.Source = 0;
            //userinfo.IsSingleSubmit = 0;
            //userinfo.IsPublic = request.IsPublic;
            userinfo.RenewalStatus = -1;
            //userinfo.QuoteStatus = -1;
            userinfo.UpdateTime = DateTime.Now;
            userinfo.Agent = request.Agent.ToString();
            userinfo.CityCode = request.CityCode.ToString();
            //userinfo.IsLastNewCar = request.IsLastYearNewCar;
            userinfo.IsTest = 0;
            userinfo.RenewalCarType = request.RenewalCarType;
            if (!string.IsNullOrWhiteSpace(request.SixDigitsAfterIdCard))
            {
                userinfo.SixDigitsAfterIdCard = request.SixDigitsAfterIdCard;
            }
            if (request.IsLastYearNewCar == 2)
            {
                userinfo.LicenseNo = request.LicenseNo;
                userinfo.MoldName = string.Empty;
                userinfo.RegisterDate = string.Empty;
                userinfo.EngineNo = !string.IsNullOrWhiteSpace(request.EngineNo) ? request.EngineNo.ToUpper() : "";
                if (!string.IsNullOrWhiteSpace(request.CarVin))
                {
                    userinfo.CarVIN = request.CarVin.ToUpper();
                }
                userinfo.NeedEngineNo = 1;
            }

            if (request.IsLastYearNewCar == 1 &
                (!string.IsNullOrWhiteSpace(request.CarVin) && !string.IsNullOrWhiteSpace(request.EngineNo)))
            {
                userinfo.LicenseNo = request.LicenseNo;
                userinfo.MoldName = string.Empty;
                userinfo.RegisterDate = string.Empty;
                if (!string.IsNullOrWhiteSpace(request.EngineNo))
                {
                    userinfo.EngineNo = request.EngineNo.ToUpper();
                }
                if (!string.IsNullOrWhiteSpace(request.CarVin))
                {
                    userinfo.CarVIN = request.CarVin.ToUpper();
                }
                userinfo.NeedEngineNo = 1;
            }
            if (request.IsLastYearNewCar == 1 &&
                (!string.IsNullOrWhiteSpace(request.CarVin) && string.IsNullOrWhiteSpace(request.EngineNo)))
            {
                userinfo.LicenseNo = request.LicenseNo;
                userinfo.MoldName = string.Empty;
                userinfo.RegisterDate = string.Empty;
                userinfo.EngineNo = !string.IsNullOrWhiteSpace(request.EngineNo) ? request.EngineNo.ToUpper() : "";
                if (!string.IsNullOrWhiteSpace(request.CarVin))
                {
                    userinfo.CarVIN = request.CarVin.ToUpper();
                }
                userinfo.NeedEngineNo = 1;
            }
            /*摄像头进店修改进店参数
             *addbygpj20180828
             * 20180918删除该逻辑，刘松年在下文会改
             */
            //if (request.RenewalType == 3)
            //{
            //    userinfo.IsCamera = true;
            //    userinfo.CameraTime = DateTime.Now;
            //}
            ////元数据是摄像头的，改为新的录入方式
            //if (userinfo.RenewalType == 3)
            //{
            //    userinfo.RenewalType = request.RenewalType;
            //}
            ////录入方式为摄像头的，将元数据改为摄像头
            //if (request.RenewalType == 3)
            //{
            //    userinfo.RenewalType = 3;
            //}
            userinfo.IsSingleLicenseno = 0;
            //20181009中心要求修改是否单车牌标识
            if (string.IsNullOrEmpty(userinfo.CarVIN) || string.IsNullOrEmpty(userinfo.EngineNo))
            {
                userinfo.IsSingleLicenseno = 1;
            }
            //20181126去掉配置
            //if (userinfo.zs_zuoxi_id == 0)
            //{
            //    //车店连呼特殊配置CLHZuoXiAgentId
            //    var strzuoxiagent = ConfigurationManager.AppSettings["CLHZuoXiAgentId"];
            //    if (!string.IsNullOrEmpty(strzuoxiagent))
            //    {
            //        string[] reAgent = strzuoxiagent.Split(',');
            //        //取配置里的数组，如果匹配上，则将子集代理id赋值给zuoxiid
            //        if (Array.IndexOf(reAgent, topAgentId.ToString()) != -1)
            //            userinfo.zs_zuoxi_id = request.Agent;
            //    }
            //}
            infoRepository.Update(userinfo);
            return userinfo;
        }

        /// <summary>
        /// 请求报价新增userinfo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="_url"></param>
        /// <param name="repository"></param>
        /// <param name="topAgentId"></param>
        /// <returns></returns>
        public static long QuoteAdd(PostPrecisePriceRequest request, string _url, IUserInfoRepository repository, int topAgentId = 0)
        {
            //如果是直接报价，没有传递车牌号，直接用车架号替换
            if (string.IsNullOrWhiteSpace(request.LicenseNo))
            {
                request.LicenseNo = request.CarVin.ToUpper();
            }
            var insertUserinfo = new bx_userinfo
            {
                agent_id = request.Agent,
                top_agent_id = topAgentId,
                IsLastYear = 0,
                LastYearSource = -1,
                Source = request.IntentionCompany,
                //Mobile = agentModel.Mobile,
                LicenseNo = request.LicenseNo,
                Agent = request.Agent.ToString(),
                //UserId = account.UserId,
                OpenId = request.CustKey,
                CityCode = request.CityCode.ToString(),
                //IsPublic = 0,
                QuoteStatus = -1,
                CreateTime = DateTime.Now,
                IsSingleSubmit = request.IsSingleSubmit,
                // CarType = request.CarType,
                // IsNewCar = request.IsNewCar,
                // CarUsedType = request.CarUsedType,
                CarVIN = request.CarVin.ToUpper(),
                MoldName = request.MoldName,
                RegisterDate = request.RegisterDate,
                EngineNo = request.EngineNo.ToUpper(),
                RenewalType = request.RenewalType,
                // SeatCout = request.SeatCount,
                // TonCount = request.TonCount,
                NeedEngineNo = 0,
                IsDistributed = 0,
                UpdateTime = DateTime.Now,
                LatestQuoteTime = DateTime.Now,//20181030跟路航沟通报价前后都改此值，modifyby gpj
                RenewalStatus = -1,
                OwnerIdCardType = -1,
                IsReView = 0,
                IsTest = 0,
                RenewalCarType = request.RenewalCarType,
                IsChangeRelation = 0//修改关系人状态改为0；17/10/23bygpjadd。//提交订单会判断此字段是否为1
            };
            if (!string.IsNullOrWhiteSpace(request.Mobile))
            {
                insertUserinfo.Mobile = request.Mobile;
            }
            //分配逻辑 如果是顶级，为未分配，否则为已分配（即自己试算数据）//modifyby20181119此处以前删除了，现在放开，因为调用crm接口的摄像头分配逻辑废弃了
            if (request.RenewalType == 2)
            {
                insertUserinfo.IsDistributed = 0; //未分配
            }
            else
            {
                if (request.ChildAgent == topAgentId)
                {
                    insertUserinfo.IsDistributed = 0; //未分配
                }
                else
                {
                    insertUserinfo.IsDistributed = 2; //已分配
                }
            }
            //此新车报价，如果有就更新 ，没有直接报价
            if (!string.IsNullOrWhiteSpace(request.UpdateLicenseNo))
            {
                insertUserinfo.LicenseNo = request.UpdateLicenseNo;
            }
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                insertUserinfo.Email = request.Email;
            }
            //#region 费改地区身份证完善
            if (!string.IsNullOrWhiteSpace(request.CarOwnersName))
            {
                //insertUserinfo.UserName = request.CarOwnersName;
                insertUserinfo.LicenseOwner = request.CarOwnersName;
                //insertUserinfo.InsuredName = request.CarOwnersName;
            }
            if (!string.IsNullOrWhiteSpace(request.IdCard))
            {
                insertUserinfo.IdCard = request.IdCard.ToUpper();
                //insertUserinfo.InsuredIdCard = request.IdCard.ToUpper();
                //insertUserinfo.InsuredIdType = 1;//小龙端 需要此属性
            }
            //#endregion
            #region 车主信息
            if (!string.IsNullOrWhiteSpace(request.IdCard))
            {
                insertUserinfo.IdCard = request.IdCard;
                //if (request.IdCard.IsValidIdCard())
                //{
                //    request.OwnerIdCardType = 1;
                //}
                if (!string.IsNullOrEmpty(_isAddIdCardBack6) && _isAddIdCardBack6.Equals("1") && string.IsNullOrEmpty(insertUserinfo.SixDigitsAfterIdCard) && !insertUserinfo.IdCard.Contains("*") && insertUserinfo.IdCard.Length > 6)
                {
                    //原先证件6位不为空&&证件不包含*&&证件长度大于6位
                    insertUserinfo.SixDigitsAfterIdCard = insertUserinfo.IdCard.Substring(insertUserinfo.IdCard.Length - 6, 6);
                }

            }
            if (request.OwnerIdCardType > 0)
            {
                insertUserinfo.OwnerIdCardType = request.OwnerIdCardType;
            }
            insertUserinfo.OwnerSex = request.OwnerSex;
            insertUserinfo.OwnerBirthday = request.OwnerBirthday;
            insertUserinfo.OwnerIssuer = request.OwnerAuthority;
            insertUserinfo.OwnerNation = request.OwnerNation;
            #endregion
            #region 投保人为空 按照被保人信息保存
            var isPosterValid = false ||
                         !string.IsNullOrWhiteSpace(request.HolderName) &&
                         !string.IsNullOrWhiteSpace(request.HolderIdCard) && request.HolderIdType > 0;
            var isInsuredValid = false || !string.IsNullOrWhiteSpace(request.InsuredName) &&
                                 !string.IsNullOrWhiteSpace(request.InsuredIdCard) && request.InsuredIdType > 0;
            if (!isPosterValid && isInsuredValid)
            {
                request.HolderName = request.InsuredName;
                request.HolderIdCard = request.InsuredIdCard;
                request.HolderIdType = request.InsuredIdType;
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredMobile) && string.IsNullOrWhiteSpace(request.HolderMobile))
            {
                request.HolderMobile = request.InsuredMobile;
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredEmail) && string.IsNullOrWhiteSpace(request.HolderEmail))
            {
                request.HolderEmail = request.InsuredEmail;
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredAddress) &&
                string.IsNullOrWhiteSpace(request.HolderAddress))
            {
                request.HolderAddress = request.InsuredAddress;
            }
            #endregion
            #region 被保险人信息
            if (!string.IsNullOrWhiteSpace(request.InsuredEmail))
            {
                insertUserinfo.InsuredEmail = request.InsuredEmail;
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredName))
            {
                insertUserinfo.InsuredName = request.InsuredName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredIdCard))
            {
                insertUserinfo.InsuredIdCard = request.InsuredIdCard.ToUpper();
                //if (request.InsuredIdCard.IsValidIdCard())
                //{
                //    request.InsuredIdType = 1;
                //}
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredMobile))
            {
                insertUserinfo.InsuredMobile = request.InsuredMobile.Trim();
            }
            if (request.InsuredIdType != 0)
            {
                insertUserinfo.InsuredIdType = request.InsuredIdType;
            }
            insertUserinfo.InsuredAddress = request.InsuredAddress;
            insertUserinfo.InsuredCertiStartdate = request.InsuredCertiStartdate;
            insertUserinfo.InsuredCertiEnddate = request.InsuredCertiEnddate;
            insertUserinfo.InsuredSex = request.InsuredSex;
            insertUserinfo.InsuredBirthday = request.InsuredBirthday;
            insertUserinfo.InsuredIssuer = request.InsuredAuthority;
            insertUserinfo.InsuredNation = request.InsuredNation;

            #endregion
            #region 投保人信息
            if (!string.IsNullOrWhiteSpace(request.HolderEmail))
            {
                insertUserinfo.HolderEmail = request.HolderEmail;
            }
            if (!string.IsNullOrWhiteSpace(request.HolderName))
            {
                insertUserinfo.HolderName = request.HolderName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(request.HolderIdCard))
            {
                insertUserinfo.HolderIdCard = request.HolderIdCard.ToUpper();
                //if (request.HolderIdCard.IsValidIdCard())
                //{
                //    request.HolderIdType = 1;
                //}
            }
            if (!string.IsNullOrWhiteSpace(request.HolderMobile))
            {
                insertUserinfo.HolderMobile = request.HolderMobile.Trim();
            }
            if (request.HolderIdType != 0)
            {
                insertUserinfo.HolderIdType = request.HolderIdType;
            }
            insertUserinfo.HolderAddress = request.HolderAddress;
            insertUserinfo.HolderCertiStartdate = request.HolderCertiStartdate;
            insertUserinfo.HolderCertiEnddate = request.HolderCertiEnddate;
            insertUserinfo.HolderSex = request.HolderSex;
            insertUserinfo.HolderBirthday = request.HolderBirthday;
            insertUserinfo.HolderIssuer = request.HolderAuthority;
            insertUserinfo.HolderNation = request.HolderNation;

            #endregion

            //此处品牌型号改在外层获取
            //if (insertUserinfo.CarVIN.Length > 5)
            //{
            //    var frontCarVin = insertUserinfo.CarVIN.Substring(0, 5);
            //    if (!insertUserinfo.CarVIN.StartsWith("L") && insertUserinfo.MoldName.ToUpper().IndexOf(frontCarVin, System.StringComparison.Ordinal) >= 0)
            //    {
            //        using (var client = new HttpClient())
            //        {
            //            client.BaseAddress = new Uri(_url);
            //            var getUrl = string.Format("api/taipingyang/gettaipycarinfoby?carvin={0}", insertUserinfo.CarVIN);
            //            HttpResponseMessage responseVin = client.GetAsync(getUrl).Result;
            //            var resultVin = responseVin.Content.ReadAsStringAsync().Result;
            //            var carinfo = resultVin.FromJson<WaGetTaiPyCarInfoResponse>();
            //            if (carinfo != null && carinfo.CarInfo != null)
            //            {
            //                insertUserinfo.MoldName = carinfo.CarInfo.moldName;
            //            }
            //        }
            //    }
            //}

            #region 车店连呼新增userinfo时权限相关特殊操作，bygpj
            //业务call liuzhenlong,qixinbo,lvshaobo
            if (request.ZuoXiId > 0)
            {
                //新增续保报价选择数据来源时用 20181218
                insertUserinfo.zs_zuoxi_id = request.ZuoXiId;
            }
            else
            {
                //20181126去掉配置//20181127只新增赋值zuoxiid
                //车店连呼特殊配置CLHZuoXiAgentId
                var strzuoxiagent = ConfigurationManager.AppSettings["CLHZuoXiAgentId"];
                if (!string.IsNullOrEmpty(strzuoxiagent))
                {
                    string[] reAgent = strzuoxiagent.Split(',');
                    //取配置里的数组，如果匹配上，则将子集代理id赋值给zuoxiid
                    if (Array.IndexOf(reAgent, topAgentId.ToString()) != -1)
                        insertUserinfo.zs_zuoxi_id = request.Agent;
                }
            }
            #endregion

            insertUserinfo.IsSingleLicenseno = 0;
            //20181009中心要求修改是否单车牌标识
            if (string.IsNullOrEmpty(insertUserinfo.CarVIN) || string.IsNullOrEmpty(insertUserinfo.EngineNo))
            {
                insertUserinfo.IsSingleLicenseno = 1;
            }
            var id = repository.Add(insertUserinfo);
            return id;
        }

        /// <summary>
        /// 报价更新userinfo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userinfo"></param>
        /// <param name="_url"></param>
        /// <param name="repository"></param>
        /// <returns></returns>
        public static long QuoteUpdate(PostPrecisePriceRequest request, bx_userinfo userinfo, string _url, IUserInfoRepository repository, int topAgentId = 0)
        {
            userinfo.IsSingleSubmit = request.IsSingleSubmit;//是否对单个公司进行核保  1：是 0： 否
            userinfo.Source = request.IntentionCompany;
            // userinfo.CarType = request.CarType;
            // userinfo.IsNewCar = request.IsNewCar;
            // userinfo.CarUsedType = request.CarUsedType;
            userinfo.CityCode = request.CityCode.ToString();
            if (!string.IsNullOrWhiteSpace(request.CarVin))
            {
                userinfo.CarVIN = request.CarVin.ToUpper();
            }
            if (!string.IsNullOrWhiteSpace(request.MoldName))
            {
                userinfo.MoldName = request.MoldName;
            }
            if (!string.IsNullOrWhiteSpace(request.RegisterDate))
            {
                userinfo.RegisterDate = request.RegisterDate;
            }
            if (!string.IsNullOrWhiteSpace(request.EngineNo))
            {
                userinfo.EngineNo = request.EngineNo.ToUpper();
            }
            userinfo.UpdateTime = DateTime.Now;
            userinfo.LatestQuoteTime = DateTime.Now;//20181030跟路航沟通报价前后都改此值，modifyby gpj
            userinfo.NeedEngineNo = 0;
            userinfo.RenewalCarType = request.RenewalCarType;
            if (!string.IsNullOrWhiteSpace(request.Mobile))
            {
                userinfo.Mobile = request.Mobile;
            }
            //if (isNeedUpdateLicensen)
            //{
            //    userinfo.LicenseNo = request.UpdateLicenseNo;
            //}
            ////如果没有车牌号，则默认按照车架号走
            //if (request.IsNewCar == 1)
            //{
            //    if (string.IsNullOrWhiteSpace(request.LicenseNo))
            //    {
            //        userinfo.LicenseNo = request.CarVin.ToUpper();
            //    }
            //}
            //此新车报价，如果有就更新 ，没有直接报价
            if (!string.IsNullOrWhiteSpace(request.UpdateLicenseNo))
            {
                userinfo.LicenseNo = request.UpdateLicenseNo;
            }
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                userinfo.Email = request.Email;
            }
            //  userinfo.SeatCout = request.SeatCount;
            //  userinfo.TonCount = request.TonCount;
            //#region 费改地区身份证完善
            if (!string.IsNullOrWhiteSpace(request.CarOwnersName))
            {
                //userinfo.UserName = request.CarOwnersName;
                userinfo.LicenseOwner = request.CarOwnersName;
                //userinfo.InsuredName = request.CarOwnersName;
            }
            if (!string.IsNullOrWhiteSpace(request.IdCard))
            {
                userinfo.IdCard = request.IdCard.ToUpper();
                //userinfo.InsuredIdCard = request.IdCard.ToUpper();
                //userinfo.InsuredIdType = 1;
            }
            //#endregion
            #region 车主信息
            if (!string.IsNullOrWhiteSpace(request.IdCard))
            {
                userinfo.IdCard = request.IdCard;
                //if (request.IdCard.IsValidIdCard())
                //{
                //    request.OwnerIdCardType = 1;
                //}
                if (!string.IsNullOrEmpty(_isAddIdCardBack6) && _isAddIdCardBack6.Equals("1") && string.IsNullOrEmpty(userinfo.SixDigitsAfterIdCard) && !userinfo.IdCard.Contains("*") && userinfo.IdCard.Length > 6)
                {
                    //原先证件6位不为空&&证件不包含*&&证件长度大于6位
                    userinfo.SixDigitsAfterIdCard = userinfo.IdCard.Substring(userinfo.IdCard.Length - 6, 6);
                }
            }
            if (request.OwnerIdCardType > 0)
            {
                userinfo.OwnerIdCardType = request.OwnerIdCardType;
            }
            userinfo.OwnerSex = request.OwnerSex;
            userinfo.OwnerBirthday = request.OwnerBirthday;
            userinfo.OwnerIssuer = request.OwnerAuthority;
            userinfo.OwnerNation = request.OwnerNation;
            #endregion
            #region 投保人为空 按照被保人信息保存
            var isPosterValid = false ||
                         !string.IsNullOrWhiteSpace(request.HolderName) &&
                         !string.IsNullOrWhiteSpace(request.HolderIdCard) && request.HolderIdType > 0;
            var isInsuredValid = false || !string.IsNullOrWhiteSpace(request.InsuredName) &&
                                 !string.IsNullOrWhiteSpace(request.InsuredIdCard) && request.InsuredIdType > 0;
            if (!isPosterValid && isInsuredValid)
            {
                request.HolderName = request.InsuredName;
                request.HolderIdCard = request.InsuredIdCard;
                request.HolderIdType = request.InsuredIdType;
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredMobile) && string.IsNullOrWhiteSpace(request.HolderMobile))
            {
                request.HolderMobile = request.InsuredMobile;
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredEmail) && string.IsNullOrWhiteSpace(request.HolderEmail))
            {
                request.HolderEmail = request.InsuredEmail;
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredAddress) &&
                string.IsNullOrWhiteSpace(request.HolderAddress))
            {
                request.HolderAddress = request.InsuredAddress;
            }
            #endregion
            #region 被保险人信息
            if (!string.IsNullOrWhiteSpace(request.InsuredName))
            {
                userinfo.InsuredName = request.InsuredName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredIdCard))
            {
                userinfo.InsuredIdCard = request.InsuredIdCard.ToUpper();
                //if (request.InsuredIdCard.IsValidIdCard())
                //{
                //    request.InsuredIdType = 1;
                //}
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredEmail))
            {
                userinfo.InsuredEmail = request.InsuredEmail;
            }
            if (!string.IsNullOrWhiteSpace(request.InsuredMobile))
            {
                userinfo.InsuredMobile = request.InsuredMobile.Trim();
            }
            if (request.InsuredIdType != 0)
            {
                userinfo.InsuredIdType = request.InsuredIdType;
            }
            userinfo.InsuredAddress = request.InsuredAddress;
            userinfo.InsuredCertiStartdate = request.InsuredCertiStartdate;
            userinfo.InsuredCertiEnddate = request.InsuredCertiEnddate;
            userinfo.InsuredSex = request.InsuredSex;
            userinfo.InsuredBirthday = request.InsuredBirthday;
            userinfo.InsuredIssuer = request.InsuredAuthority;
            userinfo.InsuredNation = request.InsuredNation;

            #endregion
            #region 投保人信息
            if (!string.IsNullOrWhiteSpace(request.HolderEmail))
            {
                userinfo.HolderEmail = request.HolderEmail;
            }
            if (!string.IsNullOrWhiteSpace(request.HolderName))
            {
                userinfo.HolderName = request.HolderName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(request.HolderIdCard))
            {
                userinfo.HolderIdCard = request.HolderIdCard.ToUpper();
                //if (request.HolderIdCard.IsValidIdCard())
                //{
                //    request.HolderIdType = 1;
                //}
            }
            if (!string.IsNullOrWhiteSpace(request.HolderMobile))
            {
                userinfo.HolderMobile = request.HolderMobile.Trim();
            }
            if (request.HolderIdType != 0)
            {
                userinfo.HolderIdType = request.HolderIdType;
            }
            userinfo.HolderAddress = request.HolderAddress;
            userinfo.HolderCertiStartdate = request.HolderCertiStartdate;
            userinfo.HolderCertiEnddate = request.HolderCertiEnddate;
            userinfo.HolderSex = request.HolderSex;
            userinfo.HolderBirthday = request.HolderBirthday;
            userinfo.HolderIssuer = request.HolderAuthority;
            userinfo.HolderNation = request.HolderNation;

            #endregion

            #region 老的获取品牌型号方法
            //此处方法获取品牌型号放在外层获取
            //if (userinfo.CarVIN.Length > 5)
            //{
            //    var frontCarVin = userinfo.CarVIN.Substring(0, 5);
            //    if (!userinfo.CarVIN.StartsWith("L") && userinfo.MoldName.ToUpper().IndexOf(frontCarVin, System.StringComparison.Ordinal) >= 0)
            //    {
            //        using (HttpClient client = new HttpClient())
            //        {
            //            client.BaseAddress = new Uri(_url);
            //            var getUrl = string.Format("api/taipingyang/gettaipycarinfoby?carvin={0}", userinfo.CarVIN);
            //            HttpResponseMessage responseVin = client.GetAsync(getUrl).Result;
            //            var resultVin = responseVin.Content.ReadAsStringAsync().Result;
            //            var carinfo = resultVin.FromJson<WaGetTaiPyCarInfoResponse>();
            //            if (carinfo != null && carinfo.CarInfo != null)
            //            {
            //                userinfo.MoldName = carinfo.CarInfo.moldName;
            //            }
            //        }
            //    }
            //}
            #endregion
            //20181126去掉配置
            //if (userinfo.zs_zuoxi_id == 0)
            //{
            //    //车店连呼特殊配置CLHZuoXiAgentId
            //    var strzuoxiagent = ConfigurationManager.AppSettings["CLHZuoXiAgentId"];
            //    if (!string.IsNullOrEmpty(strzuoxiagent))
            //    {
            //        string[] reAgent = strzuoxiagent.Split(',');
            //        //取配置里的数组，如果匹配上，则将子集代理id赋值给zuoxiid
            //        if (Array.IndexOf(reAgent, topAgentId.ToString()) != -1)
            //            userinfo.zs_zuoxi_id = request.Agent;
            //    }
            //}
            userinfo.IsChangeRelation = 0;//修改关系人状态改为0；17/10/23bygpjadd。//提交订单会判断此字段是否为1
            repository.Update(userinfo);
            return userinfo.Id;
        }
    }
}
