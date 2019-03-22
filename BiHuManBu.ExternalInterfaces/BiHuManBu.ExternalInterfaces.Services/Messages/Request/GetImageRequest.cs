using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetImageRequest
    {
        public long buid { get; set; }
        public List<string> yancheimgs { get; set; }
        public string zjimg { get; set; }
    }
}
