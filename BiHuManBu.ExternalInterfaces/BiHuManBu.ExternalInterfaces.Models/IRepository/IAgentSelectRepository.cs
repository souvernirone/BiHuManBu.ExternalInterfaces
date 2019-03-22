using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface IAgentSelectRepository
    {
        int AddMulti(string strAddSql);

        int DelMulti(long buid);

        List<long> GetMulti(int agent,int topagent);
    }
}
