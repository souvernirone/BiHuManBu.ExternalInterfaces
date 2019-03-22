using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class CarChargeMapper
    {
        public static CarChargeViewModel.CarChargeItem ConvertToViewModel(this UpdateChargeResponse response)
        {
            CarChargeViewModel.CarChargeItem view = new CarChargeViewModel.CarChargeItem();
            if (response != null)
            {
                view.CarVin = response.CarVin;
                view.EngineNo = response.EngineNo;
                //view.LicenseOwner = response.LicenseOwner;
                view.MoldName = response.MoldName;
                //view.OwnerIdNo = response.OwnerIdNo;
                view.LicenseNo = response.LicenseNo;
                view.RegisterDate = response.RegisterDate;
                view.TotalCount = response.TotalCount;
                view.UsedCount = response.UsedCount;

            }
            return view;
        }

        public static CarClaimViewModel ConvertToViewModel(this List<bx_car_claims> items)
        {
            CarClaimViewModel view = new CarClaimViewModel();
            view.List = new List<CarClaimViewModel.CarClaim>();
            if (items != null)
            {
                foreach (bx_car_claims item in items)
                {
                    CarClaimViewModel.CarClaim claim = new CarClaimViewModel.CarClaim
                    {
                        EndcaseTime =
                            item.endcase_time.HasValue ? item.endcase_time.Value.ToString("yyyy-MM-dd") : string.Empty,
                        LossTime = item.loss_time.HasValue ? item.loss_time.Value.ToString("yyyy-MM-dd") : string.Empty,
                        PayAmount = item.pay_amount.HasValue ? item.pay_amount.Value : 0,
                        PayCompanyName = item.pay_company_name,
                        PayType = item.pay_type.HasValue ? item.pay_type.Value : 0
                    };

                    view.List.Add(claim);
                }
            }

            return view;
        }
    }
}
