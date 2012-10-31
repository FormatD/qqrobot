using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.Model.Messages
{
    public class MemberLeavedMessage : GroupSystemMessage
    {
        public MemberLeavedMessage()
        {
            this.Type = GroupMessageType.Leaved;
        }
        public long AdminUin { get; set; }
        public long AdminNick { get; set; }
    }
}
