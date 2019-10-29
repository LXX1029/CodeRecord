using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
namespace Common
{
    /// <summary>
    /// 校验帮助类
    /// </summary>
    public sealed class VerifyHelper
    {
        /// <summary>
        /// 将IEnumberable 转换成ObservableCollection
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="list">IEnumerable</param>
        /// <returns>ObservableCollection<T></returns>
        public static ObservableCollection<T> ConvertToObservableCollection<T>(IEnumerable<T> list)
        {
            if (list == null || list.Count() == 0)
                return new ObservableCollection<T>();
            ObservableCollection<T> collection = new ObservableCollection<T>();
            foreach (T temp in list)
            {
                collection.Add(temp);
            }
            return collection;
        }

        /// <summary>
        /// 获取字符串中的Ip地址字符串
        /// </summary>
        /// <param name="string">传入的包含Ip的字符串</param>
        /// <returns>string</returns>
        public static string GetIpStr(string @string)
        {
            Regex ipReg = new Regex(@"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))");
            return ipReg.Match(@string).Value;
        }

        /// <summary>
        /// 判断是否为DateTime类型 格式正确返回true：否则返回false
        /// </summary>
        /// <param name="string">格式：HH:mm</param>
        /// <returns>bool</returns>
        public static bool IsDateTime(string @string)
        {
            return Regex.IsMatch(@string, @"(\d{2}):(\d{2})");
        }

        /// <summary>
        /// 判断是否为空或Null或空白字符串  成立返回true：否则返回false
        /// </summary>
        /// <param name="string">字符串</param>
        /// <returns>bool</returns>
        public static bool IsEmptyOrNullOrWhiteSpace(string @string)
        {
            if (string.IsNullOrEmpty(@string) || string.IsNullOrWhiteSpace(@string) || @string.Length == 0)
                return true;
            return false;
        }

        /// <summary>
        /// 判断是否为Int类型  格式正确返回true；否则返回false
        /// </summary>
        /// <param name="string">字符串</param>
        /// <returns>bool</returns>
        public static bool IsInt(string @string) => int.TryParse(@string, out _);

        /// <summary>
        /// 判断是否为Ip格式  格式正确返回true：否则返回false
        /// </summary>
        /// <param name="string">字符串</param>
        /// <returns>bool</returns>
        public static bool IsIp(string @string)
        {
            Regex ipReg = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
            return ipReg.IsMatch(@string);
        }

        /// <summary>
        /// 判断是否为Decimal类型  格式正确返回true;否则返回false
        /// </summary>
        /// <param name="string">字符串</param>
        /// <returns>bool</returns>
        public static bool IsNumberic(string @string) => decimal.TryParse(@string, out _);

        /// <summary>
        /// 替换字符串中的Ip，并返回新的Ip字符串
        /// </summary>
        /// <param name="string">传入的包含Ip字符串</param>
        /// <param name="newIpStr">新的Ip地址</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceIpStr(string @string, string newIpStr)
        {
            Regex ipReg = new Regex(@"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))");
            newIpStr = ipReg.Replace(@string, newIpStr);
            return newIpStr;
        }
    }
}