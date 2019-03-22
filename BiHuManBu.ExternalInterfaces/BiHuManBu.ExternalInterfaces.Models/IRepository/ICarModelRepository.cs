using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface ICarModelRepository
    {
        bx_carmodel GetCarModel(string vehicleNo);
    }
}
