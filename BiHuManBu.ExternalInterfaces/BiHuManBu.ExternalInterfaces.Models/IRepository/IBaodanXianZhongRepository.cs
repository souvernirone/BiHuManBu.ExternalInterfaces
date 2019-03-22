
namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IBaodanXianZhongRepository
    {
        bj_baodanxianzhong Add(bj_baodanxianzhong baodanxianzhong);
        bj_baodanxianzhong Find(long bxid);
        int Update(bj_baodanxianzhong baodanxianzhong);

    }
}
