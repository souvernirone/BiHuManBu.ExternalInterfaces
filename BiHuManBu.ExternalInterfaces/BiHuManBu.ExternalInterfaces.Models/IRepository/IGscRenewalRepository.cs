using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IGscRenewalRepository
    {
        List<bx_gsc_renewal> FindListByBuid(long buid);
    }
}
