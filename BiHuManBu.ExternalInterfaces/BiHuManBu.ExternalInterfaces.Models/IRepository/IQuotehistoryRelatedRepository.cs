namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface IQuotehistoryRelatedRepository
    {
        bx_quotehistory_related GetModel(long buid, long groupspan);
    }
}
