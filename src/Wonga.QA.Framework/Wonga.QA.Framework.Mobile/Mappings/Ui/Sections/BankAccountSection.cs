using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Mobile.Mappings.Sections
{
    public class BankAccountSection
    {
        public String Fieldset { get; set; }
        public String BankName { get; set; }
        public String SortCodePart1 { get; set; }
        public String SortCodePart2 { get; set; }
        public String SortCodePart3 { get; set; }
        public String AccountNumber { get; set; }
        public String BankPeriod { get; set; }
        public String BankAccountType { get; set; }
        public String InstitutionNumber { get; set; }
        public String BranchNumber { get; set; }
    }
}
