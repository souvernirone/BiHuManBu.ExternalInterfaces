
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static  class QuoteResultCarInfoMapper
    {
        public static QuoteResultCarInfoViewModel ConvertToViewModel(this bx_quoteresult_carinfo carinfo)
        {
            var model = new QuoteResultCarInfoViewModel();
            if (carinfo != null)
            {
                model.CarEquQuality = carinfo.car_equ_quality.HasValue ? carinfo.car_equ_quality.Value : 0;
                model.CarType = carinfo.car_type.HasValue ? carinfo.car_type.Value : 0;
                model.CarUsedType = carinfo.car_used_type.HasValue ? carinfo.car_used_type.Value : 0;
                model.CarVin = string.IsNullOrWhiteSpace(carinfo.vin_no) ? string.Empty : carinfo.vin_no;
                model.ClauseType = carinfo.clause_type.HasValue ? carinfo.clause_type.Value : 0;
                model.CredentislasNum = string.IsNullOrWhiteSpace(carinfo.owner_idno)
                    ? string.Empty
                    : carinfo.owner_idno;
                model.EngineNo = string.IsNullOrWhiteSpace(carinfo.engine_no) ? string.Empty : carinfo.engine_no;
                model.ExhaustScale = carinfo.exhaust_scale.HasValue ? carinfo.exhaust_scale.Value : 0;
                model.FuelType = carinfo.fuel_type.HasValue ? carinfo.fuel_type.Value : 0;
                model.IdType = carinfo.owner_idno_type.HasValue ? carinfo.owner_idno_type.Value:0;
                model.LicenseColor = carinfo.license_color.HasValue ? carinfo.license_color.Value : 0;
                model.LicenseOwner = string.IsNullOrWhiteSpace(carinfo.license_owner)
                    ? string.Empty
                    : carinfo.license_owner;
                model.LicenseType = carinfo.license_type.HasValue ? carinfo.license_type.Value : 0; 
                model.MoldName = string.IsNullOrWhiteSpace(carinfo.mold_name) ? string.Empty : carinfo.mold_name;
                model.ProofType = carinfo.proof_type.HasValue ? carinfo.proof_type.Value : 0;
                model.PurchasePrice = carinfo.purchase_price.HasValue ? carinfo.purchase_price.Value : 0;
                model.RegisterDate = carinfo.register_date.HasValue
                    ? carinfo.register_date.Value.ToString("yyyy-MM-dd")
                    : string.Empty;
                model.RunRegion = carinfo.run_region.HasValue ? carinfo.run_region.Value : 0;
                model.SeatCount = carinfo.seat_count.HasValue ? carinfo.seat_count.Value : 0;
                model.Source = carinfo.source.Value;
                model.TonCount = carinfo.car_ton_count.HasValue ? carinfo.car_ton_count.Value : 0;
                model.Risk = string.IsNullOrWhiteSpace(carinfo.risk) ? string.Empty : carinfo.risk;
                model.XinZhuanXu = string.IsNullOrWhiteSpace(carinfo.IsZhuanXubao) ? string.Empty : carinfo.IsZhuanXubao;
                model.SyVehicleClaimType = carinfo.SyVehicleClaimType ?? string.Empty;
                model.JqVehicleClaimType = carinfo.JqVehicleClaimType ?? string.Empty;
                model.VehicleStyle = carinfo.VehicleStyle ?? string.Empty;
                model.VehicleAlias = carinfo.VehicleAlias ?? string.Empty;
                model.VehicleYear = carinfo.VehicleYear ?? string.Empty;

            }
            else
            {
                model.CarEquQuality =  0;
                model.CarType = 0 ;
                model.CarUsedType = 0 ;
                model.CarVin = string.Empty;
                model.ClauseType = 0 ;
                model.CredentislasNum =string.Empty;
                model.EngineNo = string.Empty;
                model.ExhaustScale = 0;
                model.FuelType = 0;
                model.IdType = 0;
                model.LicenseColor =0;
                model.LicenseOwner = string.Empty;
                model.LicenseType = 0;
                model.MoldName = string.Empty ;
                model.ProofType = 0;
                model.PurchasePrice =  0;
                model.RegisterDate =string.Empty;
                model.RunRegion = 0;
                model.SeatCount =  0;
                model.Source = -1;
                model.TonCount =0;
                model.Risk = string.Empty;
                model.XinZhuanXu = string.Empty;
                model.SyVehicleClaimType =  string.Empty;
                model.JqVehicleClaimType = string.Empty;
                model.VehicleStyle =string.Empty;
                model.VehicleAlias = string.Empty;
                model.VehicleYear = string.Empty;
            }

            return model;
        }
    }
}
