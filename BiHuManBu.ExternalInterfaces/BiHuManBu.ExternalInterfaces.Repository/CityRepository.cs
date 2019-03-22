using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CityRepository:ICityRepository
    {
        public List<bx_city> FindList()
        {
            return DataContextFactory.GetDataContext().bx_city.ToList();
        }

        public bx_city FindCity(int cityId)
        {
            return DataContextFactory.GetDataContext().bx_city.FirstOrDefault(i => i.id == cityId);
        }

    }
}
