using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.Model.Messages
{
    public class GroupSignMessageResult
    : InPackage<GroupSignMessage>
    {
    }

    public class GroupSignMessage : BaseMessage
    {
        public int type { get; set; }
        public string value { get; set; }
        public AllowedFlag flags { get; set; }
    }

    public class AllowedFlag
    {
        public int text { get; set; }
        public int pic { get; set; }
        public int audio { get; set; }
        public int video { get; set; }
    }
}
