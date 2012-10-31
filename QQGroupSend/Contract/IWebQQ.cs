using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Format.WebQQ.WebQQ2.DLL;
using Format.WebQQ.Model.Entities;
using Format.WebQQ.Model.Messages;
using Format.WebQQ.Model.Enum;


namespace Format.WebQQ.Contract
{
    public delegate void FriendMessageHandler(string fuin, string message);
    public delegate void QunMessageHandler(GroupMessage groupMessage);
    public delegate void JoinRequestedHandle(JoinGroupRequestMessage message);
    public delegate void MemberJoinedHandle(MemberJoinedMessage message);
    public delegate void MemberLeavedHandle(MemberLeavedMessage message);
    public delegate void AddFriendHandler(string fuin, string message);
    public delegate void OffLineHandle(string userName);

    public interface IWebQQ
    {
        User CurrentUser { get; }

        event FriendMessageHandler OnFriendMessageEvent;
        event AddFriendHandler OnAddFriendEvent;
        event OffLineHandle OnOffLine;

        event JoinRequestedHandle OnJoinRequested;
        event MemberJoinedHandle OnMemberJoined;
        event MemberLeavedHandle OnMemberLeved;
        event QunMessageHandler OnGroupMessageArrivedEvent;

        bool AcceptJoinRequest(Format.WebQQ.Model.Messages.JoinGroupRequestMessage message);
        bool Acitvity(string url);
        string AddFriend(long uin);
        bool AddFriend(string fuin);
        void ChangeStatus(OnlineStatusEnum status);
        void DeleteGroupMmeber(long trueGroupUin, long[] userList);
        void DeleteGroupMmeber(long trueGroupUin, long userUin);
        string GetFaceServer(int qq);
        void GetGroupList();
        Dictionary<long, GroupMember> GetGroupMembers(GroupEntity group);
        long GetOrginalUserID(long userID);
        bool GetUserFriends();
        void FillSingleInfo(Friend friend);
        void FillUserInfo(Friend friend);
        void GetUserLevel(string userID);
        System.IO.Stream GetVerifyCodePicStream();
        bool InitWebQQ();
        void InviteGroupMmeber(string trueGroupUin, string[] userList);
        bool IsNeedVerifyCode();
        UserStatus Login();
        void LogOut();
        bool ModifyMyInfo(string nickName, string homeUrl, string shuoming);


        void RetrieveGroupMember(string trueGroupUin);
        bool sendFriendMessage(long friendUin, string message);
        bool SendGroupMessage(long groupUin, string message, string fontStyle);
        bool SendGroupMessagePic(long groupUin, string qunCode, string imageUrl);
        void SendMessageForAllFriend(string message);
        bool SendTempMessage(string message, string groupUin, string userUin);
        bool SetLongNick(string message);
        void StartPullThread();
        string UpLoadMessageImage(string filePath, string fileName, string fileId);
    }
}
