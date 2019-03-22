﻿using System;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using ServiceStack.Logging;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class UserInfoMapper
    {
        private static ILog log = LogManager.GetLogger("ERROR");
        /// <summary>
        /// 续保信息的userinfo部分
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="renewal"></param>
        /// <param name="carinfo"></param>
        /// <param name="lastinfo"></param>
        /// <param name="timeFormat"></param>
        /// <returns></returns>
        public static UserInfoViewModel ConvertToViewModel(this bx_userinfo userinfo, bx_car_renewal renewal, bx_carinfo carinfo, bx_lastinfo lastinfo, int timeFormat = 0)
        {

            var model = new UserInfoViewModel();
            model.Buid = userinfo.Id.ToString();
            try
            {
                if (renewal != null)
                {
                    model.BizNo = string.IsNullOrWhiteSpace(renewal.BizNO) ? string.Empty : renewal.BizNO;
                    model.ForceNo = string.IsNullOrWhiteSpace(renewal.ForceNO) ? string.Empty : renewal.ForceNO;
                    model.InsuredIdCard = string.IsNullOrWhiteSpace(renewal.InsuredIdCard)
                        ? string.Empty
                        : renewal.InsuredIdCard;
                    model.InsuredIdType = renewal.InsuredIdType.HasValue ? renewal.InsuredIdType.Value : 0;
                    //model.InsuredMobile = string.IsNullOrWhiteSpace(renewal.InsuredMobile)
                    //    ? string.Empty
                    //    : renewal.InsuredMobile.IndexOf("*", System.StringComparison.Ordinal) >= 0 ? string.Empty : renewal.InsuredMobile;
                    //model.HolderMobile = string.IsNullOrWhiteSpace(renewal.HolderMobile)
                    //    ? string.Empty
                    //    : renewal.HolderMobile.IndexOf("*", System.StringComparison.Ordinal) >= 0 ? string.Empty : renewal.HolderMobile;
                    model.InsuredMobile = string.Empty;
                    model.HolderMobile = string.Empty;

                    model.HolderIdCard = string.IsNullOrWhiteSpace(renewal.HolderIdCard)
                        ? string.Empty
                        : renewal.HolderIdCard;
                    model.HolderIdType = renewal.HolderIdType.HasValue ? renewal.HolderIdType.Value : 0;
                    model.HolderName = string.IsNullOrWhiteSpace(renewal.HolderName)
                        ? string.Empty
                        : renewal.HolderName;

                    //model.LicenseOwner = string.IsNullOrWhiteSpace(renewal) ? string.Empty : renewal.HolderName;
                    model.PostedName = string.IsNullOrWhiteSpace(renewal.HolderName) ? string.Empty : renewal.HolderName;
                    model.InsuredName = string.IsNullOrWhiteSpace(renewal.InsuredName) ? string.Empty : renewal.InsuredName;
                    model.RateFactor1 = renewal.RateFactor1.HasValue ? renewal.RateFactor1.Value : 0;
                    model.RateFactor2 = renewal.RateFactor2.HasValue ? renewal.RateFactor2.Value : 0;
                    model.RateFactor3 = renewal.RateFactor3.HasValue ? renewal.RateFactor3.Value : 0;
                    model.RateFactor4 = renewal.RateFactor4.HasValue ? renewal.RateFactor4.Value : 0;


                    model.ForceExpireDate = renewal.LastForceEndDate.HasValue ? (timeFormat == 1 ? renewal.LastForceEndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : renewal.LastForceEndDate.Value.ToString("yyyy-MM-dd")) : string.Empty;
                    model.BusinessExpireDate = renewal.LastBizEndDate.HasValue ? (timeFormat == 1 ? renewal.LastBizEndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : renewal.LastBizEndDate.Value.ToString("yyyy-MM-dd")) : string.Empty;
                    model.NextForceStartDate = (renewal.NextForceStartDate.HasValue
                        ? (timeFormat == 1 ? renewal.NextForceStartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : renewal.NextForceStartDate.Value.ToString("yyyy-MM-dd"))
                        : string.Empty);
                    model.NextBusinessStartDate = (renewal.NextBizStartDate.HasValue
                        ? (timeFormat == 1 ? renewal.NextBizStartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : renewal.NextBizStartDate.Value.ToString("yyyy-MM-dd"))
                        : string.Empty);

                    if (!string.IsNullOrWhiteSpace(model.ForceExpireDate))
                    {
                        var fd = DateTime.Parse(model.ForceExpireDate);
                        if (fd.Date == DateTime.MinValue.Date)
                        {
                            model.ForceExpireDate = "";
                        }
                        else
                        {
                            model.ForceExpireDate = timeFormat == 1 ? DateTime.Parse(model.ForceExpireDate).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Parse(model.ForceExpireDate).ToString("yyyy-MM-dd");
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(model.BusinessExpireDate))
                    {
                        var bd = DateTime.Parse(model.BusinessExpireDate);
                        if (bd.Date == DateTime.MinValue.Date)
                        {
                            model.BusinessExpireDate = "";
                        }
                        else
                        {
                            model.BusinessExpireDate = timeFormat == 1 ? DateTime.Parse(model.BusinessExpireDate).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Parse(model.BusinessExpireDate).ToString("yyyy-MM-dd");
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(model.NextForceStartDate))
                    {
                        var nf = DateTime.Parse(model.NextForceStartDate);
                        if (nf.Date == DateTime.MinValue.Date)
                        {
                            model.NextForceStartDate = "";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(model.NextBusinessStartDate))
                    {
                        var nb = DateTime.Parse(model.NextBusinessStartDate);
                        if (nb.Date == DateTime.MinValue.Date)
                        {
                            model.NextBusinessStartDate = "";
                        }
                    }
                    model.Organization = renewal.Organization ?? string.Empty;
                    //新增的投保人生日和性别
                    model.HolderBirthday = renewal.HolderBirthday ?? string.Empty;
                    if (!string.IsNullOrEmpty(model.HolderBirthday) && Convert.ToDateTime(model.HolderBirthday).Year == DateTime.MinValue.Year)
                    {
                        model.HolderBirthday = string.Empty;
                    }
                    model.HolderSex = (renewal.HolderSex ?? 0).ToString();
                    //新增的被保险人生日和性别
                    model.InsuredBirthday = renewal.InsuredBirthday ?? string.Empty;
                    if (!string.IsNullOrEmpty(model.InsuredBirthday) && Convert.ToDateTime(model.InsuredBirthday).Year == DateTime.MinValue.Year)
                    {
                        model.InsuredBirthday = string.Empty;
                    }
                    model.InsuredSex = (renewal.InsuredSex ?? 0).ToString();
                }
                else
                {
                    model.ForceExpireDate = string.Empty;
                    model.BusinessExpireDate = string.Empty;
                    model.NextForceStartDate = string.Empty;
                    model.NextBusinessStartDate = string.Empty;


                    model.InsuredIdCard = string.Empty;
                    model.InsuredIdType = 0;
                    model.InsuredMobile = string.Empty;
                    model.HolderMobile = string.Empty;
                    model.HolderIdCard = string.Empty;
                    model.HolderIdType = 0;
                    model.HolderName = string.Empty;
                    model.RateFactor1 = 0;
                    model.RateFactor2 = 0;
                    model.RateFactor3 = 0;
                    model.RateFactor4 = 0;
                    model.BizNo = string.Empty;
                    model.ForceNo = string.Empty;
                    model.PostedName = string.Empty;
                    model.InsuredName = string.Empty;
                    model.Organization = string.Empty;

                    model.HolderBirthday = string.Empty;
                    model.HolderSex = string.Empty;
                    model.InsuredBirthday = string.Empty;
                    model.InsuredSex = string.Empty;
                }
                if (carinfo != null)
                {
                    model.FuelType = carinfo.fuel_type.HasValue ? carinfo.fuel_type.Value : 0;
                    model.ProofType = carinfo.proof_type.HasValue ? carinfo.proof_type.Value : 0;
                    model.LicenseColor = carinfo.license_color.HasValue ? carinfo.license_color.Value : 0;
                    model.ClauseType = carinfo.clause_type.HasValue ? carinfo.clause_type.Value : 0;
                    model.RunRegion = carinfo.run_region.HasValue ? carinfo.run_region.Value : 0;

                    model.CarUsedType = carinfo.car_used_type.HasValue ? carinfo.car_used_type.Value : 0;
                    model.CredentislasNum = string.IsNullOrWhiteSpace(carinfo.owner_idno) ? string.Empty : carinfo.owner_idno;
                    model.IdType = carinfo.owner_idno_type.HasValue ? carinfo.owner_idno_type.Value : 0;
                    model.PurchasePrice = (double)(carinfo.purchase_price.HasValue ? carinfo.purchase_price.Value : 0);
                    model.SeatCount = carinfo.seat_count.HasValue ? carinfo.seat_count.Value : 0;
                    model.ExhaustScale = carinfo.exhaust_scale.HasValue ? carinfo.exhaust_scale.ToString() : "0";
                    model.LicenseOwner = string.IsNullOrWhiteSpace(carinfo.license_owner) ? string.Empty : carinfo.license_owner;
                    model.CarType = (carinfo.car_type ?? 0).ToString();
                    model.CarVin = carinfo.vin_no ?? string.Empty;
                    model.EngineNo = carinfo.engine_no ?? string.Empty;
                    model.RegisterDate = carinfo.register_date.HasValue ? carinfo.register_date.Value.ToString("yyyy-MM-dd") : string.Empty;
                    model.ModleName = carinfo.mold_name ?? string.Empty;
                    //新增的车主生日和性别
                    model.OwnerBirthday = carinfo.OwnerBirthday ?? string.Empty;
                    if (!string.IsNullOrEmpty(model.OwnerBirthday) && Convert.ToDateTime(model.OwnerBirthday).Year == DateTime.MinValue.Year)
                    {
                        model.OwnerBirthday = string.Empty;
                    }
                    model.OwnerSex = (carinfo.OwnerSex ?? 0).ToString();
                }
                else
                {
                    model.FuelType = 0;
                    model.ProofType = 0;
                    model.LicenseColor = 0;
                    model.ClauseType = 0;
                    model.RunRegion = 0;

                    model.CarUsedType = 0;
                    model.CredentislasNum = string.Empty;
                    model.IdType = 0;
                    model.ExhaustScale = string.Empty;
                    model.LicenseOwner = string.Empty;
                    model.CarType = string.Empty;
                    model.CarVin = string.IsNullOrWhiteSpace(userinfo.CarVIN) ? string.Empty : userinfo.CarVIN;
                    model.EngineNo = string.IsNullOrWhiteSpace(userinfo.EngineNo) ? string.Empty : userinfo.EngineNo;
                    model.RegisterDate = string.IsNullOrWhiteSpace(userinfo.RegisterDate) ? string.Empty : userinfo.RegisterDate;
                    model.ModleName = string.IsNullOrWhiteSpace(userinfo.MoldName) ? string.Empty : userinfo.MoldName;

                    model.OwnerSex = string.Empty;
                    model.OwnerBirthday = string.Empty;
                }
                //model.CarVin = string.IsNullOrWhiteSpace(userinfo.CarVIN) ? string.Empty : userinfo.CarVIN;
                model.CityCode = !string.IsNullOrWhiteSpace(userinfo.CityCode) ? Convert.ToInt32(userinfo.CityCode) : 1;
                //model.EngineNo = string.IsNullOrWhiteSpace(userinfo.EngineNo) ? string.Empty : userinfo.EngineNo;
                model.LicenseNo = string.IsNullOrWhiteSpace(userinfo.LicenseNo) ? string.Empty : userinfo.LicenseNo;
                //model.LicenseOwner = string.IsNullOrWhiteSpace(userinfo.LicenseOwner) ? string.Empty : userinfo.LicenseOwner;
                //model.ModleName = string.IsNullOrWhiteSpace(userinfo.MoldName) ? string.Empty : userinfo.MoldName;
                //model.RegisterDate = string.IsNullOrWhiteSpace(userinfo.RegisterDate) ? string.Empty : userinfo.RegisterDate;
                if (!string.IsNullOrWhiteSpace(model.RegisterDate)) {
                    if (model.RegisterDate.Equals(DateTime.MinValue.ToString("yyyy-MM-dd"))) {
                        model.RegisterDate = "";
                    }
                }
                //model.PostedName = model.HolderName;
                //model.InsuredName = string.IsNullOrWhiteSpace(userinfo.InsuredName) ? string.Empty : userinfo.InsuredName;
                model.HolderName = null;
                model.RenewalCarType = (userinfo.RenewalCarType ?? 0).ToString();


            }
            catch (Exception ex)
            {
                log.Info("模型转换报错:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }



            return model;
        }
        /// <summary>
        /// 报价信息的 userinfo部分
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="lastinfo"></param>
        /// <param name="quoteresult"></param>
        /// <param name="resultcarinfo"></param>
        /// <param name="timeFormat"></param>
        /// <returns></returns>
        public static GetPrecisePriceOfUserInfoViewModel ConvertToPreciseViewModel(this bx_userinfo userinfo,
            bx_lastinfo lastinfo, bx_quoteresult quoteresult, bx_quoteresult_carinfo resultcarinfo, int timeFormat = 0)
        {
            var model = new GetPrecisePriceOfUserInfoViewModel();
            if (lastinfo != null)
            {
                model.ForceExpireDate = lastinfo.last_end_date ?? string.Empty;
                model.BusinessExpireDate = lastinfo.last_business_end_date ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(model.ForceExpireDate))
                {
                    var fd = DateTime.Parse(model.ForceExpireDate);
                    if (fd.Date == DateTime.MinValue.Date)
                    {
                        model.ForceExpireDate = "";
                    }
                    else
                    {
                        model.ForceExpireDate = timeFormat == 1 ? DateTime.Parse(model.ForceExpireDate).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Parse(model.ForceExpireDate).ToString("yyyy-MM-dd");
                    }

                }
                if (!string.IsNullOrWhiteSpace(model.BusinessExpireDate))
                {
                    var bd = DateTime.Parse(model.BusinessExpireDate);
                    if (bd.Date == DateTime.MinValue.Date)
                    {
                        model.BusinessExpireDate = "";
                    }
                    else
                    {
                        model.BusinessExpireDate = timeFormat == 1 ? DateTime.Parse(model.BusinessExpireDate).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Parse(model.BusinessExpireDate).ToString("yyyy-MM-dd");
                    }

                }
            }
            else
            {
                model.ForceExpireDate = string.Empty;
                model.BusinessExpireDate = string.Empty;
            }


            if (quoteresult != null)
            {
                model.InsuredName = string.IsNullOrWhiteSpace(quoteresult.InsuredName) ? string.Empty : quoteresult.InsuredName;
                model.InsuredIdCard = string.IsNullOrWhiteSpace(quoteresult.InsuredIdCard) ? string.Empty : quoteresult.InsuredIdCard;
                model.InsuredIdType = quoteresult.InsuredIdType.HasValue ? quoteresult.InsuredIdType.Value : 0;
                model.InsuredMobile = string.IsNullOrWhiteSpace(quoteresult.InsuredMobile) ? string.Empty : quoteresult.InsuredMobile;

                model.HolderName = string.IsNullOrWhiteSpace(quoteresult.HolderName) ? string.Empty : quoteresult.HolderName;
                model.HolderIdCard = string.IsNullOrWhiteSpace(quoteresult.HolderIdCard) ? string.Empty : quoteresult.HolderIdCard;
                model.HolderIdType = quoteresult.HolderIdType.HasValue ? quoteresult.HolderIdType.Value : 0;
                model.HolderMobile = string.IsNullOrWhiteSpace(quoteresult.HolderMobile) ? string.Empty : quoteresult.HolderMobile; ;



                model.BusinessStartDate = quoteresult.BizStartDate.HasValue
                    ? (timeFormat == 1 ? quoteresult.BizStartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : quoteresult.BizStartDate.Value.ToString("yyyy-MM-dd"))
                    : string.Empty;
                model.ForceStartDate = quoteresult.ForceStartDate.HasValue
                    ? (timeFormat == 1 ? quoteresult.ForceStartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : quoteresult.ForceStartDate.Value.ToString("yyyy-MM-dd"))
                    : string.Empty;
                if (!string.IsNullOrWhiteSpace(model.BusinessStartDate))
                {
                    var st = DateTime.Parse(model.BusinessStartDate);
                    if (st.Date == DateTime.MinValue.Date)
                    {
                        model.BusinessStartDate = "";
                    }
                    else
                    {
                        model.BusinessStartDate = timeFormat == 1 ? DateTime.Parse(model.BusinessStartDate).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Parse(model.BusinessStartDate).ToString("yyyy-MM-dd");
                        model.BusinessEndDate = timeFormat == 1 ? DateTime.Parse(model.BusinessStartDate).AddYears(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Parse(model.BusinessStartDate).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                    }
                }
                if (!string.IsNullOrWhiteSpace(model.ForceStartDate))
                {
                    var st = DateTime.Parse(model.ForceStartDate);
                    if (st.Date == DateTime.MinValue.Date)
                    {
                        model.ForceStartDate = "";
                    }
                    else
                    {
                        model.ForceStartDate = timeFormat == 1 ? DateTime.Parse(model.ForceStartDate).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Parse(model.ForceStartDate).ToString("yyyy-MM-dd");
                        model.ForceEndDate = timeFormat == 1 ? DateTime.Parse(model.ForceStartDate).AddYears(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Parse(model.ForceStartDate).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                    }
                }

            }
            else
            {
                model.InsuredName = string.Empty;
                model.InsuredIdCard = string.Empty;
                model.InsuredIdType = 0;
                model.InsuredMobile = string.Empty;

                model.HolderName = string.Empty;
                model.HolderIdCard = string.Empty;
                model.HolderIdType = 0;
                model.HolderMobile = string.Empty;
                model.BusinessStartDate = string.Empty;
                model.ForceStartDate = string.Empty;
            }
            if (resultcarinfo != null)
            {
                model.AutoMoldCode = !string.IsNullOrWhiteSpace(resultcarinfo.auto_model_code)
                    ? resultcarinfo.auto_model_code
                    : string.Empty;
                model.VehicleInfo = VehicleInfoMapper.VehicleInfoMethod(resultcarinfo);
            }
            else
            {
                model.AutoMoldCode = string.Empty;
                model.VehicleInfo = string.Empty;
            }
            model.LicenseNo = userinfo.LicenseNo;
            model.Mobile = string.IsNullOrWhiteSpace(userinfo.Mobile) ? string.Empty : userinfo.Mobile;
            model.Email = string.IsNullOrWhiteSpace(userinfo.Email) ? string.Empty : userinfo.Email;

            //天猫--宝马合作 c端 
            if (userinfo.Agent == "67396")
            {
                model.Mobile = string.Empty;
                model.InsuredMobile = string.Empty;
                model.HolderMobile = string.Empty;
            }

            if (userinfo != null)
            {
                model.CarOwnerPerson = new RelationPerson
                {
                    Address = userinfo.OwnerCertiAddress ?? string.Empty,
                    Email = userinfo.Email ?? string.Empty,
                    IdCard = userinfo.IdCard ?? string.Empty,
                    IdType = userinfo.OwnerIdCardType,
                    Mobile = userinfo.Mobile ?? string.Empty,
                    Name = userinfo.LicenseOwner ?? string.Empty,
                    Sex = userinfo.OwnerSex ?? 1,
                    Authority = userinfo.OwnerIssuer ?? string.Empty,
                    Birthday = userinfo.OwnerBirthday ?? string.Empty,
                    CertiEndDate = userinfo.OwnerCertiEnddate ?? string.Empty,
                    CertiStartDate = userinfo.OwnerCertiStartdate ?? string.Empty,
                    Nation = userinfo.OwnerNation ?? string.Empty
                };
                model.HolderPerson = new RelationPerson()
                {
                    Address = userinfo.HolderAddress ?? string.Empty,
                    Email = string.IsNullOrWhiteSpace(userinfo.HolderEmail) ? model.CarOwnerPerson.Email : userinfo.HolderEmail,
                    IdCard = userinfo.HolderIdCard ?? string.Empty,
                    IdType = userinfo.HolderIdType ?? 0,
                    Mobile = userinfo.HolderMobile ?? string.Empty,
                    Name = userinfo.HolderName ?? string.Empty,
                    Sex = userinfo.HolderSex ?? 1,
                    Authority = userinfo.HolderIssuer ?? string.Empty,
                    Birthday = userinfo.HolderBirthday ?? string.Empty,
                    CertiEndDate = userinfo.HolderCertiEnddate ?? string.Empty,
                    CertiStartDate = userinfo.HolderCertiStartdate ?? string.Empty,
                    Nation = userinfo.HolderNation ?? string.Empty
                };
                model.InsuredPerson = new RelationPerson
                {
                    Address = userinfo.InsuredAddress ?? string.Empty,
                    Email = string.IsNullOrWhiteSpace(userinfo.InsuredEmail) ? model.CarOwnerPerson.Email : userinfo.InsuredEmail,
                    IdCard = userinfo.InsuredIdCard ?? string.Empty,
                    IdType = userinfo.InsuredIdType ?? 0,
                    Mobile = userinfo.InsuredMobile ?? string.Empty,
                    Name = userinfo.InsuredName ?? string.Empty,
                    Sex = userinfo.InsuredSex ?? 1,
                    Authority = userinfo.InsuredIssuer ?? string.Empty,
                    Birthday = userinfo.InsuredBirthday ?? string.Empty,
                    CertiEndDate = userinfo.InsuredCertiEnddate ?? string.Empty,
                    CertiStartDate = userinfo.InsuredCertiStartdate ?? string.Empty,
                    Nation = userinfo.InsuredNation ?? string.Empty
                };
                model.SixDigitsAfterIdCard = userinfo.SixDigitsAfterIdCard ?? "";
            }
            return model;
        }
    }



}