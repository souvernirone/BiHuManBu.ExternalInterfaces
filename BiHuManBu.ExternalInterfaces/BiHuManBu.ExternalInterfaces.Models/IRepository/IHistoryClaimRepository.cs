using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IHistoryClaimRepository
    {
        int Add(List<bx_history_contract> contracts);
        List<bx_history_contract> FindList(string licenseNo);
        int Remove(string licenseno);

    }
}
