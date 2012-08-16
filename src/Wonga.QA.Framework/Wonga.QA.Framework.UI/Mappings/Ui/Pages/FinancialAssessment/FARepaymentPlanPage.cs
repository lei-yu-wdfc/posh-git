using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Wonga.QA.Framework.UI.Mappings.Pages.FinancialAssessment
{
    public sealed class FARepaymentPlanPage
    {
        public String FirstRepaymentDate { get; set; }
        public String PaymentFrequency { get; set; }
        public String RepaymentAmount { get; set; }
        public String ButtonPrevious { get; set; }
        public String ButtonNext { get; set; }
    }
}
