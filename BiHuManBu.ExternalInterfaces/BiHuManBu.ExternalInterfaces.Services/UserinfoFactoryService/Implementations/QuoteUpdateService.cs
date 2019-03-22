using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.UserinfoFactoryService.Interfaces;
using System;

namespace BiHuManBu.ExternalInterfaces.Services.UserinfoFactoryService.Implementations
{
    public class QuoteUpdateService: IQuoteUpdateService
    {
        public QuoteUpdateService() {
        }
        public long QuoteUpdate(PostPrecisePriceRequest request, bx_userinfo userinfo, string _url, IUserInfoRepository repository)
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
            userinfo.IsChangeRelation = 0;//修改关系人状态改为0；17/10/23bygpjadd。//提交订单会判断此字段是否为1
            repository.Update(userinfo);
            return userinfo.Id;
        }
    }
}
