using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.ThirdParties
{
    public class ThirdPartyDriver
    {
        public ThirdPartyDriver()
        {
            ExactTarget = new ExactTarget();
        }

        public ExactTarget ExactTarget { get; private set; }    
    }
}
