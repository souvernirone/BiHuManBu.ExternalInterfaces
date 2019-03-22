using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ICarOrderSaveQuoteRepository
    {
        bx_order_savequote GetSavequoteByBuid(long buid);
        long Add(bx_order_savequote savequote);

        int Update(bx_order_savequote savequote);
    }
}
