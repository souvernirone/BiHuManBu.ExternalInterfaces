using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ICityRepository
    {
        List<bx_city> FindList();
        bx_city FindCity(int cityId);
    }
}
