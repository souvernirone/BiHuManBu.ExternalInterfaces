
using System.Collections.Generic;
namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IHebaoDianweiRepository
    {
        bx_hebaodianwei Find(long buid, int source);
        IList<bx_hebaodianwei> FindList(long buid, long[] sources);
    }
}
