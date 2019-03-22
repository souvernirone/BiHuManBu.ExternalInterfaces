
using System.Data;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IBaodanxinxiRepository
    {
        bj_baodanxinxi Add(bj_baodanxinxi baodanxinxi);
        bj_baodanxinxi Find(long bxid);
        BjdCorrelateViewModel Finds(long bxid);
        int Update(bj_baodanxinxi baodanxinxi);
    }
}
