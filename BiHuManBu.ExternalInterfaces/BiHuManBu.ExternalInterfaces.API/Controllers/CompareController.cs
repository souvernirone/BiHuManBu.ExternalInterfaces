using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class CompareController:ApiController
    {
        private static readonly string _url = System.Configuration.ConfigurationManager.AppSettings["BaoxianCenter"];


        public string GetXubao(int num)
        {
            for (int i = 0; i < num; i++)
            {
                ParameterizedThreadStart ParStart = new ParameterizedThreadStart(test);
                Thread myThread = new Thread(ParStart);
                myThread.Start();
            }
            return string.Format("请求次数{0}", num);
        }

        public void test()
        {
            using (HttpClient client = new HttpClient())
            {
                var getLoginUrl = "http://192.168.3.12:8099/api/CarInsurance/getreinfo?licenseno=%E4%BA%ACP8WB5X&CITYCODE=2&AGENTID=103&SECcODE=12121212121212121212121212121212";
                var tt = client.GetAsync(getLoginUrl).Result;
            }
           
        }

        public async  Task<HttpResponseMessage> GetXubao()
        {
            string[] licensenos =
            {
                "京NG0N25"
                , "京GMW852"
                , "京N2CH57"
                , "京N3ML56"
                , "京QX0H69"
                , "京HJ1010"
                , "京JV7287"
                , "京ND9465"
                , "京NM3S29"
                , "京ND3766"
                , "京N6KG76"
                , "京QEY898"
                , "京N397Y5"
                , "京N149Q6"
                , "京N5XN85"
                , "京NPL222"
                , "京CX6182"
                , "京QHG108"
                , "京PQV528"
                , "京YX1222"
                , "京NW9J38"
                , "京Q0GU03"
                , "京F16611"
                , "京N7S851"
                , "京N31U31"
                , "京HQ1801"
                , "京N36C72"
                , "京N3HU88"
                , "京N7E9W8"
                , "京MM5556"
                , "京NYM347"
                , "京N0M8U6"
                , "京P6S083"
                , "京JW9069"
                , "京PC2898"
                , "京N9UY23"
                , "京MW0602"
                , "京Y12020"
                , "京P28E81"
                , "京NEV776"
                , "京P81R38"
                , "京HB4657"
                , "京N5QM36"
                , "京P6MY90"
                , "京QR8K37"
                , "京A32346"
                , "京NF33M9"
                , "京AV6312"
                , "京Q259H6"
                , "京Q255C7"
                , "京N9W322"
                , "京N800M2"
                , "京N331U8"
                , "京N69A77"
                , "京EM4019"
                , "京NT3C15"
                , "京GYL660"
                , "京N2AE83"
                , "京GGJ060"
                , "京N80ZJ8"
                , "京J05499"
                , "京N3S6B9"
                , "京NQ00C1"
                , "京PR3013"
                , "京N01AB7"
                , "京NNK205"
                , "京N8W3Q7"
                , "京EU0583"
                , "京NM27C8"
                , "京NF1L97"
                , "京JC4487"
                , "京PF7766"
                , "京Q8BH59"
                , "京P99C80"
                , "京N9J093"
                , "京N5X2Y7"
                , "京NNY175"
                , "京N88SC3"
                , "京PDR669"
                , "京NX16V9"
                , "京NK4803"
                , "京N88JH6"
                , "京N546Y8"
                , "京L77073"
                , "京N66Y40"
                , "京N961E5"
                , "京HQ0121"
                , "京QA5137"
                , "京NW4134"
                , "京NY8Z01"
                , "京HC1253"
                , "京YJ7907"
                , "京GRC567"
                , "京NS78V9"
                , "京QC80J1"
                , "京YAD292"
                , "京N6MF57"
                , "京N9Y5S3"
                , "京N6XG79"
                , "京KH5705"
                , "京GKJ711"
            };


            //for (int i = 0; i < 30; i++)
            //{
            //    ParameterizedThreadStart ParStart = new ParameterizedThreadStart(test);
            //    Thread myThread = new Thread(ParStart);
            //    myThread.Start(licensenos[i]);
            //}
           
           

            return new HttpResponseMessage()
            {
                Content = new StringContent("发送成功")
            };



        }

        public  void test(object licenseno)
        {
            using (HttpClient client = new HttpClient())
            {


                List<int> coms = new List<int> { 0, 1, 2 };
                int seccode = 3949;
                int intentitonCompany = coms[(new Random().Next(0, coms.Count))];//随机
                int lastyearCompany = -1;
                string carlicense = licenseno.ToString();
                int agent = 4949;
                string mobile = "18310825788";
                string geturl =
                 string.Format(
                     "http://m.91bihu.com/api/InsuranceBusiness/GetReInsuranceInfo?secCode={0}&citycode=110000&carType=0&isNewCar=0&userIdentity=&useType=0&SubmitGroup={1}&lastYearCompany={2}&CarLicense={3}&agent={4}&mobile={5}",
                     seccode, intentitonCompany, lastyearCompany, carlicense, agent, mobile);
                var getLoginUrl = geturl;
                var tt = client.GetAsync(getLoginUrl).Result;





            }
        }
    }
}