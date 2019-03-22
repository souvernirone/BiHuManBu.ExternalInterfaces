
using System.Collections.Generic;
using System.Net.Configuration;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ITransferRecordRepository
    {
        long Add(bx_transferrecord record);

        List<bx_transferrecord> FindListByBuidList(List<long> buids);

        bx_transferrecord FindFirstSaByBuid(long buid);

        List<bx_transferrecord> FindByBuid(long buid);

    }

}
