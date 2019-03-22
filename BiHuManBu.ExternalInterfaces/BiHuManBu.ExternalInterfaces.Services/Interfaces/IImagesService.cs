using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IImagesService
    {
        int Add(GetImageRequest request);
        List<bx_images> GetImages(long buid);
    }
}
