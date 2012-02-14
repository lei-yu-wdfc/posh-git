using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Sections
{
    internal class BankAccountElement
    {
        internal String Legend { get; set; }
        internal String BankName { get; set; }
        internal String SortCodePart1 { get; set; }
        internal String SortCodePart2 { get; set; }
        internal String SortCodePart3 { get; set; }
        internal String AccountNumber { get; set; }
        internal String BankPeriod { get; set; }
    }
}
