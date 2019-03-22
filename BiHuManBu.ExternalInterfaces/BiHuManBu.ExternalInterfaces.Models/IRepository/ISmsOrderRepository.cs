
namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ISmsOrderRepository
    {
        bx_sms_order Add(bx_sms_order bxSmsOrder);

        bx_sms_order Find(int orderId);
        bx_sms_order Find(string orderNum);

        int Update(bx_sms_order bxSmsOrder);
    }
}
