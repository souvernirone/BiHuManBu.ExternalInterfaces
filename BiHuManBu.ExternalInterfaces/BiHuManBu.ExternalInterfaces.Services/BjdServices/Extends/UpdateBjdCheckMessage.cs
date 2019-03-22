using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public  class UpdateBjdCheckMessage
    {
        public string Message { get; set; }
        public int State { get; set; }

        public bx_submit_info SubmitInfo { get; set; }

        public bx_quoteresult Quoteresult { get; set; }

        public bx_userinfo Userinfo { get; set; }

        public bx_savequote Savequote { get; set; }

        public bx_quotereq_carinfo ReqCarInfo { get; set; }
    }
}
