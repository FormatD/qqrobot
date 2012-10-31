using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Format.WebQQ.Contract;
using Format.WebQQ.Model.Messages;

namespace Format.WebQQ.Robot.Rule
{
    public class BadWordsRule : BaseRule
    {
        public BadWordsRule(IWebQQ webqq, ConfigManager config)
            : base(webqq, config)
        {
        }

        protected override void InitRule()
        {
            base.InitRule();
            webqq.OnGroupMessageArrivedEvent += ProcessMessage;
        }

        public void ProcessMessage(BaseMessage message)
        {
            List<long> blackList = new List<long>();
            List<string> keyWords = new List<string>();

            GroupMessage groupMessage = message as GroupMessage;

            if (groupMessage != null)
            {
                long trueUin = webqq.GetOrginalUserID(groupMessage.SenderUin);
                foreach (var badWord in keyWords)
                {
                    if (groupMessage.Message.Contains(badWord))
                    {
                        if (blackList.Contains(trueUin))
                            blackList.Add(trueUin);
                        webqq.DeleteGroupMmeber(groupMessage.TrueGroupUin, trueUin);
                    }
                }
            }
        }
    }
}
