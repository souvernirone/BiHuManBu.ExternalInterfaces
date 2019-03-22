using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Repository
{
    public class HebaoDianweiRepository : IHebaoDianweiRepository
    {
        public bx_hebaodianwei Find(long buid, int source)
        {
            return DataContextFactory.GetDataContext().bx_hebaodianwei.FirstOrDefault(x => x.buid == buid && x.source == source);
        }
        public IList<bx_hebaodianwei> FindList(long buid, long[] sources)
        {
            List<int> v = new List<int>() { 1 };
            var all = DataContextFactory.GetDataContext().bx_hebaodianwei.Where(x => x.buid == buid).ToList();
            var list = all.Where(x => x.source == null ? false : sources.Contains((long)x.source)).ToList();
            return list;
            //return DataContextFactory.GetDataContext().bx_hebaodianwei.Where(x => x.buid == buid && x.source == null ? false : sources.Contains((long)x.source)).ToList();

        }

    }
}
