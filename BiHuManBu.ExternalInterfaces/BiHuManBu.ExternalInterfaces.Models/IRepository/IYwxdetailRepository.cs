using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface IYwxdetailRepository
    {
        int DelList(long buid);
        int AddList(List<bx_ywxdetail> jiayiList);
        List<bx_ywxdetail> GetList(long buid);
    }
}
