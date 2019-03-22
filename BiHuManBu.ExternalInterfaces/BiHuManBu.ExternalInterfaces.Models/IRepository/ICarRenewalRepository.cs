
namespace BiHuManBu.ExternalInterfaces.Models
{
    public  interface ICarRenewalRepository
    {
        bx_car_renewal FindByLicenseno(string licenseno);

        bx_car_renewal FindCarRenewal(long buid);
        int GetCarUsedType(long buid);
    }
}
