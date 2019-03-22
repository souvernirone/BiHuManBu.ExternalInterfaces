using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface ISpecialOptionRepository
    {
        int AddList(List<bx_specialoption> specialOptionList);
        int DelList(long buid);
    }
}
