using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.Model.Messages
{
    public class InPackage<T> where T:BaseMessage
    {
        public int RetCode { get; set; }
        public T Result { get; set; }
    }
}
