using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Format.WebQQ.WebQQ2.DLL;
using Format.WebQQ.WebQQ2.DLL.Repository;

namespace Format.WebQQ.WebQQ2.DLL.Repository
{
    public class MessageRepository : IMessageRepository
    {
        QQDbContext xxxx = new QQDbContext();



        public void SaveMessage(UserMessage message)
        {
            throw new NotImplementedException();
        }

        public List<UserMessage> UserMessageFor(string uin)
        {
            throw new NotImplementedException();
        }

        public List<UserMessage> GroupMessageFor(string groupUin)
        {
            throw new NotImplementedException();
        }

        public List<UserMessage> GroupMessageFor(string groupUin, string uin)
        {
            throw new NotImplementedException();
        }
    }
}
