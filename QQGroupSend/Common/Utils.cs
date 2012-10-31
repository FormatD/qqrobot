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
        {//���ع��ʱ�׼ʱ��
            //ֻʹ�õ�ʱ���������IP��ַ��δʹ������
            string[,] ʱ������� = new string[14, 2];
            int[] ����˳�� = new int[] { 3, 2, 4, 8, 9, 6, 11, 5, 10, 0, 1, 7, 12 };
            ʱ�������[0, 0] = "time-a.nist.gov";
            ʱ�������[0, 1] = "129.6.15.28";
            ʱ�������[1, 0] = "time-b.nist.gov";
            ʱ�������[1, 1] = "129.6.15.29";
            ʱ�������[2, 0] = "time-a.timefreq.bldrdoc.gov";
            ʱ�������[2, 1] = "132.163.4.101";
            ʱ�������[3, 0] = "time-b.timefreq.bldrdoc.gov";
            ʱ�������[3, 1] = "132.163.4.102";
            ʱ�������[4, 0] = "time-c.timefreq.bldrdoc.gov";
            ʱ�������[4, 1] = "132.163.4.103";
            ʱ�������[5, 0] = "utcnist.colorado.edu";
            ʱ�������[5, 1] = "128.138.140.44";
            ʱ�������[6, 0] = "time.nist.gov";
            ʱ�������[6, 1] = "192.43.244.18";
            ʱ�������[7, 0] = "time-nw.nist.gov";
            ʱ�������[7, 1] = "131.107.1.10";
            ʱ�������[8, 0] = "nist1.symmetricom.com";
            ʱ�������[8, 1] = "69.25.96.13";
            ʱ�������[9, 0] = "nist1-dc.glassey.com";
            ʱ�������[9, 1] = "216.200.93.8";
            ʱ�������[10, 0] = "nist1-ny.glassey.com";
            ʱ�������[10, 1] = "208.184.49.9";
            ʱ�������[11, 0] = "nist1-sj.glassey.com";
            ʱ�������[11, 1] = "207.126.98.204";
            ʱ�������[12, 0] = "nist1.aol-ca.truetime.com";
            ʱ�������[12, 1] = "207.200.81.113";
            ʱ�������[13, 0] = "nist1.aol-va.truetime.com";
            ʱ�������[13, 1] = "64.236.96.53";
            var portNum = 13;
            string hostName;
            byte[] bytes = new byte[1024];
            int bytesRead = 0;
            using (System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient())
            {
                for (int i = 0; i < 13; i++)
                {
                    hostName = ʱ�������[����˳��[i], 1];
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
            dt = System.DateTime.Parse(String.Format("{0} {1}", s[1], s[2]));//�õ���׼ʱ��
            dt = dt.AddHours(8);//�õ�����ʱ��*/
            return dt;
        }

        /// <summary>
        /// ͼƬ������
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
                gc.DrawString("hackren����", new Font(FontFamily.GenericSerif, 10), new SolidBrush(Color.Red), new PointF(xx, yy));

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
        /// ����URL�����ַ�
        /// </summary>
        /// <param name="strIn">��Ҫ���������</param>
        /// <param name="encoding">����</param>
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
        /// rsa����
        /// </summary>
        /// <param name="content"></param>
        /// <param name="PublicKey"></param>
        /// <returns></returns>
        public static byte[] EncryptData(string content, string PublicKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(content);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            //����Կ���뵽RSA�����У�׼�����ܣ� 
            rsa.FromXmlString(PublicKey);
            //������data���м��ܣ������ؼ��ܽ���� 
            //�ڶ�����������ѡ��Padding�ĸ�ʽ 
            return rsa.Encrypt(data, false);
        }

        /// <summary>
        /// RSA����
        /// </summary>
        /// <param name="content"></param>
        /// <param name="PrivateKey"></param>
        /// <returns></returns>
        public static string DecryptData(byte[] data, string PrivateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            //��˽Կ����RSA�У�׼�����ܣ�  
            rsa.FromXmlString(PrivateKey);
            //�����ݽ��н��ܣ������ؽ��ܽ����
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
        /// md5���� 
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
        /// ����md5���� 
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
        ///   ȥ��HTML���   
        ///   </summary>   
        ///   <param   name="NoHTML">����HTML��Դ��   </param>   
        ///   <returns>�Ѿ�ȥ���������</returns>   
        public static string NoHTML(string Htmlstring)
        {
            //ɾ���ű�   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //ɾ��HTML   
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
        /// ����16λС��
        /// </summary>
        /// <returns></returns>
        public static double getRadomNum()
        {
            Random rd = new Random();
            double result = rd.NextDouble();
            return result;
        }

        /// <summary>
        /// ��������ַ���
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
        /// md5���� 
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
        /// ���л�����
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
        /// �����л�
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
