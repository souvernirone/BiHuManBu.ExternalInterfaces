namespace BiHuManBu.ExternalInterfaces.Models
{
    public  interface ICarInfoRepository
    {
        bx_carinfo Find(string licenseno);
        bx_carinfo FindOrderDate(string licenseno,int renewaltype=0);
        bx_carinfo FindVinCarInfo(string carVin, int renewaltype = 0);
    }
}
