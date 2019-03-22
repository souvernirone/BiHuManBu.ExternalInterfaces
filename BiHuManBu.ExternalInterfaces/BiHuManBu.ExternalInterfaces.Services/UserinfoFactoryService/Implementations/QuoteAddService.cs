using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.GetCarInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.UserinfoFactoryService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.UserinfoFactoryService.Implementations
{
    public class QuoteAddService: IQuoteAddService
    {
        private readonly IGetMoldNameFromCenter _getMoldNameFromCenter;

        public QuoteAddService(IGetMoldNameFromCenter getMoldNameFromCenter) {
            _getMoldNameFromCenter = getMoldNameFromCenter;
        }

        public long QuoteAdd(PostPrecisePriceRequest request, string _url, IUserInfoRepository repository, int topAgentId = 0)
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
            //分配逻辑 
            //if (request.RenewalType == 2)
            //{
            //    insertUserinfo.IsDistributed = 0; //未分配
            //}
            //else
            //{
            //    if (request.ChildAgent == request.Agent)
            //    {
            //        insertUserinfo.IsDistributed = 0; //未分配
            //    }
            //    else
            //    {
            //        insertUserinfo.IsDistributed = 2; //已 分配
            //    }
            //}
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

            var id = repository.Add(insertUserinfo);
            return id;
        }
    }
}
