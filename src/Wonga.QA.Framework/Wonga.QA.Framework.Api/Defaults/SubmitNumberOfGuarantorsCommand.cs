using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SubmitNumberOfGuarantorsCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            ApplicationId = Data.GetId();
            NumberOfGuarantors = 0;
        }
    }
}
