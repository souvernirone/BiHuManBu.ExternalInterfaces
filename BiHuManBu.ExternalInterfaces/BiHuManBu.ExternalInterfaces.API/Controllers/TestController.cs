using BiHuManBu.ExternalInterfaces.Infrastructure;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class TestController:ApiController
    {
        [HttpGet]
        public HttpResponseMessage TestWeb()
        {
            return "1".ResponseToJson();
        }
        public async Task<HttpResponseMessage> Go()
        {
            var licenselist = @"京HA5575
                            ,京QZP150
                            ,京NET353
                            ,京N78TY9
                            ,京NJU257
                            ,京Q5FF86
                            ,京PZ1X37
                            ,京QMM098
                            ,京CM5970
                            ,京PM0K37
                            ,京QQ1C88
                            ,京N005G3
                            ,京ND7588
                            ,京Q5KL05
                            ,京A11523
                            ,京NG2L56
                            ,京MA9191
                            ,京NT3451
                            ,京NYC175
                            ,京P7A669
                            ,京A70728
                            ,京QLC096
                            ,京LB5210
                            ,京L99512
                            ,京QC97G0
                            ,京SB8050
                            ,京QA72Q8
                            ,京DA4171
                            ,京MP1986
                            ,京QQ29C0
                            ,京KL5685
                            ,京WPG127
                            ,京N1C293
                            ,京NXG683
                            ,京QJ20C0
                            ,京SFC101
                            ,京PA5586
                            ,京MJ8869
                            ,京WJP128
                            ,京QH7B89
                            ,京N711Z9
                            ,京PYA892
                            ,京YJ8157
                            ,京NEU902
                            ,京P313F5
                            ,京P9VT73
                            ,京FD6011
                            ,京N86HL5
                            ,京NKX624
                            ,京SD0925
                            ,京Q1Q901
                            ,京NB2X08
                            ,京AH9924
                            ,京NG5T76
                            ,京GJ1045
                            ,京N62932
                            ,京DR1201
                            ,京P17D50
                            ,京NED477
                            ,京L60083
                            ,京NDC627
                            ,京NNG458
                            ,京NX1W06
                            ,京N74X90
                            ,京FV2051
                            ,京QYC672
                            ,京DS0323
                            ,京P998P8
                            ,京LQ0096
                            ,京N305p0
                            ,京JX1454
                            ,京Q0UQ23
                            ,京Q7HA13
                            ,京PG3207
                            ,京MP5080
                            ,京EJ9026
                            ,京Q76978
                            ,京NDJ678
                            ,京NT4497
                            ,京TE5671
                            ,京Qqu869
                            ,京QA7G56
                            ,京EC4796
                            ,京MWF478
                            ,京PDX128
                            ,京PKL857
                            ,京JDS012
                            ,京PYQ612
                            ,京Q6ZQ38
                            ,京QE5989
                            ,京p8k990
                            ,京N16Q28
                            ,京N2RZ18
                            ,京KU6655
                            ,京QR19J0
                            ,京LF5885
                            ,京NM0A80
                            ,京GF1017
                            ,京N158B7
                            ,京NX5D11
                            ";

            var list = licenselist.Split(',');
            foreach (var item in list)
            {
                using (HttpClient client = new HttpClient())
                {
                    //var url = System.Configuration.ConfigurationManager.AppSettings["CarInsuranceGetPriceInfo"];
                    var geturl = string.Format("http://192.168.1.65:8099/api/CarInsurance/getreinfo?licenseno={0}&agent={1}&seccode={2}&custkey={3}&citycode=1", item.Replace("\r\n", "").Replace(" ", ""), 102, "14236514236514236514236514236514", "kkkkkkgfxp");
                   
                    var clientResult =await client.GetAsync(geturl);
                    if (clientResult.IsSuccessStatusCode)
                    {
                      var  str1 = await clientResult.Content.ReadAsStringAsync();
                    }
                }
            }

            return new HttpResponseMessage();
        }
    }
}