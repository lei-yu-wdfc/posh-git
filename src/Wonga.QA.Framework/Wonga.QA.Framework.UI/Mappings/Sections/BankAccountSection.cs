using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Sections
{
    public class BankAccountSection
    {
        public String Legend { get; set; }
        public String BankName { get; set; }
        public String SortCodePart1 { get; set; }
        public String SortCodePart2 { get; set; }
        public String SortCodePart3 { get; set; }
        public String AccountNumber { get; set; }
        public String BankPeriod { get; set; }
        public String BankAccountType { get; set; }
    }
}
