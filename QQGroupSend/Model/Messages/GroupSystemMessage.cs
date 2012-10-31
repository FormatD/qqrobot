using System;
using System.Collections.Generic;
using System.Linq;
using Format.WebQQ.Model.Entities;

namespace Format.WebQQ.Model.Messages
{
    public class GroupSystemMessage : BaseMessage
    {
        public long GroupUin { get; set; }
        public long TrueGroupUin { get; set; }
        public long MemberUin { get; set; }
        public long MemberTrueUin { get; set; }

        public GroupMessageType Type { get; set; }

        public GroupEntity GroupEntity { get; set; }
    }
}
