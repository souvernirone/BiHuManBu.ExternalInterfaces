
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface IConfigRepository
    {
        bx_config Find(string configKey, int configType);
        List<bx_config> FindList(int configType);
    }
}
