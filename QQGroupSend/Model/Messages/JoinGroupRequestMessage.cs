using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.Model.Messages
{
    public class JoinGroupRequestMessage : GroupSystemMessage
    {
        public JoinGroupRequestMessage()
        {
            this.Type = GroupMessageType.RequestJoin;
        }
        public string RequestMessage { get; set; }
    }
}
