using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using Format.WebQQ.Common;

namespace Format.WebQQ.Model.Messages
{
    public class MessageBuilder
    {
        public static BaseMessage ParseMessage(JsonObject messageObject)
        {
            BaseMessage message = null;
            string type = messageObject["poll_type"].ToString().Replace("\"", "");

            switch (type)
            {
                case "message":
                    message = ParseFriendMessage(messageObject);
                    break;
                case "sess_message":
                    break;
                case "system_message":
                    break;
                case "group_message":
                    message = ParseGroupMessage(messageObject);
                    break;
                case "sys_g_msg":
                    message = ParseGroupSystemMessage(messageObject);
                    break;
                default:
                    break;
            }
            return message;
        }

        private static FriendMessage ParseFriendMessage(JsonObject item)
        {
            return new FriendMessage { };
        }

        public static GroupSystemMessage ParseGroupSystemMessage(JsonValue item)
        {
            GroupSystemMessage message = null;
            string type = item["type"].ToString().Trim('"');
            switch (type)
            {
                case "group_request_join":
                    message = ParseJoinGroupRequestMessage(item);
                    break;
                case "group_join":
                    message = ParseMemberJoinedMessage(item);
                    break;
                case "group_leave":
                    message = ParseMemberLeavedMessage(item);
                    break;
                default:
                    //TODO:Log undefined message
                    break;
            }
            return message;
        }


        private static JoinGroupRequestMessage ParseJoinGroupRequestMessage(JsonValue item)
        {
            JoinGroupRequestMessage systemMessage = new JoinGroupRequestMessage();
            InitBaseGroupMessage(item, systemMessage);
            systemMessage.MemberUin = item["request_uin"];
            systemMessage.MemberTrueUin = item["t_request_uin"];
            systemMessage.RequestMessage = item["msg"];
            return systemMessage;
        }

        private static MemberJoinedMessage ParseMemberJoinedMessage(JsonValue item)
        {
            MemberJoinedMessage systemMessage = new MemberJoinedMessage();
            InitBaseGroupMessage(item, systemMessage);
            systemMessage.MemberUin = item["new_member"];
            systemMessage.MemberTrueUin = item["t_new_member"];
            systemMessage.AdminNick = item["admin_nick"];
            systemMessage.AdminUin = item["admin_uin"];
            return systemMessage;
        }

        private static MemberLeavedMessage ParseMemberLeavedMessage(JsonValue item)
        {
            MemberLeavedMessage systemMessage = new MemberLeavedMessage();
            InitBaseGroupMessage(item, systemMessage);
            systemMessage.MemberUin = item["old_member"];
            systemMessage.MemberTrueUin = item["t_old_member"];
            systemMessage.AdminNick = item["admin_nick"];
            systemMessage.AdminUin = item["admin_uin"];
            return systemMessage;
        }

        public static GroupMessage ParseGroupMessage(JsonValue jsonValue)
        {
            GroupMessage groupMessage = new GroupMessage();

            JsonValue item = jsonValue["value"];

            groupMessage.TrueGroupUin = item["info_seq"];
            groupMessage.GroupUin = item["from_uin"];
            groupMessage.SenderUin = item["send_uin"];
            groupMessage.SentTime = WebQQUtil.GetLocalTimeByLong(item["time"]);
            groupMessage.Order = item["seq"];

            groupMessage.FontStyle = item["content"][0].ToString();
            groupMessage.Message = item["content"].ToString().Replace(groupMessage.FontStyle, "");
            return groupMessage;
        }

        private static GroupMessageType ParseGroupType(string typeString)
        {
            GroupMessageType groupMessageType;
            switch (typeString)
            {
                case "group_request_join":
                    groupMessageType = GroupMessageType.RequestJoin;
                    break;
                case "group_join":
                    groupMessageType = GroupMessageType.Joined;
                    break;
                case "group_leave":
                    groupMessageType = GroupMessageType.Leaved;
                    break;
                default:
                    groupMessageType = GroupMessageType.UnDefined;
                    break;
            }
            return groupMessageType;
        }

        private static void InitBaseGroupMessage(JsonValue item, GroupSystemMessage systemMessage)
        {
            systemMessage.GroupUin = item["from_uin"];
            systemMessage.Type = ParseGroupType(item["type"]);
            systemMessage.TrueGroupUin = item["t_gcode"];
        }
    }
}
