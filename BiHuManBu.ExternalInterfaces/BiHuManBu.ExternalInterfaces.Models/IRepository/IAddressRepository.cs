using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IAddressRepository
    {
        int Add(bx_address bxAddress);
        bx_address Find(int addressId, int userid);
        int Delete(int addressId, int userid);
        int Update(bx_address bxAddress);
        List<bx_address> FindByBuidAndAgentId(int userid);//, int agentId
    }
}
