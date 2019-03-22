
namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IAppSettingRepository
    {
        bx_appsetting FindByKey(string appSettingKey);
    }
}
