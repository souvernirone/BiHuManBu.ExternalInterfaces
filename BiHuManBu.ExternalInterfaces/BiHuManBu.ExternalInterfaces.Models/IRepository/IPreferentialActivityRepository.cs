using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IPreferentialActivityRepository
    {
        List<bx_preferential_activity> GetActivityByBuid(long buid);

        List<bx_preferential_activity> GetActivityByIds(string stringId);

        Task<List<bx_preferential_activity>> GetActivityByIdsAsync(string stringId);

        bx_preferential_activity AddActivity(bx_preferential_activity request);
        bx_preferential_activity GetListByType(int Type, string ActivityContent);
    }
}
