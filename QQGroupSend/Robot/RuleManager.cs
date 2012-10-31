using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Format.WebQQ.Robot.Rule;

namespace Format.WebQQ.Robot
{
    public class RuleManager
    {
        List<BaseRule> rules = new List<BaseRule>();

        public void RegisterRule(BaseRule rule)
        {
            rules.Add(rule);
        }
    }
}
