using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Builders.Consumer
{
    abstract class ConsumerLoanExtensionBuilderBase
    {
        protected ConsumerLoanExtensionDataBase ExtensionData;
        public Guid PaymentCardId;
        public string PaymentCardCv2;
        public Guid CustomerId;
        public Guid ApplicationId;

        public ConsumerLoanExtensionBuilderBase(Guid customerId, Guid applicationId, DateTime term, double partPaymentAmount)
        {
            CustomerId = customerId;
            ApplicationId = applicationId;
            ExtensionData.Term = term;
            ExtensionData.PartPaymentAmount = (Decimal)partPaymentAmount;
        }

        public LoanExtension Build()
        {
            GetGetPaymentCardId(CustomerId);
            ExtensionData.OriginalBalance = (decimal)GetOriginalBalance();
            ExtensionData.NewFinalBalance = (decimal)GetNewFinalBalance();
            RequestLoanExtension();
            if (ExtensionData.HasStatusAccepted)
                AcceptLoanExtension();

            return CreateLoanExtension((bool)ExtensionData.HasStatusAccepted, new Guid(ExtensionData.LoanExtensionId), CustomerId, ApplicationId, (DateTime)ExtensionData.Term, (double)ExtensionData.PartPaymentAmount, (double)ExtensionData.OriginalBalance, (double)ExtensionData.NewFinalBalance);
        }

        public ConsumerLoanExtensionBuilderBase OnlyInRequest()
        {
            ExtensionData.HasStatusAccepted = false;
            return this;
        }

        protected abstract LoanExtension CreateLoanExtension(bool hasStatusAccepted, Guid loanExtensionId, Guid customerId, Guid apllicationId, DateTime term, double partPaymentAmount, double originalBalance, double newFinalBalance);
        protected abstract void RequestLoanExtension();
        protected abstract void AcceptLoanExtension();
        protected abstract void GetGetPaymentCardId(Guid cusotmerId);
        protected abstract double GetOriginalBalance();
        protected abstract double GetNewFinalBalance();
    }
}
