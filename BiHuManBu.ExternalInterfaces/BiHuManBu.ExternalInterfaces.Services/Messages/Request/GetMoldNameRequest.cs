using System.ComponentModel.DataAnnotations;
namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetMoldNameRequest
    {
        [Range(1, 10000000)]
        public int Agent { get; set; }
        [RegularExpression(@"^[^L].{4,50}",ErrorMessage = "该接口只适用于进口车型，进口车的车架号不是L开头，车架号长度是50个以内的大写字符长度")]
        public string CarVin { get; set; }
        [Range(1, 10000)]
        public int CityCode { get; set; }
        public string SecCode { get; set; }
    }
}
