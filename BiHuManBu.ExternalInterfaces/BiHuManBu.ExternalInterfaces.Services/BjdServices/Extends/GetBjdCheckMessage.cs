using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class GetBjdCheckMessage
    {
        public string Message { get; set; }
        public int State { get; set; }

        public bj_baodanxinxi Baodanxinxi { get; set; }

        public bj_baodanxianzhong Baodanxianzhong { get; set; }

        public bx_bj_union BjUnion { get; set; }
    }
}
