using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Json;
using System.IO;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Reactive.Concurrency;

using Format.WebQQ.Common;
using Format.WebQQ.Contract;
using Format.WebQQ.Model.Entities;
using Format.WebQQ.Model.Enum;
using Format.WebQQ.Model.Messages;

namespace Format.WebQQ.WebQQ2.DLL
{
    [Serializable]
    public class WebQQCommon : Format.WebQQ.Contract.IWebQQ
    {

        public class MessageEventArgs<T> : EventArgs
             where T : BaseMessage
        {
            public T Message { get; set; }

            public MessageEventArgs(T message)
            {
                this.Message = message;
            }
        }


        #region event
        public event EventHandler<MessageEventArgs<FriendMessage>> FriendMessageArrived;
        public event EventHandler<MessageEventArgs<GroupMessage>> GroupMessageArrived;
        public event EventHandler<MessageEventArgs<GroupSystemMessage>> GroupSystemMessageArrived;

        public event FriendMessageHandler OnFriendMessageEvent;
        public event AddFriendHandler OnAddFriendEvent;
        public event OffLineHandle OnOffLine;

        public event JoinRequestedHandle OnJoinRequested;
        public event MemberJoinedHandle OnMemberJoined;
        public event MemberLeavedHandle OnMemberLeved;
        public event QunMessageHandler OnGroupMessageArrivedEvent;

        public IObservable<BaseMessage> MessageQueen = null;
        #endregion

        #region const and readonly

        private static readonly string[] AVATAR_SERVER_DOMAINS = new[] { "http://face1.qun.qq.com/", "http://face2.qun.qq.com/", "http://face3.qun.qq.com/", "http://face4.qun.qq.com/", "http://face5.qun.qq.com/", "http://face6.qun.qq.com/", "http://face7.qun.qq.com/", "http://face8.qun.qq.com/", "http://face9.qun.qq.com/", "http://face10.qun.qq.com/" };

        private static readonly string defaultRefUrl = "http://d.web2.qq.com/proxy.html?v=20110331002&callback=2";

        #endregion

        #region fileds
        private int MsgId = 59680000;
        private readonly Random rand = new Random();
        private readonly User currentUser = new User();
        private readonly HttpHelper httpHelper = new HttpHelper();
        private readonly Dictionary<long, long> dicUinTrueUinMap = new Dictionary<long, long>();
        #endregion

        public LoginInfo loginInfo;

