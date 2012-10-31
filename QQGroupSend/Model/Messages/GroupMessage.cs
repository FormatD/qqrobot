using System;
using System.Collections.Generic;
using System.Linq;
using Format.WebQQ.Model.Entities;

namespace Format.WebQQ.Model.Messages
{
    public class GroupMessage:BaseMessage
    {
        public long GroupUin { get; set; }
        public long TrueGroupUin { get; set; }
        public long SenderUin { get; set; }
        public long TrueSenderUin { get; set; }
        public string FontStyle { get; set; }
        public string Message { get; set; }
        public DateTime? SentTime { get; set; }
        public int Order { get; set; }

        public GroupEntity GroupEntity { get; set; }
    }
}
