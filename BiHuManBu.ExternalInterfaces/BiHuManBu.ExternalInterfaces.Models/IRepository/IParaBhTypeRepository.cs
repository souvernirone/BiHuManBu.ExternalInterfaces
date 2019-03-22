
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IParaBhTypeRepository
    {
        List<bx_para_bhtype> GetListByTypeId(int typeId, int isAll);
    }
}
