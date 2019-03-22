using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface IDriverLicenseTypeRepository
    {
        List<bx_drivelicense_cartype> FindList();
    }
}
