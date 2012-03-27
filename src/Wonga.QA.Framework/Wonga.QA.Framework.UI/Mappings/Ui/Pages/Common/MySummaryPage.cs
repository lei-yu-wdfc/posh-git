using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Pages
{
    public sealed class MySummaryPage
    {
        public String Title { get; set; }
        public String TotalToRepay { get; set; }
        public String RepaymentDate { get; set;} 
        public String PromisedRepayAmount { get; set; }
        public String PromisedRepayDate { get; set; }
        public String RepayButton { get; set; }
        public String TotalToRepayAmountPopup { get; set; }
        public String PromisedRepayDatePopup { get; set; }
    }
}
