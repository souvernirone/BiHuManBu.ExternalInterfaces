using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IDriverLicenseTypeService
    {
        DriverLicenseTypeResponse GetList(GetDriverLicenseCarTypeRequest request, IEnumerable<KeyValuePair<string, string>> pairs);
    }
}
