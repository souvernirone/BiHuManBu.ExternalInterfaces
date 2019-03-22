using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IAddressService
    {
        int Add(AddressRequest bxAddress);
        AddressModel Find(int addressId, int childAgent);
        int Delete(int addressId, int childAgent);
        //int Delete(int addressId, string openid);
        int Update(AddressRequest bxAddress);
        List<AddressModel> FindByBuidAndAgentId(int agentId);
        //List<AddressModel> FindByBuidAndAgentId(string openid, int agentId);
    }

}
