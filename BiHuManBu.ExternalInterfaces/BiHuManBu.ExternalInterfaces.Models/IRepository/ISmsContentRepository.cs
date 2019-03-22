
namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface  ISmsContentRepository
    {
        bx_sms_account Find(int agent);
        int Add(bx_sms_account bxSmsAccount);
        int Update(bx_sms_account bxSmsAccount);
    }
}
