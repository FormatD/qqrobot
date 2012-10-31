using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Format.WebQQ.Contract;
using Format.WebQQ.WebQQ2.DLL;
using Format.WebQQ.Model.Messages;

namespace Format.WebQQ.Robot.Rule
{
    public class BlackListRule : BaseRule
    {
        public BlackListRule(IWebQQ webqq, ConfigManager config)
            : base(webqq, config)
        {
        }

        protected override void InitRule()
        {
            base.InitRule();
            webqq.OnGroupMessageArrivedEvent += ProcessMessage;
            webqq.OnMemberJoined += ProcessMessage;
        }

        public void ProcessMessage(BaseMessage message)
        {
            List<long> blackUins = config.GetBlackList();

            long trueGroupUin = long.MinValue;
            long trueUin = long.MinValue;

            if (message is GroupMessage)
            {
                GroupMessage groupMessage = message as GroupMessage;
                trueGroupUin = groupMessage.TrueGroupUin;
                trueUin = webqq.GetOrginalUserID(groupMessage.SenderUin);
            }
            else if (message is MemberJoinedMessage)
            {
                GroupSystemMessage groupSystemMessage = message as GroupSystemMessage;
                trueGroupUin = groupSystemMessage.TrueGroupUin;
                trueUin = groupSystemMessage.MemberTrueUin;
            }

            if (trueGroupUin != long.MinValue &&
                trueUin != long.MinValue &&
                blackUins.Contains(trueUin))
            {
                webqq.DeleteGroupMmeber(trueGroupUin, new long[] { trueUin });
            }

        }
    }
}
