namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface IBatchRenewalRepository
    {
        int UpdateItemStatus(long buId, int itemStatus);
        bx_batchrenewal_item GetItemByBuId(long buId);
        bx_batchrenewal_item GetItemByBuIdSync(long buId);
    }
}
