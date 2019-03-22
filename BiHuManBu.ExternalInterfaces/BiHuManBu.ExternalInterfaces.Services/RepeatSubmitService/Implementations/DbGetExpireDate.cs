using System;
using System.Linq;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Repository.DbOper;
using BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.Implementations
{
    public class DbGetExpireDate : IGetExpireDate
    {
        private readonly IRepository<bx_lastinfo> _repository;

        public DbGetExpireDate(IRepository<bx_lastinfo> repository)
        {
            _repository = repository;
        }

        public async Task<bx_lastinfo> GetDate(string identity)
        {
            var buid = Convert.ToInt64(identity);

            return _repository.Search(x => x.b_uid == buid).FirstOrDefault();
        }
    }
}