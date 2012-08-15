using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework
{
    public class LoanExtension
    {
        public DateTime Term;
        public Guid CustomerId;
        public Guid ApplicationId;
        public bool IsEnabled;
        public Guid LoanExtensionId;
        public bool HasStatusAccepted;
        public double PartPaymentAmount;
        public double TodaysBalance;
        public double OriginalBalance;
        public double NewFinalBalance;


        public LoanExtension(bool hasStatusAccepted, Guid loanExtensionId, Guid customerId, Guid apllicationId, DateTime term, double partPaymentAmount, double todaysBalance, double originalBalance, double newFinalBalance)
        {
            HasStatusAccepted = hasStatusAccepted;
            Term = term;
            IsEnabled = true;
            LoanExtensionId = loanExtensionId;
            CustomerId = customerId;
            ApplicationId = apllicationId;
            PartPaymentAmount = partPaymentAmount;
            TodaysBalance = todaysBalance;
            OriginalBalance = originalBalance;
            NewFinalBalance = newFinalBalance;
        }
    }
}
