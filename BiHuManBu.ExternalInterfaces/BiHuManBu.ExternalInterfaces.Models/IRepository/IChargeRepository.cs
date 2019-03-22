
namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IChargeRepository
    {
        int Add(bx_charge charege);

        bx_charge Find(int agent,int chargeType);
        int Update(bx_charge charege);
    }
}
