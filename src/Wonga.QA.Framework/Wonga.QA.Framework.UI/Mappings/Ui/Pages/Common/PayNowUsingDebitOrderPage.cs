using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Pages
{
    public class PayNowUsingDebitOrderPage
    {
        public string Form { get; set; }
        public string EditRepaymentDate { get; set; }
        public string AmountOnDueDate { get; set; }
        public string EditRepaymentAmount { get; set; }
        public string RemainderToPay { get; set; }
        public string EditBankAccountMasked { get; set; }
        public string Captcha { get; set; }
        public string EditCaptchaField { get; set; }
        public string CancelButton { get; set; }
        public string Submit { get; set; }
    }
}
