using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Pages
{
    public class MyAccountRepayManualPage
    {
        public string RepaymentDate { get; set; }
        public string AmountDueOnDueDate { get; set; }
        public string RepaymentAmount { get; set; }
        public string RemainderToRepay { get; set; }
        public string BankAccountMasked { get; set; }
        public string PleaseReadMeRepaymentAmount { get; set; }
        public string BankAccountMaskedLabelRepaymentAmount { get; set; }
        public string Captcha { get; set; }
        public string Submit { get; set; }
    }
}
