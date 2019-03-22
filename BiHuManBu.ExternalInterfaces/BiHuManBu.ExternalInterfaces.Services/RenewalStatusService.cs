using System;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using log4net;
using ServiceStack.Text;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class RenewalStatusService : IRenewalStatusService
    {
        private readonly IRenewalStatusRepository _renewalStatusrepository;
        public RenewalStatusService(IRenewalStatusRepository renewalStatusRepository)
        {
            _renewalStatusrepository = renewalStatusRepository;
        }
        public long AddRenewalStatus(int status, GetReInfoRequest request)
        {
            bx_renewalstatus renewalStatus = new bx_renewalstatus
            {
                RenewalStatus=status,
                LicenseNo=request.LicenseNo,
                Agent=request.Agent.ToString(),
                CreateTime=DateTime.Now
            };
            return _renewalStatusrepository.Add(renewalStatus);
        }
    }
}
