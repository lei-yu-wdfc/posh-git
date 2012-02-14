using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Sections
{
    internal class DebitCardElement
    {
        internal String Legend { get; set; }
        internal String CardType { get; set; }
        internal String CardNumber { get; set; }
        internal String CardName { get; set; }
        internal String CardExpiryDateMonth { get; set; }
        internal String CardExpiryDateYear { get; set; }
        internal String CardStartDateMonth { get; set; }
        internal String CardStartDateYear { get; set; }
        internal String CardSecurityNumber { get; set; }
    }
}
