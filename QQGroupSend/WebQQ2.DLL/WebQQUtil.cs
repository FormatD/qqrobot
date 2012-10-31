using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;

namespace WebQQ2.DLL
{
    /// <summary>
    /// 2011-1-5
    /// by hackren
    /// Email:hackren@vip.qq.com
    /// </summary>
    public class WebQQUtil
    {
        /// <summary>
        /// 生成ClientId
        /// </summary>
        /// <returns></returns>
        public static string GenerateClientID()
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(0, 99) + "" + GetTime(DateTime.Now) / 1000000;
        }

        /// <summary>
        /// 获取cookie参数
        /// </summary>
        /// <param name="cc">cookie集合</param>
        /// <returns></returns>
        public static string GetGtkByCookieSkey(string key, CookieContainer cc)
        {
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies)
                    {
                        if (c.Name.ToLower() == key)
                        {
                            return c.Value;
                        }
                    }
            }
            return "";
        }

        /// <summary>
        /// 获取cookie参数
        /// </summary>
        /// <param name="cc">cookie集合</param>
        /// <returns></returns>
        public static CookieContainer UpdateCookie(CookieContainer cc,string domain)
        {
            CookieContainer newcookie = new CookieContainer();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies)
                    {
                        c.Domain = domain;
                        c.Path = "/";
                        newcookie.Add(c);
                    }
            }
            return newcookie;
        }

        /// <summary>
        /// 从1970纪元起UTC时间的时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetTime(DateTime dateTime)
        {
            DateTime startDate = new DateTime(1970, 1, 1);
            DateTime endDate = dateTime.ToUniversalTime();
            TimeSpan span = endDate - startDate;
            return (long)(span.TotalMilliseconds + 0.5);
        }

        public static string JsStringToString(string input)
        {
            input = input.Replace(@"\\", @"\");
            string result = Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(input));
            return result;
        }


    }
}
