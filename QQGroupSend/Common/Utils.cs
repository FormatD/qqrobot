using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace Format.WebQQ.Common
{
    /// <summary>
    /// 2011-1-5
    /// by hackren
    /// Email:hackren@vip.qq.com
    /// </summary>
    public class Utils
    {
        public static DateTime DataStandardTime()
        {//返回国际标准时间
            //只使用的时间服务器的IP地址，未使用域名
            string[,] 时间服务器 = new string[14, 2];
            int[] 搜索顺序 = new int[] { 3, 2, 4, 8, 9, 6, 11, 5, 10, 0, 1, 7, 12 };
            时间服务器[0, 0] = "time-a.nist.gov";
            时间服务器[0, 1] = "129.6.15.28";
            时间服务器[1, 0] = "time-b.nist.gov";
            时间服务器[1, 1] = "129.6.15.29";
            时间服务器[2, 0] = "time-a.timefreq.bldrdoc.gov";
            时间服务器[2, 1] = "132.163.4.101";
            时间服务器[3, 0] = "time-b.timefreq.bldrdoc.gov";
            时间服务器[3, 1] = "132.163.4.102";
            时间服务器[4, 0] = "time-c.timefreq.bldrdoc.gov";
            时间服务器[4, 1] = "132.163.4.103";
            时间服务器[5, 0] = "utcnist.colorado.edu";
            时间服务器[5, 1] = "128.138.140.44";
            时间服务器[6, 0] = "time.nist.gov";
            时间服务器[6, 1] = "192.43.244.18";
            时间服务器[7, 0] = "time-nw.nist.gov";
            时间服务器[7, 1] = "131.107.1.10";
            时间服务器[8, 0] = "nist1.symmetricom.com";
            时间服务器[8, 1] = "69.25.96.13";
            时间服务器[9, 0] = "nist1-dc.glassey.com";
            时间服务器[9, 1] = "216.200.93.8";
            时间服务器[10, 0] = "nist1-ny.glassey.com";
            时间服务器[10, 1] = "208.184.49.9";
            时间服务器[11, 0] = "nist1-sj.glassey.com";
            时间服务器[11, 1] = "207.126.98.204";
            时间服务器[12, 0] = "nist1.aol-ca.truetime.com";
            时间服务器[12, 1] = "207.200.81.113";
            时间服务器[13, 0] = "nist1.aol-va.truetime.com";
            时间服务器[13, 1] = "64.236.96.53";
            var portNum = 13;
            string hostName;
            byte[] bytes = new byte[1024];
            int bytesRead = 0;
            using (System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient())
            {
                for (int i = 0; i < 13; i++)
                {
                    hostName = 时间服务器[搜索顺序[i], 1];
                    try
                    {
                        client.Connect(hostName, portNum);
                        System.Net.Sockets.NetworkStream ns = client.GetStream();
                        bytesRead = ns.Read(bytes, 0, bytes.Length);
                        client.Close();
                        break;
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            System.DateTime dt = new DateTime();
            string returnString = System.Text.Encoding.ASCII.GetString(bytes, 0, bytesRead);

            string[] s = returnString.Split(new char[] { ' ' });
            dt = System.DateTime.Parse(String.Format("{0} {1}", s[1], s[2]));//得到标准时间
            dt = dt.AddHours(8);//得到北京时间*/
            return dt;
        }

        /// <summary>
        /// 图片加扰码
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static Stream AddPicCode(FileStream pic)
        {
            try
            {
                Image img = Image.FromStream(pic);
                Graphics gc = Graphics.FromImage(img);
                Random r = new Random();
                int x = r.Next(0, img.Width);
                int y = r.Next(0, img.Height);
                gc.DrawString(".", new Font(FontFamily.GenericSerif, 10), new SolidBrush(Color.Red), new PointF(x, y));
                int xx = r.Next(0, img.Width);
                int yy = r.Next(0, img.Height);
                gc.DrawString("hackren制造", new Font(FontFamily.GenericSerif, 10), new SolidBrush(Color.Red), new PointF(xx, yy));

                MemoryStream stream = new MemoryStream();

                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imagedata = null;
                imagedata = stream.GetBuffer();
                stream.Close();

                return new MemoryStream(imagedata);
            }
            catch (Exception)
            {
                throw;
                //return pic;
            }

        }

        /// <summary>
        /// 返回URL编码字符
        /// </summary>
        /// <param name="strIn">需要编码的内容</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string StrConvUrlEncoding(string strIn, string encoding)
        {
            return System.Web.HttpUtility.UrlEncode(strIn, System.Text.Encoding.GetEncoding(encoding));
        }


        public static void WriteLogMessage(string message, string FilePath)
        {
            try
            {
                Console.WriteLine(String.Format("\r\n{0}\r\n", message));
                using (var sw = new StreamWriter(FilePath, true, Encoding.Default))
                {
                    sw.WriteLine(String.Format("{0}\t{1}", DateTime.Now, message));
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// rsa加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="PublicKey"></param>
        /// <returns></returns>
        public static byte[] EncryptData(string content, string PublicKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(content);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            //将公钥导入到RSA对象中，准备加密； 
            rsa.FromXmlString(PublicKey);
            //对数据data进行加密，并返回加密结果； 
            //第二个参数用来选择Padding的格式 
            return rsa.Encrypt(data, false);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="PrivateKey"></param>
        /// <returns></returns>
        public static string DecryptData(byte[] data, string PrivateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            //将私钥导入RSA中，准备解密；  
            rsa.FromXmlString(PrivateKey);
            //对数据进行解密，并返回解密结果；
            return Encoding.UTF8.GetString(rsa.Decrypt(data, false));
        }

        public static string ConvertUnicodeStringToChinese(string unicodeString)
        {
            if (string.IsNullOrEmpty(unicodeString))
                return string.Empty;

            string outStr = unicodeString;

            Regex re = new Regex("\\\\u[0123456789abcdef]{4}", RegexOptions.IgnoreCase);
            MatchCollection mc = re.Matches(unicodeString);
            foreach (Match ma in mc)
            {
                outStr = outStr.Replace(ma.Value, ConverUnicodeStringToChar(ma.Value).ToString());
            }
            return outStr;
        }
        private static char ConverUnicodeStringToChar(string str)
        {
            char outStr = Char.MinValue;
            outStr = (char)int.Parse(str.Remove(0, 2), System.Globalization.NumberStyles.HexNumber);
            return outStr;
        }
        /// <summary> 
        /// md5加密 
        /// </summary> 
        /// <param name="input"></param> 
        /// <returns></returns> 
        public static string GetMD5Hash2(string input)
        {
            byte[] buffer = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }
            return builder.ToString();
        }
        /// <summary> 
        /// 三次md5加密 
        /// </summary> 
        /// <param name="input"></param> 
        /// <returns></returns> 
        public static string GetMD5Hash(string input)
        {
            MD5 md = MD5.Create();
            byte[] buffer = md.ComputeHash(Encoding.Default.GetBytes(input));
            buffer = md.ComputeHash(buffer);
            buffer = md.ComputeHash(buffer);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }
            return builder.ToString();
        }
        ////   <summary>   
        ///   去除HTML标记   
        ///   </summary>   
        ///   <param   name="NoHTML">包括HTML的源码   </param>   
        ///   <returns>已经去除后的文字</returns>   
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            Htmlstring = regex.Replace(Htmlstring, "");
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            //Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim(); 

            return Htmlstring;
        }

        /// <summary>
        /// 返回16位小数
        /// </summary>
        /// <returns></returns>
        public static double getRadomNum()
        {
            Random rd = new Random();
            double result = rd.NextDouble();
            return result;
        }

        /// <summary>
        /// 返回随机字符串
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string getStringByRandom(int num)
        {
            String str = "1234567890qwertyuiopasdfghjklzxcvbnm!@#$%^&*()-=_+.,/?";
            StringBuilder returnStr = new StringBuilder();
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    Random r = new Random();
                    int temp = r.Next(0, str.Length - 1);
                    returnStr.Append(str.Substring(temp, 1));
                }
            }

            return returnStr.ToString();

        }

        /// <summary> 
        /// md5加密 
        /// </summary> 
        /// <param name="str"></param> 
        /// <returns></returns> 
        //public string MD5(String str) 
        //{ 
        //    MD5 md5 = new MD5CryptoServiceProvider(); 
        //    byte[] data = System.Text.Encoding.Default.GetBytes(str); 
        //    byte[] result = md5.ComputeHash(data); 
        //    String ret = ""; 
        //    for (int i = 0; i < result.Length; i++) 
        //        ret += result[i].ToString("x").PadLeft(2, '0'); 
        //    return ret; 
        //}  


        public static string getStringByRegex(string result, string regexStr, string key, int index)
        {
            Regex r = new Regex(regexStr, RegexOptions.IgnoreCase);

            MatchCollection m = r.Matches(result);

            try
            {
                return m[index].Groups[key].Value;
            }
            catch (Exception)
            {
                throw;
                //return "";
            }
        }

        /// <summary>
        /// 序列化保存
        /// </summary>
        public static void save(Object obj, string path)
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        public static Object load(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    FileStream stream = new FileStream(path, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    return bf.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
