using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Format.WebQQ.Model.Messages;
using Format.WebQQ.Contract;

namespace Format.WebQQ.Robot.Rule
{
    public abstract class BaseRule
    {
        protected ConfigManager config = null;
        protected IWebQQ webqq = null;

        public BaseRule(IWebQQ webqq,ConfigManager config)
        {
            this.webqq = webqq;
            this.config = config;
            InitRule();
        }

        protected virtual void InitRule()
        {

        }
    }
}
