using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Builders.Consumer
{
    abstract class ConsumerLoanExtensionBuilderBase
    {
        protected ConsumerLoanExtensionDataBase ExtensionData;

        public ConsumerLoanExtensionBuilderBase(Guid customerId, Guid applicationId, DateTime term, double partPaymentAmount)
        {
            ExtensionData.CustomerId = Convert.ToString(customerId);
            ExtensionData.ApplicationId = Convert.ToString(applicationId);
            ExtensionData.Term = term;
            ExtensionData.PartPaymentAmount = (Decimal)partPaymentAmount;
        }

        public LoanExtension Build()
        {
            GetCusotmerDetails(new Guid(ExtensionData.CustomerId));
            ExtensionData.TodaysBalance = (decimal)GetTodaysBalance();
            ExtensionData.OriginalBalance = (decimal)GetOriginalBalance();
            ExtensionData.NewFinalBalance = (decimal)GetNewFinalBalance();
            RequestLoanExtension();
            if (ExtensionData.HasStatusAccepted)
                AcceptLoanExtension();

            return CreateLoanExtension((bool)ExtensionData.HasStatusAccepted, new Guid(ExtensionData.LoanExtensionId), new Guid(ExtensionData.CustomerId), new Guid(ExtensionData.ApplicationId), (DateTime)ExtensionData.Term, (double)ExtensionData.PartPaymentAmount, (double)ExtensionData.TodaysBalance, (double)ExtensionData.OriginalBalance, (double)ExtensionData.NewFinalBalance);
        }

        public ConsumerLoanExtensionBuilderBase OnlyInRequest()
        {
            ExtensionData.HasStatusAccepted = false;
            return this;
        }

        protected abstract LoanExtension CreateLoanExtension(bool hasStatusAccepted, Guid loanExtensionId, Guid customerId, Guid apllicationId, DateTime term, double partPaymentAmount, double todaysBalance, double originalBalance, double newFinalBalance);
        protected abstract void RequestLoanExtension();
        protected abstract void AcceptLoanExtension();
        protected abstract void GetCusotmerDetails(Guid cusotmerId);
        protected abstract double GetTodaysBalance();
        protected abstract double GetOriginalBalance();
        protected abstract double GetNewFinalBalance();
    }
}
