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
        public String BalanceToday { get; set; }
        public String RepaymentDate { get; set; }
        public String PromisedRepayAmount { get; set; }
        public String PromisedRepayDate { get; set; }
        public String RepayButton { get; set; }
        public String TotalToRepayAmountPopup { get; set; }
        public String PromisedRepayDatePopup { get; set; }
        public String TagCloud { get; set; }
        public String ViewLoanDetailsButton { get; set; }
        public String LoanStatusMessage { get; set; }
        public String ChangePromiseDateButton { get; set; }
        public String AccountStatusText { get; set; }

        //--loan details popup--
        public String PopupForm { get; set; }
        public String PopupMySummaryTitle { get; set; }
        public String PopupSummaryDetailsTable { get; set; }

        //--FE elements--
        public String YouCan { get; set; }
        public String Promise { get; set; }
        public String OptionsCloud { get; set; }
        public String IntroText { get; set; }
        public String StatusMessage { get; set; }
        public String MaxAvailableCredit { get; set; }
        public String StatusText { get; set; }
        public String PromiseText { get; set; }
    }
}
