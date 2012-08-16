using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Account;
using Wonga.QA.Framework.Account.Consumer;
using Wonga.QA.Framework.Account.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Builders.Consumer;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;

namespace Wonga.QA.Framework.Builders.Consumer.Uk
{
    class ConsumerLoanExtensionBuilder : ConsumerLoanExtensionBuilderBase
    {
        public ConsumerLoanExtensionBuilder(Guid customerId, Guid applicationId, DateTime term, double partPaymentAmount)
            : base(customerId, applicationId, term, partPaymentAmount)
        {
        }

        protected override LoanExtension CreateLoanExtension(bool hasStatusAccepted, Guid loanExtensionId, Guid customerId, Guid apllicationId, DateTime term, double partPaymentAmount, double originalBalance, double newFinalBalance)
        {
            return new LoanExtension(hasStatusAccepted, loanExtensionId, customerId, apllicationId, term, partPaymentAmount, originalBalance, newFinalBalance);
        }

        protected override void RequestLoanExtension()
        {
            var response = Drive.Api.Queries.Post(CreateFixedTermLoanExtensionCommand.New(r =>
            {
                r.ApplicationId = ApplicationId;
                r.ExtensionId = ExtensionData.LoanExtensionId;
                r.PaymentCardId = PaymentCardId;
                r.PartPaymentAmount = ExtensionData.PartPaymentAmount;
            }));
        }

        protected override void AcceptLoanExtension()
        {
            var response = Drive.Api.Queries.Post(SignFixedTermLoanExtensionCommand.New(r =>
            {
                r.ApplicationId = ApplicationId;
                r.ExtensionId = ExtensionData.LoanExtensionId;
            }));
        }

        protected override void GetGetPaymentCardId(Guid cusotmerId)
        {
            AccountQueriesPaymentDetails accountQueries = new AccountQueriesPaymentDetails();
            PaymentCardId = accountQueries.GetPrimaryPaymentCardGuid(new ConsumerAccount(cusotmerId));
        }

        protected override double GetOriginalBalance()
        {
            var response = Drive.Api.Queries.Post(GetFixedTermLoanExtensionCalculationQuery.New(r =>
            {
                r.ApplicationId = ApplicationId;
                r.ExtendDate = ExtensionData.ExtendDate;
            }));

            return Convert.ToDouble(response.Values["OriginalBalance"].Single());
        }

        protected override double GetNewFinalBalance()
        {
            var response = Drive.Api.Queries.Post(GetFixedTermLoanExtensionCalculationQuery.New(r =>
            {
                r.ApplicationId = ApplicationId;
                r.ExtendDate = ExtensionData.ExtendDate;
            }));

            return Convert.ToDouble(response.Values["NewFinalBalance"].Single());
        }
    }
}
