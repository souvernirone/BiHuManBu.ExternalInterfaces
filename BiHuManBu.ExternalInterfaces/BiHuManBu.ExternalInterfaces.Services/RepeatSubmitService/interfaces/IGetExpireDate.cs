using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.interfaces
{
    public interface IGetExpireDate
    {
       Task<bx_lastinfo> GetDate(string identity);
    }
}