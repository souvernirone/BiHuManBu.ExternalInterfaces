using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class CarModelRepository:ICarModelRepository
    {
        public bx_carmodel GetCarModel(string vehicleNo)
        {
            var item = DataContextFactory.GetDataContext().bx_carmodel.FirstOrDefault(x => x.VehicleNo == vehicleNo);
            return item;

        }
    }
}
