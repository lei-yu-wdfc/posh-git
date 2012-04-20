using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Sections
{
    /// <summary>
    /// Mobile PIN verification section
    /// </summary>
    public class MobilePinVerificationSection
    {
        public String Fieldset { get; set; }
        public String Pin { get; set; }
        public String ResendPin { get; set; }
        public String ResendPinMessage { get; set; }
    }
}
