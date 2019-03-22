using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.Helpers
{
    public static class Extends
    {
        /// <summary>
        /// 将时间转换为时间戳
        /// </summary>
        /// <param name="dt">时间类型对象比如(DateTime.Now)</param>
        /// <returns></returns>
        public static long ConvertToTimeStmap(this DateTime dt)
        {
            return (dt.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        #region 枚举转换
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value">枚举的值</param>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static String ToEnumDescriptionString(this int value, Type enumType)
        {
            NameValueCollection nvc = GetNVCFromEnumValue(enumType);
            return nvc[value.ToString()];
        }
        public static NameValueCollection GetNVCFromEnumValue(Type enumType)
        {
            NameValueCollection nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = "";
                    }
                    nvc.Add(strValue, strText);
                }
            }
            return nvc;
        }
        /// <summary>
        /// 输出MD5加密串
        /// </summary>
        /// <param name="s">当前字符串对象</param>
        /// <returns></returns>       
        public static string ToMd5(this string s, int type = 1)
        {
            var md5Hasher = new MD5CryptoServiceProvider();
            byte[] hash = null;
            if (type == 1)
            {
                hash = md5Hasher.ComputeHash(Encoding.Default.GetBytes(s));
            }
            else
            {
                hash = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(s));
            }


            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        #endregion
    }
}
