using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.Model.Messages
{
    public class JoinGroupRequestInPackage : InPackage<JoinGroupRequestMessage>
    {
        public JoinGroupRequestInPackage(JoinGroupRequestMessage message)
        {
            this.Result = message;
        }
    }
}
