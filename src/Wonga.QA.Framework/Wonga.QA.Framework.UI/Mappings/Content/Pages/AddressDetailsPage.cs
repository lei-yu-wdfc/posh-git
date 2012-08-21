using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Wonga.QA.Framework.UI.Mappings.Content
{
    public class AddressDetailsPage
    {
        public String PostcodeError1 { get; set; }
        public String PostcodeError2 { get; set; }
        public String AddresPeriodLess4Month { get; set; }
    }
}
