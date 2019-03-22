using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ICarOrderQuoteResultRepository
    {
        long Add(bx_order_quoteresult quoteresult);
        bx_order_quoteresult GetQuoteResultByBuid(long buid,int source);
    }
}
