using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.ChannelService
{
    public class GetSourceOfCityRequest
    {
        [Range(1,10000000)]
        public int Agent { get; set; }
        [Range(1,1000)]
        public int CityCode { get; set; }
        [Required]
        [StringLength(32,MinimumLength = 32)]
        public string SecCode { get; set; }
    }
}