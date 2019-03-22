using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface IQuoteManySourceRepository
    {
        bx_quote_many_source GetModel(int agent, int source);
        List<bx_quote_many_source> GetModels(int agent, string sources);
        int Update(bx_quote_many_source model);
        long Add(bx_quote_many_source model);
    }
}
