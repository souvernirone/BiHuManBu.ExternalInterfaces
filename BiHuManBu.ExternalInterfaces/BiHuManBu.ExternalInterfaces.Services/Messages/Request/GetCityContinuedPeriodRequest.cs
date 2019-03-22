using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetCityContinuedPeriodRequest : BaseRequest
    {
        [Range(0,1)]
        public int ShowName { get; set; }
    }
}
