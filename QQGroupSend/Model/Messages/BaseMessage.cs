using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.Model.Messages
{
    public class BaseMessage
    {
        public BaseMessage()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }
    }
}
