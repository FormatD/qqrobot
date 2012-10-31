using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.Model.Messages
{
    public class MemberJoinedMessage : GroupSystemMessage
    {
        public MemberJoinedMessage()
        {
            this.Type = GroupMessageType.Joined;
        }
        public long AdminUin { get; set; }
        public string AdminNick { get; set; }
    }
}
