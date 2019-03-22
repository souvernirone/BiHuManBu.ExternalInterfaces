using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public interface ICreateActivity
    {
        bx_preferential_activity AddActivity(CreateOrUpdateBjdInfoRequest request, int aciivityType);
    }
}
