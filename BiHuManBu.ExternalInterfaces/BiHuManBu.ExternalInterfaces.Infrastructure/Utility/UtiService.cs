using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.Utility
{
    public static class UtiService
    {
        public static Dictionary<int, int> GetCompnayTransSource()
        {
            var quoteCompany = new Dictionary<int, int>();
            quoteCompany.Add(1, 1);
            quoteCompany.Add(2, 0);
            quoteCompany.Add(4, 2);
            quoteCompany.Add(8, 3);
            quoteCompany.Add(32, 5);
            quoteCompany.Add(64, 6);
            quoteCompany.Add(128, 7);
            quoteCompany.Add(256, 8);
            quoteCompany.Add(512, 9);
            quoteCompany.Add(1024, 10);
            quoteCompany.Add(2048, 11);
            return quoteCompany;
        }
    }
}
