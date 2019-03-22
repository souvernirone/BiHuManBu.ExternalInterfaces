
namespace BiHuManBu.ExternalInterfaces.Services.ConfigService.Interfaces
{
    public interface IGetConfigValueService
    {
        string GetConfigValue(string configKey, int configType, string cacheKey);
    }
}
