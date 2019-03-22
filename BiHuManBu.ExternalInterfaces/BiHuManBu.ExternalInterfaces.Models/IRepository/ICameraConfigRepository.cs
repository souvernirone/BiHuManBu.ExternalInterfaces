

using System.Collections.Generic;
namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ICameraConfigRepository
    {
        int Update(bx_camera_config model);
        List<bx_camera_config> Get(int agentId);
    }
}
