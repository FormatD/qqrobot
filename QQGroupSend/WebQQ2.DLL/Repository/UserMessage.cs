using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Format.WebQQ.WebQQ2.DLL.Repository
{
    public class UserMessage
    {
        public Guid Guid { get; set; }
        public int MsgID { get; set; }
        public string FromUin { get; set; }
        public string ToUin { get; set; }
        public string Content { get; set; }
        public Font Font { get; set; }
    }
}
