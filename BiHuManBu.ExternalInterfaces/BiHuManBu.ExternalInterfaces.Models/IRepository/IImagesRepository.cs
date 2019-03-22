using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IImagesRepository
    {
        int Add(List<bx_images> images);
        List<bx_images> FindByBuid(long buid);
    }
}
