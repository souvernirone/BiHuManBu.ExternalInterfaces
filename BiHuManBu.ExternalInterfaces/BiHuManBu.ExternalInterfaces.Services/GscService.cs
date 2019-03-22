using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class GscService:IGscService
    {
        private IGscRepository _gscRepository;

        public GscService(IGscRepository gscRepository)
        {
            _gscRepository = gscRepository;
        }

        public bx_gsc_userinfo FindUserInfoByGsc(string usercode, string pwd)
        {
            return _gscRepository.FindUserInfoByGsc(usercode, pwd);
        }
    }
}
