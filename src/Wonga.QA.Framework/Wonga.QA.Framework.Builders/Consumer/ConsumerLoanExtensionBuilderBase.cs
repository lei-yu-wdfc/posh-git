using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Account.Queries;

namespace Wonga.QA.Framework.Builders.Consumer
{
    abstract class ConsumerLoanExtensionBuilderBase
    {
        protected ConsumerLoanExtensionDataBase ExtensionData;
        protected Guid LoanExtensionId { get; private set; }
        protected Guid PaymentCardId { get; set; }
        protected string PaymentCardCv2 { get; private set; }
        protected Guid ApplicationId { get; private set; }
        protected bool HasStatusAccepted { get; private set; }
        protected Decimal OriginalBalance { get; private set; }
        protected Decimal NewFinalBalance { get; private set; }

        public ConsumerLoanExtensionBuilderBase(Guid applicationId, DateTime term, double partPaymentAmount, ConsumerLoanExtensionDataBase LoanExtensionData)
        {
            ExtensionData = LoanExtensionData;
            LoanExtensionId = Guid.NewGuid();
            ApplicationId = applicationId;
            ExtensionData.Term = term;
            ExtensionData.PartPaymentAmount = (Decimal)partPaymentAmount;
        }

        public LoanExtension Build()
        {
            GetPaymentCardId();
            OriginalBalance = (decimal)GetOriginalBalance();
            NewFinalBalance = (decimal)GetNewFinalBalance();
            RequestLoanExtension();
            if (HasStatusAccepted)
                AcceptLoanExtension();

            return CreateLoanExtension(HasStatusAccepted, LoanExtensionId, ApplicationId, (DateTime)ExtensionData.Term, (double)ExtensionData.PartPaymentAmount, OriginalBalance, NewFinalBalance);
        }

        public ConsumerLoanExtensionBuilderBase OnlyInRequest()
        {
            HasStatusAccepted = false;
            return this;
        }

        protected abstract LoanExtension CreateLoanExtension(bool hasStatusAccepted, Guid loanExtensionId, Guid apllicationId, DateTime term, double partPaymentAmount, double originalBalance, double newFinalBalance);
        protected abstract void RequestLoanExtension();
        protected abstract void AcceptLoanExtension();
        protected abstract void GetPaymentCardId();
        protected abstract double GetOriginalBalance();
        protected abstract double GetNewFinalBalance();
    }
}
