using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public interface ISetPrecisePriceItemNew
    {
        List<long> FindSource(List<long> listquote01, int agent);
    }
}
