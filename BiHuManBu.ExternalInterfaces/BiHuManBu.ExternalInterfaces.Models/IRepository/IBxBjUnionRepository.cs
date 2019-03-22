
namespace BiHuManBu.ExternalInterfaces.Models
{
     public interface IBxBjUnionRepository
     {
         int Add(long buid, long bxid);
         bx_bj_union Find(long bxid);
     }
}
