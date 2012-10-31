using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.Model.Messages
{
    public class BadMessage : BaseMessage
    {
        public BadMessage(string message)
            : this(message, string.Empty)
        {
        }

        public BadMessage(string message, string content)
        {
            this.Message = message;
            this.Content = content;
        }

        public string Message { get; set; }

        public string Content { get; set; }
    }
}
