using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace BiHuManBu.ExternalInterfaces.Infrastructure
{
    public static class CommonHelper
    {
        public static List<T> ToListT<T>(this string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<List<T>>(json.ToString());
        }

        public static string TToJson(DataTable dt)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                var result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }

            return js.Serialize(list);
        }

        public static string GetMd5(this string message)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                byte[] md5Bytes = md5.ComputeHash(bytes);
                foreach (byte item in md5Bytes)
                {
                    stringBuilder.Append(item.ToString("x2"));
                }
            }
            return stringBuilder.ToString();

        }

        public static string GetUrl(this string url)
        {
            string[] arr = url.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(arr);
            return string.Join("&", arr);
        }

        public static string StringToHash(this string str)
        {
            var idBytes = Encoding.UTF8.GetBytes(str);
            var hashBytes = new SHA1Managed().ComputeHash(idBytes);
            var hex = BitConverter.ToString(hashBytes);
            return hex;
        }

        public static string GetParasString(IEnumerable<KeyValuePair<string, string>> list)
        {
            if (!list.Any()) return string.Empty;
            StringBuilder inputParamsString = new StringBuilder();
            foreach (KeyValuePair<string, string> item in list)
            {

                inputParamsString.Append(string.Format("{0}={1}&", item.Value, item.Key));

            }

            var content = inputParamsString.ToString();
            var securityString = content.Substring(0, content.Length - 1);

            return securityString;
        }

        public static HttpResponseMessage ResponseToJson(this object obj, HttpStatusCode code = HttpStatusCode.OK)
        {
            String str;
            str = obj.ToJson();
            var result = new HttpResponseMessage
            {
                Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            result.StatusCode = code;
            return result;
        }

        public static HttpResponseMessage ResponseToJsonReplaceType(this object obj, HttpStatusCode code = HttpStatusCode.OK)
        {
            String str;
            str = obj.ToJson();
            Regex reg = new Regex("\\\"__type\\\":\\\"[\\w\\W.]+?\\\",");
            str = reg.Replace(str, "");
            var result = new HttpResponseMessage
            {
                Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            result.StatusCode = code;
            return result;
        }

        public static string GetOrderNum()
        {
            StringBuilder formcode = new StringBuilder();
            formcode.Append(DateTime.Now.Year.ToString());
            formcode.Append(DateTime.Now.Month.ToString().Length == 1
                ? "0" + DateTime.Now.Month
                : DateTime.Now.Month.ToString());
            formcode.Append(DateTime.Now.Day.ToString().Length == 1
                ? "0" + DateTime.Now.Day
                : DateTime.Now.Day.ToString());
            formcode.Append(DateTime.Now.Hour.ToString().Length == 1
                ? "0" + DateTime.Now.Hour
                : DateTime.Now.Hour.ToString());
            formcode.Append(DateTime.Now.Minute.ToString().Length == 1
                ? "0" + DateTime.Now.Minute
                : DateTime.Now.Minute.ToString());
            formcode.Append(DateTime.Now.Second.ToString().Length == 1
                ? "0" + DateTime.Now.Second
                : DateTime.Now.Second.ToString());
            if (DateTime.Now.Millisecond.ToString().Length == 1)
            {
                formcode.Append("00" + DateTime.Now.Millisecond);
            }
            else if (DateTime.Now.Millisecond.ToString().Length == 2)
            {
                formcode.Append("0" + DateTime.Now.Millisecond);
            }
            else
            {
                formcode.Append(DateTime.Now.Millisecond.ToString());
            }
            return formcode.ToString();
        }

        public static IEnumerable<KeyValuePair<string, string>> EachProperties<T>(T obj)
        {
            var pairs = new List<KeyValuePair<string, string>>();
            Type type = obj.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo info in ps)
            {
                object o = info.GetValue(obj, null);
                //if (o == null) continue;
                string name = info.Name;
                if (o == null)
                {
                    pairs.Add(new KeyValuePair<string, string>(string.Empty, name));
                }
                else
                {
                    pairs.Add(new KeyValuePair<string, string>(o.ToString(), name));
                }

            }
            return pairs;
        }
        public static IEnumerable<KeyValuePair<string, string>> ReverseEachProperties<T>(T obj)
        {
            var pairs = new List<KeyValuePair<string, string>>();
            Type type = obj.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo info in ps)
            {
                object o = info.GetValue(obj, null);
                if (o == null) continue;
                string name = info.Name;
                pairs.Add(new KeyValuePair<string, string>(name, o.ToString()));
            }
            return pairs;
        }

        public static bool IsEmail(this string emailString)
        {
            Regex re = new Regex(@"^[\w-\.]+@(?:[A-Za-z0-9-]+\.)+[a-z]+(,[\w-\.]+@(?:[A-Za-z0-9-]+\.)+[a-z]+)*$");
            return re.IsMatch(emailString);
        }
        #region 校验身份证
        public static bool IsValidIdCard(this string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
            {
                return false;
            }

            if (idCard.Length == 18)
            {
                bool check = CheckIDCard18(idCard);
                return check;
            }
            else if (idCard.Length == 15)
            {
                bool check = CheckIDCard15(idCard);
                return check;
            }
            else
            {
                return false;
            }
        }
        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }
        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            return true;//符合15位身份证标准  
        }
        #endregion
        /// <summary>
        /// 比较两个数组的值是否相同
        /// 如果数组中有重复的数据会返回false
        /// </summary>
        /// <param name="target"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ArrayCompare(string[] target, string[] input)
        {
            var tL = target.Length;
            var iL = input.Length;

            //先比较数组长度
            if (tL != iL)
            {
                return false;
            }

            var t = target.Distinct().Count();
            var i = input.Distinct().Count();

            if (tL != t || iL != i)
            {
                return false;
            }

            foreach (var item in target)
            {
                if (!input.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool ArrayCompare2(string[] target, string[] input)
        {
            foreach (var item in input)
            {
                if (target.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        #region 校验组织机构代码
        /// <summary>
        /// 校验组织机构的代码
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool IsValidZZJG(this string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
            {
                return false;
            }
            return CheckZZJG(idCard);
        }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static bool CheckZZJG(string Id)
        {
            if (Id.Length != 9 && Id.Length != 10 && Id.Length != 18)
            {
                return false;
            }
            //前8位只能是数字或大写字母，最后1位只能是数字、字母X，第9位可以为-
            string regexZZJG910 = @"^[0-9A-Z]{18}|[0-9A-Z]{8}-?[0-9X]$";
            var regex = new Regex(regexZZJG910);
            if (!regex.IsMatch(Id))
            {
                return false;
            }
            return true;
        }
        #endregion
        #region 校验营业执照，统一信用代码
        /// <summary>
        /// 校验营业执照的代码
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool IsValidYYZZ(this string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
            {
                return false;
            }
            if (idCard.Length == 18)
            {
                bool check = CheckYYZZ18(idCard);
                return check;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 营业执照只判断18位
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static bool CheckYYZZ18(string Id)
        {
            if (Id.Length != 18)
            {
                return false;
            }
            //第1 - 2位为数字0 - 9或英文字母A - Z，
            //第3 - 8位为数字0 - 9，
            //第9 - 16位为数字0 - 9或英文字母A - Z，
            //第17位为数字0 - 9或X
            //第18位为数字0 - 9或英文字母A - Z
            string regexYYZZ18 = @"^[A-Z0-9]{2}[0-9]{6}[A-Z0-9]{8}[X-X0-9]{1}[A-Z0-9]{1}$";
            var regex = new Regex(regexYYZZ18);
            if (!regex.IsMatch(Id))
            {
                return false;
            }
            return true;
        }
        #endregion
        /// <summary>
        /// 证件号是否带星，如果带星，则有问题
        /// 空和带星，返回false
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool IsValidStar(this string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
            {
                return false;
            }
            if (!idCard.Contains('*'))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 车牌号校验
        /// </summary>
        /// <param name="licenseno"></param>
        /// <returns></returns>
        public static bool IsValidLicenseno(this string licenseno)
        {
            if (string.IsNullOrWhiteSpace(licenseno))
            {
                return false;
            }
            //车牌号校验
            string regexLicenseno = @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-HJ-Z]{1}[A-HJ-NP-Z0-9]{3,6}[A-HJ-NP-Z0-9挂学警港澳]{1}$";
            var regex = new Regex(regexLicenseno);
            if (!regex.IsMatch(licenseno))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 日期格式校验
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsValidDate(this string date)
        {
            string regexDate = @"^[1-9]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$";
            var regex = new Regex(regexDate);
            if (!regex.IsMatch(date))
            {
                return false;
            }
            return true;
        }

        #region 校验港澳居民来往通行证
        public static bool IsValidTXZ(this string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
            {
                return false;
            }
            //第一位字母，其他为数字，共9位或11位
            string regexTXZ11 = @"^[A-Z][0-9]{10}$";
            string regexTXZ9 = @"^[A-Z][0-9]{8}$";
            var regex11 = new Regex(regexTXZ11);
            var regex9 = new Regex(regexTXZ9);
            if (!regex11.IsMatch(idCard) && !regex9.IsMatch(idCard))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 校验港澳身份证
        public static bool IsValidGASFZ(this string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
            {
                return false;
            }
            //香港身份证一个英文+6个数字+（一个校验码0~9或A）
            string regexXG = @"^[A-Z][0-9]{6}\([0-9A]\)$";
            string regexXG2 = @"^[A-Z][0-9]{6}[0-9A]$";
            //澳门身份证1、5、7+6个数字+（一个校验码0~9）
            string regexAM = @"^[157][0-9]{6}\([0-9]\)$";
            string regexAM2 = @"^[157][0-9]{6}[0-9]$";
            //台湾身份证一个英文+6个数字+（一个校验码0~9或A）
            //string regexTW = @"^[A-Z][0-9]{9}";
            var regXG = new Regex(regexXG);
            var regXG2 = new Regex(regexXG2);
            var regAM = new Regex(regexAM);
            var regAM2 = new Regex(regexAM2);
            if (!regXG.IsMatch(idCard) && !regXG2.IsMatch(idCard) && !regAM.IsMatch(idCard) && !regAM2.IsMatch(idCard))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 校验姓名
        public static bool IsValidName(this string name, int idtype)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            Regex regex;
            if (idtype == 1 || idtype == 5 || idtype == 14)
            {
                //个人用户（身份证、港澳来往通行、港澳身份证）汉字开头的2-5位 兼容新疆
                regex = new Regex(@"^[\u4e00-\u9fa5+\·?\u4e00-\u9fa5+]{2,30}$");
                if (!regex.IsMatch(name))
                {
                    return false;
                }
            }
            else if (idtype == 2 || idtype == 9)
            {
                //公司用户（组织机构代码证、统一社会信用）有英文开头的
                if (name.Length < 2 || name.Length > 40)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
