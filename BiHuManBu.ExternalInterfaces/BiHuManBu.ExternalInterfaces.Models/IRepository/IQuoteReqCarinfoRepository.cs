
namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IQuoteReqCarinfoRepository
    {
        bx_quotereq_carinfo Find(long buid);
        bx_quotereq_carinfo Add(bx_quotereq_carinfo item);

        int Update(bx_quotereq_carinfo item);
    }
}