        #region ctor
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="loginInfo"></param>
        public WebQQCommon(LoginInfo loginInfo)
        {
            loginInfo.Cookie = new System.Net.CookieContainer();
            this.loginInfo = loginInfo;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="loginInfo"></param>
        public WebQQCommon(LoginInfo loginInfo, System.Net.IWebProxy proxy)
        {
            loginInfo.Cookie = new System.Net.CookieContainer();
            if (proxy != null)
                httpHelper.Proxy = proxy;
            this.loginInfo = loginInfo;
        }
        #endregion

        #region prop
        public User CurrentUser
        {
            get { return currentUser; }
        }
        #endregion

        private int NextMsgId()
        {
            return MsgId + rand.Next(9999);
        }

        public IObservable<BaseMessage> QueryMessages()
        {
            while (true)
            {
                return Observable.Create<BaseMessage>(
                     observer =>
                     {
                         return Scheduler.Immediate.Schedule(
                               _ =>
                               {
                                   while (true)
                                   {
                                       //foreach (var message in GetMessage())
                                       observer.OnNext(GetMessage()[0]);
                                       //Thread.Sleep(1000);
                                   }
                               });
                     });
            }
        }

        public void StartPullThread()
        {
            MessageQueen = QueryMessages();

            MessageQueen.Subscribe(m => Console.WriteLine("{0}::{1}", m.ID, m.ToString()));

            MessageQueen
                //.Where(m => m is FriendMessage)
                //.Cast<FriendMessage>()
                .Subscribe(m =>
                {
                    Console.WriteLine("F:{0}", m.ID);
                    //if (FriendMessageArrived != null)
                    //    FriendMessageArrived(this, new MessageEventArgs<FriendMessage>(m));
                });

            MessageQueen.Where(m => m is GroupMessage)
                .Cast<GroupMessage>()
                .Subscribe(m =>
                {
                    Console.WriteLine("G:{0}", m.ID);
                    if (GroupMessageArrived != null)
                        GroupMessageArrived(this, new MessageEventArgs<GroupMessage>(m));
                });

            //    Thread pullThread = new System.Threading.Thread(() =>
            //    {
            //        while (true)
            //        {
            //            GetMessage();
            //        }
            //    });
            //    pullThread.IsBackground = true;
            //    pullThread.Start();

            //    System.Threading.Thread.Sleep(1000);
        }

        #region 消息处理
        /// <summary>
        /// 获取即时消息
        /// </summary>
        private List<BaseMessage> GetMessage()
        {
            var messages = new List<BaseMessage>();

            string url = string.Format("http://d.web2.qq.com/channel/poll2?clientid={0}&psessionid={1}&t={2}", loginInfo.ClientId, loginInfo.PSessionid, WebQQUtil.GetLongTime(DateTime.Now));
            string result = httpHelper.GetHtml(url, loginInfo.Cookie);
            if (string.IsNullOrWhiteSpace(result))
                messages.Add(new BadMessage("服务器返回空白"));

            Console.WriteLine(result);

            try
            {
                JsonObject message = (JsonObject)JsonValue.Parse(result);
                if (message["retcode"] == 0)
                {
                    foreach (JsonObject item in message["result"])
                    {
                        messages.Add(MessageBuilder.ParseMessage(item));
                    }
                }
                else if (message[""] == 102)
                {
                    messages.Add(new BadMessage(result));
                }
                else if (message["retcode"] == 121)
                {
                    if (OnOffLine != null)
                        OnOffLine(loginInfo.Username);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("----------------");
                Console.WriteLine(result);
                Console.WriteLine("----------------");
                messages.Add(new BadMessage(ex.ToString(), result));
            }
            //todo temp
            if (messages.Count == 0)
            {
                messages.Add(new BaseMessage());
            }

            return messages;
        }

        private void DealWithSystemMessage(JsonObject item)
        {
            JsonValue jsValue = item["value"];
            GroupSystemMessage systemMessage = MessageBuilder.ParseGroupSystemMessage(jsValue);

            switch (systemMessage.Type)
            {
                case GroupMessageType.RequestJoin:
                    if (OnJoinRequested != null)
                        OnJoinRequested(systemMessage as JoinGroupRequestMessage);
                    break;
                case GroupMessageType.Joined:
                    if (OnMemberJoined != null)
                        OnMemberJoined(systemMessage as MemberJoinedMessage);
                    break;
                case GroupMessageType.Leaved:
                    if (OnMemberLeved != null)
                        OnMemberLeved(systemMessage as MemberLeavedMessage);
                    break;
                case GroupMessageType.UnDefined:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 触发加好友事件
        /// </summary>
        /// <param name="jsObject"></param>
        private void InitAddFriendEvent(JsonValue jsObject)
        {
            if (jsObject != null)
            {
                JsonValue item = jsObject["value"];
                if (OnAddFriendEvent != null)
                    OnAddFriendEvent(item["from_uin"].ToString(), item.ContainsKey("msg") == false ? "" : item["msg"].ToString());

                Console.WriteLine(item["from_uin"].ToString() + "------------------" + item["msg"].ToString());
            }
        }

        /// <summary>
        /// 触发收到好友消息事件
        /// </summary>
        /// <param name="jsObject"></param>
        private void InitFriendMessageEvent(JsonValue jsObject)
        {
            if (jsObject != null)
            {
                JsonValue item = jsObject["value"];
                if (OnFriendMessageEvent != null)
                    OnFriendMessageEvent(item["from_uin"].ToString(), item["content"].ToString());
            }
        }


        private void InitSessMessageEvent(JsonObject jsObject)
        {
            if (jsObject != null)
            {
                JsonValue item = jsObject["value"];
                if (OnFriendMessageEvent != null)
                    OnFriendMessageEvent(item["from_uin"].ToString(), item["content"].ToString());
            }
        }


        /// <summary>
        /// 触发收到群消息事件
        /// </summary>
        /// <param name="jsObject"></param>
        private void InitGroupMessageEvent(JsonValue jsObject)
        {
            if (jsObject != null)
            {
                GroupMessage groupMessage = MessageBuilder.ParseGroupMessage(jsObject as JsonObject);

                groupMessage.GroupEntity = currentUser.Groups[groupMessage.GroupUin];

                if (groupMessage.SenderUin > 0)
                    groupMessage.TrueSenderUin = GetOrginalUserID(groupMessage.SenderUin);
                //处理表情
                if (groupMessage.Message.Substring(0, 4) == "[,[\"")
                {
                    groupMessage.Message = "s" + groupMessage.Message;
                    groupMessage.Message = groupMessage.Message.Insert(3, "\",");
                }

                if (groupMessage.Message.Substring(groupMessage.Message.Length - 2) == "]]")
                {
                    groupMessage.Message = groupMessage.Message.Substring(0, groupMessage.Message.Length - 1) + ",\". s";
                }
                groupMessage.Message = groupMessage.Message.Substring(3, groupMessage.Message.Length - 5).Replace("\\", "\\\\").Replace("\"", "\\\"");

                //带图片消息处理
                //if (groupMessage.Message.Contains("\\\"cface\\\""))
                //    groupMessage.Message = "机器人暂不能转发图片！";

                //事件触发
                if (OnGroupMessageArrivedEvent != null)
                    OnGroupMessageArrivedEvent(groupMessage);
            }
        }
        #endregion

        #region IWebQQ

        /// <summary>
        /// 登陆方法，登陆到普通版本的QQ空间，
        /// 登陆需要初始化UserEntity的用户名，密码，验证码。在执行此方法前务必先执行判断是否需要验证码的方法
        /// </summary>
        public UserStatus Login()
        {
            string urlTemplate = "http://ptlogin2.qq.com/login?u={0}&p={1}&verifycode={2}&webqq_type=10&remember_uin=1&login2qq=0&aid=1003903&u1=http%3A%2F%2Fweb.qq.com%2Floginproxy.html%3Flogin2qq%3D0%26webqq_type%3D10&h=1&ptredirect=0&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert&action=5-12-69315&mibao_css=m_webqq&t=1&g=1";
            //string urlTemplate = "http://ptlogin2.qq.com/login?u={0}&p={1}&verifycode={2}&remember_uin=1&aid=1003903&u1=http%3A%2F%2Fweb2.qq.com%2Floginproxy.html%3Fstrong%3Dtrue&h=1&ptredirect=0&ptlang=2052&from_ui=1&pttype=1&dumy=&fp=loginerroralert";
            string url = String.Format(urlTemplate, loginInfo.Username, loginInfo.PassWord, loginInfo.VerifyCode);
            string result = httpHelper.GetHtml(url, loginInfo.Cookie);

            string state = "";

            if (result != "")
            {
                state = result.Substring(8, 1);
            }
            if (state == "0")
            {
                CurrentUser.Status = UserStatus.Logined;
            }
            else if (state == "4")
            {
                CurrentUser.Status = UserStatus.BadVerifyCode;
            }
            else if (state == "7")
            {
                //ptuiCB('7','0','','0','很遗憾，网络连接出现异常，请您稍后再试。(933238208)');

            }
            else
            {
                CurrentUser.Status = UserStatus.LoginFault;
            }

            if (result.Contains("aq.qq.com"))
            {
                //try
                //{
                //    string refUrl = result.Replace("ptuiCB('0','0','", "").Replace("','1','登录成功！');", "");
                //    if (refUrl != "")
                //    {
                //        if (jiechuxianzhi(refUrl))
                //        {
                //            user.Status = UserStatus.Logined;
                //            if (!initWebqq())
                //                user.Status = UserStatus.loginwang;
                //        }
                //    }
                //}
                //catch (Exception)
                //{
                //    user.Status = UserStatus.PasswordWang;
                //}
                CurrentUser.Status = UserStatus.Frezen;
            }

            return CurrentUser.Status;
        }


        /// <summary>
        /// 解除限制
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool Acitvity(String url)
        {
            httpHelper.GetHtml(url, loginInfo.Cookie);
            string result = httpHelper.GetHtml("http://aq.qq.com/cn/services/abnormal/abnormal_direc_release", loginInfo.Cookie);

            if (result.Contains("您的QQ帐号不能直接解除登录限制"))
                return false;
            else
                return true;
        }

        /// <summary>
        /// 判断是否需要验证码
        /// http://ptlogin2.qq.com/check?uin=415516174&appid=15000101&r=0.8178552857537984
        /// uin为QQ号码
        /// appid没弄清干什么的，在我这里不会改变
        /// r为16位随机小数
        ///返回格式 ptui_checkVC('0','!XGB');  ptui_checkVC('1','d3831928a037b4fb5a5336936989e6a607a4edb0ed6da139');
        ///0代表不需要输入验证码，验证码为！XGB
        ///1代表需要验证码，后边会跟48位的密文，应该是个算子，在获取验证码的时候要用到
        /// </summary>
        /// <returns>返回为需要验证码吗，当返回true时需要结合user.VerifyCode32是否为空去最后判断是否需要</returns>
        public bool IsNeedVerifyCode()
        {
            string url = String.Format("http://ptlogin2.qq.com/check?uin={0}&appid={1}&r={2}", loginInfo.Username, loginInfo.AppID, Utils.getRadomNum().ToString());
            string result = httpHelper.GetHtml(url, loginInfo.Cookie);
            if (result.Contains("\'0\'"))
            {
                string vc = "";
                try
                {
                    vc = result.Substring(result.LastIndexOf("\'") - 4, 4);
                }
                catch (Exception)
                {
                    throw;
                }
                loginInfo.VerifyCode = vc;
                return false;
            }
            else if (result.Contains("\'1\'"))
            {
                string vc = "";
                try
                {
                    vc = result.Substring(result.LastIndexOf("\'") - 48, 48);
                }
                catch (Exception)
                {
                    throw;
                }
                loginInfo.VerifyCode32 = vc;
                return true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 返回验证码的图片流
        /// 验证码的获取地址的构成需要48位的算子，需要在判断是否需要验证码时获取
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream GetVerifyCodePicStream()
        {
            System.IO.Stream result = null;
            try
            {
                string url = String.Format("http://captcha.qq.com/getimage?aid={0}&r={1}&uin={2}&vc_type={3}", loginInfo.AppID, Utils.getRadomNum().ToString(), loginInfo.Username, loginInfo.VerifyCode32);
                result = httpHelper.GetStream(url, loginInfo.Cookie);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public bool InitWebQQ()
        {
            loginInfo.ClientId = WebQQUtil.GenerateClientID();
            string url = "http://d.web2.qq.com/channel/login2";
            string postData = string.Format("r=%7B%22status%22%3A%22%22%2C%22ptwebqq%22%3A%22{0}%22%2C%22passwd_sig%22%3A%22%22%2C%22clientid%22%3A%22{1}%22%2C%22psessionid%22%3Anull%7D", loginInfo.PtWebQQ, loginInfo.ClientId);

            string result = httpHelper.GetHtml(url, postData, true, loginInfo.Cookie, url);

            JsonValue json = JsonValue.Parse(result);

            if (json["retcode"] == 0)
            {
                JsonValue jsonResult = json["result"];
                loginInfo.VfWebQQ = Utils.getStringByRegex(result, "(\"vfwebqq\":\")(?<key>.+?)(\",)", "key", 0);
                loginInfo.PSessionid = Utils.getStringByRegex(result, "(psessionid\":\")(?<id>.+?)(\",\")", "id", 0);
                currentUser.BuddyInfo = new Friend { Uin = jsonResult["uin"] };

                FillUserInfo(currentUser.BuddyInfo);
                InitUserInfo();
                return true;
            }
            else
            {
                loginInfo.VfWebQQ = "";
                return false;
            }
        }

        private void InitUserInfo()
        {

        }

        /// <summary>
        /// 获取好友及好友分组
        /// </summary>
        /// <returns></returns>
        public bool GetUserFriends()
        {
            //CurrentUser.Groups = new Dictionary<long, string>();
            CurrentUser.Friends = new Dictionary<long, Friend>();
            string url = "http://web2-b.qq.com/api/get_user_friends";
            string data = string.Format("r=%7B%22vfwebqq%22%3A%22{0}%22%7D", loginInfo.VfWebQQ);
            string result = httpHelper.GetHtml(url, data, true, loginInfo.Cookie, url);

            //Regex group = new Regex("({\"index\":)(?<index>[0-9]{1,2}?)(,\"name\":\")(?<name>.+?)(\"})");
            //foreach (Match item in group.Matches(result))
            //{
            //    CurrentUser.Groups.Add(Convert.ToInt32(item.Groups["index"].Value), Utils.ConvertUnicodeStringToChinese(item.Groups["name"].Value));
            //}

            Regex friend = new Regex("({\"uin\":)(?<uin>[0-9]{5,11}?)(,\"categories\":)(?<id>[0-9]{1,2}?)(})");
            foreach (Match item in friend.Matches(result))
            {
                Friend f = new Friend();
                f.OrginalUin = long.Parse(item.Groups["uin"].Value);
                f.GroupId = int.Parse(item.Groups["id"].Value);
                CurrentUser.Friends.Add(f.OrginalUin, f);
            }

            Regex info = new Regex("({\"uin\":)(?<uin>[0-9]{5,11}?)(,\"nick\":\")(?<nick>.+?)(\",\"face\":)(?<face>.+?)(,\"flag\":)(?<flag>.+?)(})");
            foreach (Match item in info.Matches(result))
            {
                long uin = 0;
                if (long.TryParse(item.Groups["uin"].Value, out uin) && CurrentUser.Friends.ContainsKey(uin))
                {
                    CurrentUser.Friends[uin].Name = Utils.ConvertUnicodeStringToChinese(item.Groups["nick"].Value);
                    CurrentUser.Friends[uin].FaceImg = item.Groups["face"].Value;
                }
            }
            return false;
        }

        /// <summary>
        /// 给好友发送消息
        /// </summary>
        /// <param name="friendUin"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool sendFriendMessage(long friendUin, string message)
        {
            string temp = @"r=%7B%22to%22%3A{0}%2C%22face%22%3A576%2C%22content%22%3A%22%5B%5C%22{1}%5C%22%2C%5B%5C%22font%5C%22%2C%7B%5C%22name%5C%22%3A%5C%22%E5%AE%8B%E4%BD%93%5C%22%2C%5C%22size%5C%22%3A%5C%2210%5C%22%2C%5C%22style%5C%22%3A%5B0%2C0%2C0%5D%2C%5C%22color%5C%22%3A%5C%22000000%5C%22%7D%5D%5D%22%2C%22msg_id%22%3A{2}%2C%22clientid%22%3A%22{3}%22%2C%22psessionid%22%3A%22{4}%22%7D&clientid={3}&psessionid={4}";
            string url = "http://d.web2.qq.com/channel/send_buddy_msg2";
            string postData = string.Format(temp, friendUin, Utils.StrConvUrlEncoding(message, "utf-8"), MsgId++, loginInfo.ClientId, loginInfo.PSessionid);
            string result = httpHelper.GetHtml(url, postData, true, loginInfo.Cookie, "http://d.web2.qq.com/proxy.html?v=20110331002&callback=2");
            if (result.Contains("retcode\":0"))
                return true;
            else
                return false;
        }

        public string GetTempSessionSign(string groupUin, string toUin)
        {
            //GET http://d.web2.qq.com/channel/get_c2cmsg_sig2?id=1784818685&to_uin=1779839164&service_type=0&clientid=90633816&psessionid=8368046764001e636f6e6e7365727665725f77656271714031302e3132382e36362e31313200004032000002dd026e0400e724f5636d0000000a4072663958356b516d6b6d000000288019843797934e4f5ea223c6979850121b289650a1c32a7a9520e6062a7fe5abaf4ec6f83eacf13a&t=1317049220575 
            string url = "http://d.web2.qq.com/channel/get_c2cmsg_sig2?id={0}&to_uin={1}&service_type=0&clientid={2}&psessionid={3}&t={4}";

            url = string.Format(url, groupUin, toUin, loginInfo.ClientId, loginInfo.PSessionid, WebQQUtil.GetLongTime(DateTime.Now));
            string result = httpHelper.GetHtml(url, "", false, loginInfo.Cookie, "");
            //{"retcode":0,"result":{"type":0,"value":"f3f18cc5e8e50bcd05caff30634c546b211bf2850a240a7f7f9ac975d031269ad6d183467ef29f144a68b43e0c88d268","flags":{"text":1,"pic":1,"file":1,"audio":1,"video":1}}}
            Console.WriteLine(result);

            JsonObject jsObject = (JsonObject)JsonValue.Parse(result);
            int retcode = jsObject["retcode"];
            if (retcode == 0)
            {
                return jsObject["result"]["value"].ToString().Trim('\"');
            }
            else
                return string.Empty;
        }

        public bool SendTempMessage(string message, string groupUin, string userUin)
        {
            string groupSig = GetTempSessionSign(groupUin, userUin);
            return SendTempMessageWithSig(message, groupSig, userUin);
        }

        private bool SendTempMessageWithSig(string message, string groupSig, string uin)
        {
            message = System.Web.HttpUtility.UrlEncode(message);
            string url = "http://d.web2.qq.com/channel/send_sess_msg2";
            string postData = "r=%7B%22to%22%3A{0}%2C%22group_sig%22%3A%22{1}%22%2C%22face%22%3A576%2C%22content%22%3A%22%5B%5C%22{2}%5C%22%2C%5B%5C%22font%5C%22%2C%7B%5C%22name%5C%22%3A%5C%22%E5%AE%8B%E4%BD%93%5C%22%2C%5C%22size%5C%22%3A%5C%2210%5C%22%2C%5C%22style%5C%22%3A%5B0%2C0%2C0%5D%2C%5C%22color%5C%22%3A%5C%22000000%5C%22%7D%5D%5D%22%2C%22msg_id%22%3A{3}%2C%22service_type%22%3A0%2C%22clientid%22%3A%22{4}%22%2C%22psessionid%22%3A%22{5}%22%7D&clientid={4}&psessionid={5}";
            postData = string.Format(postData, uin, groupSig, message, MsgId, loginInfo.ClientId, loginInfo.PSessionid);

            string result = httpHelper.GetHtml(url, postData, true);
            if (string.IsNullOrEmpty(result))
            {
                //TODO Log bad return
                return false;
            }
            JsonObject jsObject = (JsonObject)JsonValue.Parse(result);
            Console.WriteLine(result);
            return jsObject["retcode"] == 0;
        }

        public void RetrieveGroupMember(string trueGroupUin)
        {
            string url = "http://qun.qq.com/air/?w=a&c=json&a=members&g={0}&ty=0&_={1}";
            string refUrl = "http://qun.qq.com/air/{0}/admin#{0}/admin/member";
            refUrl = string.Format(refUrl, trueGroupUin);
            url = string.Format(url, trueGroupUin, WebQQUtil.GetLongTime(DateTime.Now));

            string result = httpHelper.GetHtml(url, string.Empty, false, loginInfo.Cookie, refUrl);
            Console.WriteLine(result);
        }

        public void DeleteGroupMmeber(long trueGroupUin, long userUin)
        {
            DeleteGroupMmeber(trueGroupUin, new long[] { userUin });
        }

        public void DeleteGroupMmeber(long trueGroupUin, long[] userList)
        {
            string url = "http://qun.qq.com/air/?w=a&c=admin&a=member&g={0}";
            url = string.Format(url, trueGroupUin);
            string refUrl = "http://qun.qq.com/air/{0}/admin#{0}/admin/member";
            refUrl = string.Format(refUrl, trueGroupUin);
            string postData = "ul={0}%2C&m=bdu";
            postData = string.Format(postData, System.Web.HttpUtility.UrlEncode(string.Join(",", userList)));
            string result = httpHelper.GetHtml(url, postData, true, loginInfo.Cookie, refUrl);
        }

        public void InviteGroupMmeber(string trueGroupUin, string[] userList)
        {
            string url = "http://qun.qq.com/air/?w=a&c=admin&a=member&g={0}";
            url = string.Format(url, trueGroupUin);
            string refUrl = "http://qun.qq.com/air/{0}/admin#{0}/admin/member";
            refUrl = string.Format(refUrl, trueGroupUin);
            string postData = "ul={0}%2C&m=bau";
            postData = string.Format(postData, System.Web.HttpUtility.UrlEncode(string.Join(",", userList)));
            string result = httpHelper.GetHtml(url, postData, true, loginInfo.Cookie, refUrl);
            Console.WriteLine("Invite User");
            Console.WriteLine(result);
        }

        /// <summary>
        /// 给所有好友发送消息，间隔10秒
        /// </summary>
        /// <param name="message"></param>
        public void SendMessageForAllFriend(string message)
        {
            if (CurrentUser.Friends.Count <= 0)
                GetUserFriends();
            foreach (long uin in CurrentUser.Friends.Keys)
            {
                sendFriendMessage(uin, message);
                System.Threading.Thread.Sleep(10000);
            }
        }

        /// <summary>
        /// 修改个性签名
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool SetLongNick(string message)
        {
            string url = "http://web2-b.qq.com/api/set_long_nick";
            string postDate = string.Format("r=%7B%22nlk%22%3A%22{0}%22%2C%22vfwebqq%22%3A%22{1}%22%7D", Utils.StrConvUrlEncoding(message, "utf-8"), loginInfo.VfWebQQ);

            string result = httpHelper.GetHtml(url, postDate, true, loginInfo.Cookie, url);
            if (result.Contains("retcode\":0"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 修改资料（昵称，网址，个性说明）
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="homeUrl"></param>
        /// <param name="shuoming"></param>
        /// <returns></returns>
        public bool ModifyMyInfo(string nickName, string homeUrl, string shuoming)
        {
            string url = "http://web2-b.qq.com/api/modify_my_details";
            string postDate = string.Format("r=%7B%22nick%22%3A%22{0}%22%2C%22gender%22%3A%22female"
                + "%22%2C%22shengxiao%22%3A%221%22%2C%22constel%22%3A%221%22%2C%22blood%22%3A%221%22%2C%22"
                + "birthyear%22%3A%221989%22%2C%22birthmonth%22%3A%2211%22%2C%22birthday%22%3A%221%22%2C%22"
                + "phone%22%3A%2201022222222%22%2C%22mobile%22%3A%2213111111122%22%2C%22email%22%3A%22abcde%40qq.com%22%2C%22"
                + "occupation%22%3A%22hack%22%2C%22college%22%3A%22beida%22%2C%22homepage%22%3A%22{1}%22%2C%22personal%22%3A%22"
                + "{2}%22%2C%22vfwebqq%22%3A%22{3}%22%7D", Utils.StrConvUrlEncoding(nickName, "utf-8"), Utils.StrConvUrlEncoding(homeUrl, "utf-8"), Utils.StrConvUrlEncoding(shuoming, "utf-8"), loginInfo.VfWebQQ);

            string result = httpHelper.GetHtml(url, postDate, true, loginInfo.Cookie, url);
            if (result.Contains("retcode\":0"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void LogOut()
        {
            string url = string.Format("http://d.web2.qq.com/channel/logout2?clientid={0}&psessionid={1}&t={2}", loginInfo.ClientId, loginInfo.PSessionid, WebQQUtil.GetLongTime(DateTime.Now));

            string result = httpHelper.GetHtml(url, loginInfo.Cookie);

        }

        /// <summary>
        /// 隐身
        /// </summary>
        public void ChangeStatus(OnlineStatusEnum status)
        {
            string state = "online";
            switch (status)
            {
                case OnlineStatusEnum.Online:
                    state = "online";
                    break;
                case OnlineStatusEnum.Away:
                    state = "away";
                    break;
                case OnlineStatusEnum.Hidden:
                    state = "hidden";
                    break;
                case OnlineStatusEnum.Offline:
                    state = "offine";
                    break;
                default:
                    state = "online";
                    break;
            }
            string url = string.Format("http://web2-b.qq.com/channel/change_status?newstatus={0}&clientid={1}&psessionid={2}&t={3}", state, loginInfo.ClientId, loginInfo.PSessionid, WebQQUtil.GetLongTime(DateTime.Now));
            string result = httpHelper.GetHtml(url, loginInfo.Cookie);
        }

        /// <summary>
        /// 获取头像服务器
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public string GetFaceServer(int qq)
        {
            return AVATAR_SERVER_DOMAINS[qq % 10];
        }
        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public System.IO.Stream GetUserAvatar(string qq)
        {
            return httpHelper.GetStream(GetFaceServer(Convert.ToInt32(qq)) + "cgi/svr/face/getface?cache=0&type=1&fid=0&uin=" + qq, loginInfo.Cookie);
        }

        public string AddFriend(long uin)
        {
            //POST http://id.qq.com/cgi-bin/friends_add HTTP/1.1
            //Accept: */*
            //Referer: http://id.qq.com/af/af.html?appid=10021&lang=2052&t=1351488480195&u=553073466&ut=undefined
            //Accept-Language: zh-cn
            //Accept-Encoding: gzip, deflate
            //User-Agent: Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)
            //Host: id.qq.com
            //Content-Length: 87
            //DNT: 1
            //Connection: Keep-Alive
            //Pragma: no-cache
            //Cookie: pt2gguin=o0190993691; RK=K6hj57c2mI; pgv_pvid=1951158080; ptui_loginuin=190993691; nickxs=%25u5BF9%25u65B9; uin=o0190993691; skey=@7MTCvS0kv; ptisp=cnc; ldw=7b823666a1b7e997b06365a863c5d8af1d497648667d2716

            //ldw=7b823666a1b7e997b06365a863c5d8af1d497648667d2716&t=1&u=553073466&0.7799491813584485
            string refer = "http://id.qq.com/af/af.html?appid=10021&lang=2052&t=1351488480195&u=553073466&ut=undefined";
            //httpHelper.GetHtml(refer, loginInfo.Cookie);

            // get ldw
            var xx = httpHelper.GetHtml("http://id.qq.com/cgi-bin/get_base_key?r=0.8045852194541678", loginInfo.Cookie);
            var cookie = loginInfo.Cookie.GetCookies(new Uri(refer));
            var ldw = cookie["ldw"];

            return httpHelper.GetHtml("http://id.qq.com/cgi-bin/friends_add", string.Format("t=1&u={0}&{1}&0.7799491813584485", uin, ldw), true, loginInfo.Cookie, refer);
        }


        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="fuin">好友号码</param>
        /// <returns></returns>
        public bool AddFriend(string fuin)
        {
            string url = "http://web2-b.qq.com/api/allow_and_add";
            string postData = string.Format("r=%7B%22tuin%22%3A{0}%2C%22gid%22%3A0%2C%22mname%22%3A%22%22%2C%22vfwebqq%22%3A%22{1}%22%7D", fuin, loginInfo.VfWebQQ);

            string result = httpHelper.GetHtml(url, postData, true, loginInfo.Cookie, defaultRefUrl);

            if (result.Contains("retcode\":0"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取群列表及信息
        /// </summary>
        public void GetGroupList()
        {
            string url = "http://s.web2.qq.com/api/get_group_name_list_mask2";
            string postdata = string.Format("r=%7B%22vfwebqq%22%3A%22{0}%22%7D", loginInfo.VfWebQQ);

            string result = httpHelper.GetHtml(url, postdata, true, loginInfo.Cookie, defaultRefUrl);
            CurrentUser.Groups = new Dictionary<long, GroupEntity>();
            try
            {
                JsonObject message = (JsonObject)JsonValue.Parse(result);
                if (message["retcode"].ToString() == "0")
                {
                    JsonObject minfos = (JsonObject)JsonValue.Parse(message["result"].ToString());

                    foreach (JsonObject item in minfos["gnamelist"])
                    {
                        try
                        {
                            GroupEntity qun = new GroupEntity();
                            qun.GroupId = item["gid"];
                            qun.GroupCode = item["code"];
                            qun.GroupName = ((string)item["name"]).Replace("\"", "");
                            qun.Members = GetGroupMembers(qun);
                            CurrentUser.Groups.Add(qun.GroupId, qun);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public Dictionary<long, GroupMember> GetGroupMembers(GroupEntity groupEntity)
        {
            //Verify group entity
            string url = string.Format("http://s.web2.qq.com/api/get_group_info_ext2?gcode={0}&vfwebqq={1}&t={2}", groupEntity.GroupCode, loginInfo.VfWebQQ, WebQQUtil.GetLongTime(DateTime.Now));

            string result = httpHelper.GetHtml(url, loginInfo.Cookie);

            Dictionary<long, GroupMember> dicMember = new Dictionary<long, GroupMember>();

            JsonObject message = (JsonObject)JsonValue.Parse(result);
            if (message["retcode"] == 0)
            {
                JsonObject jsonResult = (JsonObject)JsonValue.Parse(message["result"].ToString());

                //群详细信息
                JsonValue groupInfo = jsonResult["ginfo"];
                groupEntity.Memo = groupInfo["memo"];
                groupEntity.Class = groupInfo["class"];
                groupEntity.CreateTime = WebQQUtil.GetLocalTimeByLong((long)groupInfo["createtime"]);
                groupEntity.Level = groupInfo["level"];
                groupEntity.OwnerUin = groupInfo["owner"];
                groupEntity.Option = groupInfo["option"];

                //成员MFlag
                foreach (JsonObject item in groupInfo["members"])
                {
                    GroupMember member = new GroupMember()
                    {
                        Uin = item["muin"],
                        MFlag = item["mflag"]
                    };
                    dicMember.Add(member.Uin, member);
                }

                //成员详细信息
                foreach (JsonObject item in jsonResult["minfo"])
                {
                    long uin = item["uin"];
                    if (!dicMember.ContainsKey(uin))
                    {
                        Console.WriteLine("uin lost didn't in ‘members’");
                        dicMember[uin] = new GroupMember();
                        dicMember[uin].Uin = uin;
                    }
                    dicMember[uin].Card = item["nick"];
                }

                //昵称
                if (jsonResult.ContainsKey("cards"))
                {
                    foreach (JsonObject item in jsonResult["cards"])
                    {
                        long uin = item["muin"];
                        if (!dicMember.ContainsKey(uin))
                        {
                            Console.WriteLine("uin lost didn't in ‘members’");
                            dicMember[uin] = new GroupMember();
                            dicMember[uin].Uin = uin;
                        }
                        dicMember[uin].NickName = item["card"];
                    }
                }

                //type
                foreach (JsonObject item in jsonResult["stats"])
                {
                    long uin = item["uin"];
                    if (!dicMember.ContainsKey(uin))
                    {
                        Console.WriteLine("uin lost didn't in ‘members’");
                        dicMember[uin] = new GroupMember();
                        dicMember[uin].Uin = uin;
                    }
                    dicMember[uin].State = item["stat"];
                }
            }

            return dicMember;
        }

        public bool AcceptJoinRequest(JoinGroupRequestMessage message)
        {
            ///channel/op_group_join_req?group_uin=3279007732
            //&req_uin=2561740128
            //&msg=
            //&op_type=2
            //&clientid=34699393
            //&psessionid=8368046764001e636f6e6e7365727665725f77656271714031302e3132382e36362e31313200003cc70000047c026e0400ef19c5126d0000000a40434f6b497931397a586d000000285c0300828d708b00d5ae42c61856e430a5433ff19bd095a4ed33c52e6afd1743cc3d109a39a70d1f&t=1311171729109
            string urlFormat = "http://d.web2.qq.com/channel/op_group_join_req?group_uin={0}&req_uin={1}&msg=&op_type=2&clientid={2}&psessionid={3}&t={4}";
            string url = string.Format(urlFormat, message.GroupUin, message.MemberUin, loginInfo.ClientId, loginInfo.PSessionid, WebQQUtil.GetLongTime(DateTime.Now));
            string result = httpHelper.GetHtml(url, loginInfo.Cookie);
            return result.Contains("retcode\":0");
        }

        /// <summary>
        /// 发送群消息
        /// </summary>
        /// <param name="groupUin"></param>
        /// <param name="message"></param>
        /// <param name="fontStyle"></param>
        /// <returns></returns>
        public bool SendGroupMessage(long groupUin, string message, string fontStyle)
        {
            //bool isOk = false;
            //string msg;
            //while (true)
            //{
            //    if (message.Length > 19)
            //    {
            //        msg = new string(message.ToCharArray(0, 19));
            //        message = message.Replace(msg, "");
            //    }
            //    else if (message.Length > 0 && message.Length <= 19)
            //    {
            //        msg = message;
            //        message = "";
            //    }else
            //    {
            //        break;
            //    }
            //    isOk = SendGroupMsg(qunCode, msg, fontStyle);
            //   // System.Threading.Thread.Sleep(2000);
            //}
            //return isOk;
            return SendGroupMsg(groupUin, message, fontStyle);
        }

        private bool SendGroupMsg(long groupUin, string message, string fontStyle)
        {
            string temp = @"r=%7B%22group_uin%22%3A{0}%2C%22content%22%3A%22%5B%5C%22{1}%5C%22%2C%5B%5C%22font%5C%22%2C%7B%5C%22name%5C%22%3A%5C%22%E5%AE%8B%E4%BD%93%5C%22%2C%5C%22size%5C%22%3A%5C%2210%5C%22%2C%5C%22style%5C%22%3A%5B0%2C0%2C0%5D%2C%5C%22color%5C%22%3A%5C%22000000%5C%22%7D%5D%5D%22%2C%22msg_id%22%3A{2}%2C%22clientid%22%3A%22{3}%22%2C%22psessionid%22%3A%22{4}%22%7D&clientid={3}&psessionid={4}";

            message = message.Replace("\\", "\\\\").Replace("\"", "\\\\\"").Replace("\r", "\\\\r").Replace("\n", "\\\\n");
            string url = "http://d.web2.qq.com/channel/send_qun_msg2";
            string postData = string.Format(temp, groupUin, Utils.StrConvUrlEncoding(message, "utf-8"), NextMsgId(), loginInfo.ClientId, loginInfo.PSessionid);

            string result = httpHelper.GetHtml(url, postData, true, loginInfo.Cookie, defaultRefUrl);
            Console.WriteLine(result);
            if (result.Contains("retcode\":0"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 发送群消息--可带图片
        /// </summary>
        /// <param name="qunCode"></param>
        /// <param name="message"></param>
        /// <param name="fontStyle"></param>
        /// <returns></returns>
        public bool SendGroupMessagePic(long groupUin, string qunCode, string imageUrl)
        {
            string url = "http://d.web2.qq.com/channel/send_group_msg2";
            string postData = string.Format("r=%7B%22group_uin%22%3A{0}%2C%22group_code%22%3A{1}%2C%22key%22%3A%22{2}%22%2C%22sig%22%3A%22{3}%22%2C%22content%22%3A%22%5B%5B%5C%22cface%5C%22%2C%5C%22group%5C%22%2C%5C%22{4}%5C%22%5D%2C%5C%22%5C%22%2C%5B%5C%22font%5C%22%2C%7B%5C%22name%5C%22%3A%5C%22%E5%AE%8B%E4%BD%93%5C%22%2C%5C%22size%5C%22%3A%5C%2210%5C%22%2C%5C%22style%5C%22%3A%5B0%2C0%2C0%5D%2C%5C%22color%5C%22%3A%5C%22000000%5C%22%7D%5D%5D%22%2C%22clientid%22%3A%22{5}%22%2C%22psessionid%22%3A%22{6}%22%7D"
                , groupUin, qunCode, loginInfo.Gface_key, loginInfo.Gface_sig, imageUrl, loginInfo.ClientId, loginInfo.PSessionid);
            string result = httpHelper.GetHtml(url, postData, true, loginInfo.Cookie, defaultRefUrl);
            if (result.Contains("retcode\":0"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 发送图片第一步-上传图片
        /// </summary>
        /// <param name="filePath">图片地址</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public string UpLoadMessageImage(string filePath, string fileName, string fileId)
        {
            string returnValue = "";

            string url = string.Format("http://up.web2.qq.com/cgi-bin/cface_upload?time={0}", WebQQUtil.GetLongTime(DateTime.Now));

            // 要上传的文件 
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            //时间戳 
            string strBoundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");

            //请求头部信息 
            StringBuilder sb = new StringBuilder();
            sb.Append("\r\n\r\n" + strBoundary + "\r\n");
            sb.Append("Content-Disposition: form-data; name=\"fileid\"\r\n\r\n");
            sb.Append(fileId + "\r\n");
            sb.Append(strBoundary + "--");

            StringBuilder ss = new StringBuilder();
            ss.Append(strBoundary + "\r\n");
            ss.Append("Content-Disposition: form-data; name=\"from\"\r\n\r\n");
            ss.Append("control\r\n");
            ss.Append(strBoundary + "\r\n");
            ss.Append("Content-Disposition: form-data; name=\"f\"\r\n\r\n");
            ss.Append("EQQ.Model.ChatMsg.callbackSendPicGroup\r\n");
            ss.Append(strBoundary + "\r\n");
            ss.Append("Content-Disposition: form-data; name=\"custom_face\"; filename=\"" + fileName + "\"\r\n");
            ss.Append("Content-Type: image/jpeg \r\n\r\n");
            //ss.Append(strBoundary + "\r\n");
            //ss.Append("Content-Disposition: form-data; name=\"widthlimit\"\r\n\r\n");
            //ss.Append("540\r\n");
            //ss.Append(strBoundary + "\r\n");
            //ss.Append("Content-Disposition: form-data; name=\"heightlimit\"\r\n\r\n");
            //ss.Append("440\r\n");
            //ss.Append(strBoundary + "\r\n");
            //ss.Append("Content-Disposition: form-data; name=\"scale\"\r\n\r\n");
            //ss.Append("true\r\n");
            //ss.Append(strBoundary + "--");

            byte[] boundaryBytes = Encoding.UTF8.GetBytes(sb.ToString());
            string strPostHeader = ss.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);
            // 根据uri创建HttpWebRequest对象 
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
            httpReq.Method = "POST";
            //对发送的数据不使用缓存 
            httpReq.AllowWriteStreamBuffering = false;
            httpReq.CookieContainer = loginInfo.Cookie;
            //设置获得响应的超时时间（300秒） 
            httpReq.Timeout = 300000;
            httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary.Substring(2);
            long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;
            long fileLength = fs.Length;
            httpReq.ContentLength = length;
            httpReq.Referer = "http://web2.qq.com/";
            try
            {
                //每次上传4k 
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];
                //已上传的字节数 
                long offset = 0;
                //开始上传时间 
                DateTime startTime = DateTime.Now;
                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();
                //发送请求头部消息 
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    TimeSpan span = DateTime.Now - startTime;
                    double second = span.TotalSeconds;
                    size = r.Read(buffer, 0, bufferLength);
                }
                //添加尾部的时间戳 
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();
                //获取服务器端的响应 
                WebResponse webRespon = httpReq.GetResponse();
                Stream s = webRespon.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                //读取服务器端返回的消息 
                String sReturnString = sr.ReadToEnd();
                s.Close();
                sr.Close();

                returnValue = Utils.getStringByRegex(sReturnString.Replace(".", "@").Replace(":", "@").Replace("'", "@"), "(@msg@@@)(?<url>.+?)(@jPg)", "url", 0).Replace("@", ".");
                //httpHelper.GetHtml("http://pinghot.qq.com/pingd?dm=web2.qq.com.hot&url=web2.qq.com&tt=WebQQ2.0%20-%20%u817E%u8BAF%u5B98%u65B9%u4E3A%u60A8%u6253%u9020%u7684%u4E00%u7AD9%u5F0F%u7F51%u7EDC%u670D%u52A1&hottag=web2qq.groupmask.toolbar.Insertimage&hotx=9999&hoty=9999&rand=89119", user.Cookie);
                // httpHelper.GetHtml("http://web2.qq.com/cgi-bin/webqq_app/?cmd=2&bd=" + returnValue,user.Cookie);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                fs.Close();
                r.Close();
            }
            return returnValue + ".jPg";
        }

        /// <summary>
        /// 获取sig
        /// </summary>
        public void InitUserSig()
        {
            string url = string.Format("http://d.web2.qq.com/channel/get_gface_sig2?clientid={0}&psessionid={1}&t={2}&vfwebqq={3}", loginInfo.ClientId, loginInfo.PSessionid, WebQQUtil.GetLongTime(DateTime.Now), loginInfo.VfWebQQ);
            string result = httpHelper.GetHtml(url, loginInfo.Cookie);
            try
            {
                JsonObject jsobj = (JsonObject)JsonValue.Parse(result);

                if (jsobj["retcode"].ToString() == "0")
                {
                    JsonObject minfos = (JsonObject)JsonValue.Parse(jsobj["result"].ToString());
                    loginInfo.Gface_sig = minfos["gface_sig"].ToString().Replace("\"", "");
                    loginInfo.Gface_key = minfos["gface_key"].ToString().Replace("\"", "");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //TODO:Log it
            }
        }

        /// <summary>
        /// 获取等级
        /// </summary>
        /// <param name="userID"></param>
        public void GetUserLevel(string userID)
        {
            string url = "http://s.web2.qq.com/api/get_qq_level2?tuin={0}&vfwebqq={1}&t={2}";
            //返回{"retcode":0,"result":{"level":42,"days":2008,"hours":17850,"remainDays":13,"tuin":4041768801}}

        }



        /// <summary>
        /// 获取用户的详细信息
        /// </summary>
        /// <param name="buddy"></param>
        /// <returns></returns>
        public void FillSingleInfo(Friend buddy)
        {
            string url = string.Format("http://web2-b.qq.com/api/get_single_info?tuin={0}&t={1}", buddy.Uin,
                                       WebQQUtil.GetLongTime(DateTime.Now));

            string html = httpHelper.GetHtml(url, loginInfo.Cookie);
            //_logger.Debug("---------\r\nget请求url:{0} html:{1}", url, html);

            var json = (JsonObject)JsonValue.Parse(html);
            int retcode = json["retcode"];
            JsonValue jsonValue = json["result"];
            if (retcode == 0)
            {
                EntityBuilder.ParseFriendInfo(buddy, json);
            }
        }

        public void FillUserInfo(Friend friend)
        {
            //GET http://s.web2.qq.com/api/get_friend_info2?tuin=4041768801&verifysession=&code=&vfwebqq=0943e0095a64f1bf6862d831ed5ea3751df2505e62a218ecbe64ba1fae35ee5a44d7b337bac76569&t=1316836451435 HTTP/1.1
            string url = "http://s.web2.qq.com/api/get_friend_info2?tuin={0}&verifysession=&code=&vfwebqq={1}&t={2}";

            string result = httpHelper.GetHtml(
                string.Format(url, friend.Uin, loginInfo.VfWebQQ, WebQQUtil.GetLongTime(DateTime.Now)),
                loginInfo.Cookie);

            //返回{"retcode":0,"result":{"face":105,"birthday":{"month":3,"year":1984,"day":8},"occupation":"提磗，抹墻，扛水泥","phone":"‮","allow":1,"college":"西華大學","reg_time":1072570855,"uin":4041768801,"constel":2,"blood":3,"homepage":"http://184.82.244.230/","stat":20,"vip_info":0,"country":"中国","city":"成都","personal":"gZ2YX8jP7eP7 http://stackoverflow.com/questions/158706/how-to-properly-clean-up-excel-interop-objects-in-c","nick":"Format","shengxiao":5,"email":"qian_deng@163.com","province":"四川","gender":"male","mobile":"134********"}}
            JsonValue json = JsonValue.Parse(result);
            int retcode = json["retcode"];
            JsonValue jsonValue = json["result"];

            if (retcode == 0)
            {
                EntityBuilder.ParseFriendInfo(friend, jsonValue);
            }
        }

        public long GetOrginalUserID(long uin)
        {
            //GET http://s.web2.qq.com/api/get_friend_uin2?tuin=4041768801&verifysession=&type=1&code=&vfwebqq=0943e0095a64f1bf6862d831ed5ea3751df2505e62a218ecbe64ba1fae35ee5a44d7b337bac76569&t=1316837463860 HTTP/1.1
            //返回{"retcode":0,"result":{"account":314907119,"uin":4041768801}}

            string url = "http://s.web2.qq.com/api/get_friend_uin2?tuin={0}&verifysession=&type=1&code=&vfwebqq={1}&t={2}";

            if (dicUinTrueUinMap.ContainsKey(uin))
            {
                return dicUinTrueUinMap[uin];
            }
            else
            {
                string result = httpHelper.GetHtml(
                    string.Format(url, uin, loginInfo.VfWebQQ, WebQQUtil.GetLongTime(DateTime.Now)),
                    loginInfo.Cookie
                    );
                JsonObject jsonObject = (JsonObject)JsonValue.Parse(result);
                if (jsonObject["retcode"] == 0)
                {
                    long trueUin = ((JsonObject)(jsonObject["result"]))["account"];
                    dicUinTrueUinMap.Add(uin, trueUin);
                    return trueUin;
                }
                else
                {
                    return -1;
                }
            }
        }
        #endregion




    }
}
