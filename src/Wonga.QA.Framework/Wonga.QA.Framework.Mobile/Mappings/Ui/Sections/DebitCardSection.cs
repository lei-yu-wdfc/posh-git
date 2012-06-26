using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Mobile.Mappings.Sections
{
    public class DebitCardSection
    {
        public String Fieldset { get; set; }
        public String CardType { get; set; }
        public String CardNumber { get; set; }
        public String CardName { get; set; }
        public String CardExpiryDateMonth { get; set; }
        public String CardExpiryDateYear { get; set; }
        public String CardStartDateMonth { get; set; }
        public String CardStartDateYear { get; set; }
        public String CardSecurityNumber { get; set; }
    }
}
