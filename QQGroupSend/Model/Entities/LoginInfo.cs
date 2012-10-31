using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Format.WebQQ.Common;

namespace Format.WebQQ.WebQQ2.DLL
{
    public class LoginInfo
    {
        string appID = "15000101";

        public string Username { get; set; }

        string password = string.Empty;
        public String PassWord
        {
            get
            {
                if (VerifyCode != "")
                {
                    //return Utils.GetMD5Hash2(Utils.GetMD5Hash(password).ToUpper() + VerifyCode.ToUpper()).ToUpper();
                    return MD5Helper.MD5_QQ_2_Encrypt(Username, password, VerifyCode.ToUpper());
                }
                else
                {
                    return "";
                }
            }
            set
            {
                password = value;
            }
        }

        public string VerifyCode { get; set; }

        public CookieContainer Cookie { get; set; }

        public string Gface_sig { get; set; }

        public string Gface_key { get; set; }

        public string AppID
        {
            get
            {
                return appID;
            }
            set
            {
                appID = value; ;
            }
        }

        public string ClientId { get; set; }

        public string PtWebQQ
        {
            get
            {
                return WebQQUtil.GetGtkByCookieSkey("ptwebqq", Cookie);
            }
        }

        public string Skey
        {
            get
            {
                return WebQQUtil.GetGtkByCookieSkey("skey", Cookie);
            }
        }

        public string VfWebQQ { get; set; }

        public string PSessionid { get; set; }

        public string VerifyCode32 { get; set; }

    }
}
