using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class UserClaimRepository : IUserClaimRepository
    {
        private ILog logError = LogManager.GetLogger("ERROR");

        public List<bx_claim_detail> FindList(long buid)
        {
            List<bx_claim_detail> list = new List<bx_claim_detail>();
            try
            {
                list = DataContextFactory.GetDataContext().bx_claim_detail.Where(x => x.b_uid == buid).ToList();

            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return list;
        }

        public async Task<List<bx_claim_detail>> FindListAsync(long buid)
        {
            List<bx_claim_detail> list = new List<bx_claim_detail>();
            try
            {
                list = DataContextFactory.GetDataContext().bx_claim_detail.Where(x => x.b_uid == buid).ToList();

            }
            catch (Exception ex)
            {

                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return list;
        }
        //public void Dispose()
        //{
        //    _entityContext.Dispose();
        //}
    }
}
