using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IUserClaimRepository
    {
        List<bx_claim_detail> FindList(long buid);

        Task<List<bx_claim_detail>> FindListAsync(long buid);
    }
}
