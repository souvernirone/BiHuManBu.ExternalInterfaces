
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IDeviceDetailRepository
    {
        List<bx_devicedetail> FindList(long buid);

        int Delete(long buid);

        long Add(bx_devicedetail devicedetail);

    }
}
