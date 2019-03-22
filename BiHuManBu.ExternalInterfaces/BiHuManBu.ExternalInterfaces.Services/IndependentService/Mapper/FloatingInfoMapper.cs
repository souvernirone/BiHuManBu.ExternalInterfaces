using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel.FloatingInfoModel;

namespace BiHuManBu.ExternalInterfaces.Services.IndependentService.Mapper
{
    public static class FloatingInfoMapper
    {
        public static GetFloatingInfoViewModel ConverToViewModel(this JSFloatingNotificationPrintListResponseMain model)
        {
            GetFloatingInfoViewModel viewModel = new GetFloatingInfoViewModel();
            if (model == null)
            {
                return new GetFloatingInfoViewModel()
                {
                    CarInfo = new CarInfo(),
                    BizFloatingInfo = new BizFloatingInfo(),
                    ForceFloatingInfo = new ForceFloatingInfo()
                };
            }
            CarInfo carInfo = new CarInfo()
            {
                CModelNmeJY = "",
                CarType = "",
                CPlateNo = "",
                CPlateTyp = "",
                CEngNo = "",
                CFrmNo = "",
                CRegOwner = ""
            };
            VhlVOBase baseCar = null;
            #region 交强险模型
            var forceModel = model.JQFloatingNotificationList;
            if (forceModel != null)
            {
                //初始化
                var forcebase = forceModel.JQBaseVO;
                var forceclaim = forceModel.JQClaimList;
                var forcerate = forceModel.JQprmCoef;
                var forcecvrgvo = forceModel.JQCvrgList != null ? forceModel.JQCvrgList.FirstOrDefault() : new CvrgVO();
                var tax = forceModel.VsTax;
                if (forceModel.JQVhlVO != null)
                    baseCar = forceModel.JQVhlVO;
                //交换信息
                ForceBaseInfo forceBaseInfo = new ForceBaseInfo()
                {
                    CPlyNo = "",
                    CAppName = "",
                    TSrcInsrncBgnTm = "",
                    TSrcInsrncEndTm = "",
                    TLastStartDate = "",
                    TLaseEndDate = "",
                    NPrm = "",
                    DNPrm = "",
                    FloatRatio = "",
                    NBefPrm = ""
                };
                if (forcebase != null)
                {
                    forceBaseInfo.CPlyNo = forcebase.CPlyNo ?? "";
                    forceBaseInfo.CAppName = forcebase.CAppName ?? "";
                    forceBaseInfo.TSrcInsrncBgnTm = forcebase.TSrcInsrncBgnTm ?? "";
                    forceBaseInfo.TSrcInsrncEndTm = forcebase.TSrcInsrncEndTm ?? "";
                    forceBaseInfo.TLastStartDate = forcebase.TLastStartDate ?? "";
                    forceBaseInfo.TLaseEndDate = forcebase.TLaseEndDate ?? "";
                    forceBaseInfo.NPrm = forcebase.NPrm ?? "";
                    forceBaseInfo.DNPrm = forcebase.DNPrm ?? "";
                }
                if (forcerate != null)
                {
                    forceBaseInfo.FloatRatio = forcerate.FloatRatio ?? "";
                }
                if (forcecvrgvo != null)
                {
                    forceBaseInfo.NBefPrm = forcecvrgvo.nBefPrm ?? "";
                }
                TaxInfo taxInfo = new TaxInfo()
                {
                    TTaxEffBgnTm = "",
                    TTaxEffEndTm = "",
                    NTaxableAmt = "",
                    NOverdueAmt = "",
                    TaxSumAmount = "",
                    TaxSumAmountUp = ""
                };
                if (tax != null)
                {
                    taxInfo = new TaxInfo()
                    {
                        TTaxEffBgnTm = tax.TTaxEffBgnTm ?? "",
                        TTaxEffEndTm = tax.TTaxEffEndTm ?? "",
                        NTaxableAmt = tax.NTaxableAmt ?? "",
                        NOverdueAmt = tax.NOverdueAmt ?? "",
                        TaxSumAmount = tax.TaxSumAmount ?? "",
                        TaxSumAmountUp = tax.TaxSumAmountUp ?? ""
                    };
                }
                //赋值
                ForceFloatingInfo forceFloatingInfo = new ForceFloatingInfo();
                forceFloatingInfo.ForceBaseInfo = forceBaseInfo;
                forceFloatingInfo.ForceClaim = forceclaim ?? new List<ClaimVo>();
                forceFloatingInfo.TaxInfo = taxInfo;
                viewModel.ForceFloatingInfo = forceFloatingInfo;
            }
            #endregion
            #region 商业险模型
            var bizModel = model.SYFloatingNotificationList;
            if (bizModel != null)
            {
                //初始化
                var bizbase = bizModel.SYBaseVO;
                var bizquote = bizModel.SYCvrgList;
                var bizrate = bizModel.SYprmCoef;
                if (baseCar == null && bizModel.SYVhlVO != null)
                    baseCar = bizModel.SYVhlVO;
                //交换信息
                BizBaseInfo bizBaseInfo = new BizBaseInfo()
                {
                    CPlyNo = "",
                    CAppName = "",
                    TSrcInsrncBgnTm = "",
                    TSrcInsrncEndTm = "",
                    TLastStartDate = "",
                    TLaseEndDate = "",
                    NPrm = "",
                    DNPrm = "",
                    FloatRatio = "",
                    CAgoClmRecQuick = "",
                    NAutoCheCoef = "",
                    NAutoChaCoef = "",
                    NResvNum = "",
                    NIrrRatio = ""
                };
                if (bizbase != null)
                {
                    bizBaseInfo.CPlyNo = bizbase.CPlyNo ?? "";
                    bizBaseInfo.CAppName = bizbase.CAppName ?? "";
                    bizBaseInfo.TSrcInsrncBgnTm = bizbase.TSrcInsrncBgnTm ?? "";
                    bizBaseInfo.TSrcInsrncEndTm = bizbase.TSrcInsrncEndTm ?? "";
                    bizBaseInfo.TLastStartDate = bizbase.TLastStartDate ?? "";
                    bizBaseInfo.TLaseEndDate = bizbase.TLaseEndDate ?? "";
                    bizBaseInfo.NPrm = bizbase.NPrm ?? "";
                    bizBaseInfo.DNPrm = bizbase.DNPrm ?? "";
                }
                if (bizrate != null)
                {
                    bizBaseInfo.FloatRatio = bizrate.FloatRatio ?? "";
                    bizBaseInfo.CAgoClmRecQuick = bizrate.cAgoClmRecQuick ?? "";
                    bizBaseInfo.NAutoCheCoef = bizrate.nAutoCheCoef ?? "";
                    bizBaseInfo.NAutoChaCoef = bizrate.nAutoChaCoef ?? "";
                    bizBaseInfo.NResvNum = bizrate.nResvNum ?? "";
                    bizBaseInfo.NIrrRatio = bizrate.nIrrRatio ?? "";
                }
                //赋值
                BizFloatingInfo bizFloatingInfo = new BizFloatingInfo();
                bizFloatingInfo.BizBaseInfo = bizBaseInfo;
                bizFloatingInfo.BizQuoteResult = bizquote ?? new List<CvrgVO>();
                viewModel.BizFloatingInfo = bizFloatingInfo;
            }
            #endregion
            if (baseCar != null)
            {
                carInfo = new CarInfo()
                {
                    CModelNmeJY = baseCar.CModelNmeJY ?? "",
                    CarType = baseCar.CarType ?? "",
                    CPlateNo = baseCar.CPlateNo ?? "",
                    CPlateTyp = baseCar.CPlateTyp ?? "",
                    CEngNo = baseCar.CEngNo ?? "",
                    CFrmNo = baseCar.CFrmNo ?? "",
                    CRegOwner = baseCar.CRegOwner ?? "",
                };
            }
            viewModel.CarInfo = carInfo;
            return viewModel;
        }
    }
}
