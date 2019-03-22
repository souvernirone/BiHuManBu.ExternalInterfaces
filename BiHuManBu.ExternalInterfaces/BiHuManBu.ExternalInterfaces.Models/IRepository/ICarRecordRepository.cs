
namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface ICarRecordRepository
    {
        int AddUpdateCarRecord(bx_car_record model);
        bx_car_record GetModel(string carVin, string engino);
    }
}
