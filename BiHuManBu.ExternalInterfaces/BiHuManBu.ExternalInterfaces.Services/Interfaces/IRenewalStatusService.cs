using System.Collections.Generic;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
   public interface IRenewalStatusService
    {
       long AddRenewalStatus(int status, GetReInfoRequest request);
    }
}
