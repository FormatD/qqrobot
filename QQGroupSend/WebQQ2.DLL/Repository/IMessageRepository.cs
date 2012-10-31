using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.WebQQ2.DLL.Repository
{
    public interface IMessageRepository
    {
        void SaveMessage(UserMessage message);
        List<UserMessage> UserMessageFor(string uin);
        List<UserMessage> GroupMessageFor(string groupUin);
        List<UserMessage> GroupMessageFor(string groupUin, string uin);

    }
}
