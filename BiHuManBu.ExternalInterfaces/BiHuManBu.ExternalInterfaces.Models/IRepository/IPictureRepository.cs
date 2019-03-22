
namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface IPictureRepository : IRepositoryBase<bx_picture>
    {
        int UpdateState(long buid);
    }
}
