using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ISaveQuoteRepository
    {
        bx_savequote GetSavequoteByBuid(long buid);

        Task<bx_savequote> GetSavequoteByBuidAsync(long buid);
        long Add(bx_savequote savequote);

        int Update(bx_savequote savequote);
    }
}
