using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class HistoryContractMapper
    {
        public static HistoryContract ConvertToHistoryContract(this bx_history_contract history)
        {
            var item = new HistoryContract();
            item.Enddate = history.Enddate.HasValue ? history.Enddate.Value.ToString("yyyy-MM-dd") : string.Empty;
            item.InsureCompanyName = history.InsureCompanyName;
            item.IsCommerce = history.IsCommerce ?? 0;
            item.PolicyNo = history.PolicyNo;
            item.Strdate = history.Strdate.HasValue ? history.Strdate.Value.ToString("yyyy-MM-dd") : string.Empty;
            return item;
        }

        public static List<HistoryContract> ConvertToHistoryContract(this List<bx_history_contract> contracts)
        {
            var list = new List<HistoryContract>();
            if (contracts.Count > 0)
            {
                list.AddRange(contracts.Select(ConvertToHistoryContract));
            }
            return list;
        } 
    }
}
