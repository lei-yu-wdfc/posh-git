using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Mobile.Mappings.Sections
{
    public class MobilePinVerificationSection
    {
        public String Fieldset { get; set; }
        public String Pin { get; set; }
        public String ResendPin { get; set; }
        public String ResendPinMessage { get; set; }
        public String ResendPinPopupClose { get; set; }
    }
}
