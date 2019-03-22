using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public interface IUpdateBjdCheck
    {
        UpdateBjdCheckMessage Valid(CreateOrUpdateBjdInfoRequest request);

    }
}
