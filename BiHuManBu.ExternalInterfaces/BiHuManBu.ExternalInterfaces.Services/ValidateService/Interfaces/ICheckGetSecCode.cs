using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces
{
    public interface ICheckGetSecCode
    {
        bool ValidateReqest(IEnumerable<KeyValuePair<string, string>> list, string configKey, string checkCode);
    }
}
