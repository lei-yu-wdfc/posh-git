using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework
{
    public class LoanExtension
    {
        public DateTime Term;
        public Guid ApplicationId;
        public bool IsEnabled;
        public Guid LoanExtensionId;
        public bool HasStatusAccepted;
        public double PartPaymentAmount;
        public double OriginalBalance;
        public double NewFinalBalance;


        public LoanExtension(bool hasStatusAccepted, Guid loanExtensionId, Guid apllicationId, DateTime term, double partPaymentAmount, double originalBalance, double newFinalBalance)
        {
            HasStatusAccepted = hasStatusAccepted;
            Term = term;
            IsEnabled = true;
            LoanExtensionId = loanExtensionId;
            ApplicationId = apllicationId;
            PartPaymentAmount = partPaymentAmount;
            OriginalBalance = originalBalance;
            NewFinalBalance = newFinalBalance;
        }
    }
}
