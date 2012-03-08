using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Pages
{
    public class AcceptedPage
    {
        public String FormId { get; set; }
        public String AcceptBusinessLoan { get; set; }
        public String AcceptGuarantorLoan { get; set; }
        public String AgreementConfirm { get; set; }
        public String DirectDebitConfirm { get; set; }
        public String SubmitButton { get; set; }

        public String Initials1 { get; set; }
        public String Initials2 { get; set; }
        public String Initials3 { get; set; }
        public String Signature { get; set; }
        public String DateOfAgreement { get; set; }
        public String ContinueTermsButton { get; set; }
        public String ContinueDirectDebitButton { get; set; }
    }
}
