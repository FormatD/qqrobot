using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Format.WebQQ.Contract;
using Format.WebQQ.Model.Messages;

namespace Format.WebQQ.Robot.Rule
{
    public class JoinRequestRule : BaseRule
    {
        public JoinRequestRule(IWebQQ webqq, ConfigManager config)
            : base(webqq, config)
        {
        }

        protected override void InitRule()
        {
            base.InitRule();
            webqq.OnJoinRequested += new JoinRequestedHandle(OnJoinRequested);
        }

        private void OnJoinRequested(JoinGroupRequestMessage message)
        {
            List<long> blackList = config.GetBlackList();
            if (blackList == null || !blackList.Contains(message.MemberTrueUin))
            {
                webqq.AcceptJoinRequest(message);
            }
        }

    }
}
