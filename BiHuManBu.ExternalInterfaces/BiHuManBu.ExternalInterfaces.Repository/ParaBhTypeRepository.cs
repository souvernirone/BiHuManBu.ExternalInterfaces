using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class ParaBhTypeRepository : IParaBhTypeRepository
    {
        private ILog _logError = LogManager.GetLogger("ERROR");

        public List<bx_para_bhtype> GetListByTypeId(int typeId, int isAll)
        {
            var list = new List<bx_para_bhtype>();
            try
            {
                list = DataContextFactory.GetDataContext().bx_para_bhtype.Where(x => x.bh_type == typeId && (isAll == 1 ? true : x.is_support == 1)).ToList();
            }
            catch (Exception ex)
            {
                _logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return list;
        }
    }
}
