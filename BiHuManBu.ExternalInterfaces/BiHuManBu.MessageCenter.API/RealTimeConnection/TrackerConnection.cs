using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.MessageCenter.API.Demo;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace BiHuManBu.MessageCenter.API.RealTimeConnection
{
    public class TrackerConnection : PersistentConnection
    {
        private IDemoService _demoService;
        private IAddressRepository _addressRepository;

        public TrackerConnection(IDemoService demoService,IAddressRepository addressRepository)
        {
            _demoService = demoService;
            _addressRepository = addressRepository;
        }

        protected override async System.Threading.Tasks.Task OnReceived(IRequest request, string connectionId, string data)
        {
            var  t= _demoService.Add(2);
            var s = _addressRepository.Find(1, 1);
           
            await Connection.Send(connectionId,data);
        }
    }
}