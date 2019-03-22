using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ICustomerCategoriesRepository
    {
        bx_customercategories Get(int id);
        bx_customercategories GetAsync(int id);
    }
}
