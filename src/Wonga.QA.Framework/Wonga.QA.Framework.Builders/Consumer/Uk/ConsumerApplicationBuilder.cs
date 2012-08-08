using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer.Uk
{
	public class ConsumerApplicationBuilder : ConsumerApplicationBuilderBase
	{
		public ConsumerApplicationBuilder(Customer consumerAccountBase, ConsumerApplicationDataBase consumerApplicationData) : base(consumerAccountBase, consumerApplicationData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			var primaryPaymentCardGuid = ConsumerAccountBase.GetPaymentCard();
			var primaryBankAccountGuid = ConsumerAccountBase.GetBankAccount();

			yield return CreateFixedTermLoanApplicationUkCommand.New(r =>
			                                                         	{
			                                                         		r.ApplicationId = ApplicationId;
			                                                         		r.AccountId = ConsumerAccountBase.Id;
			                                                         		r.BankAccountId = primaryBankAccountGuid;
			                                                         		r.PaymentCardId = primaryPaymentCardGuid;
			                                                         		r.LoanAmount = ConsumerApplicationData.LoanAmount;
			                                                         		r.PromiseDate = ConsumerApplicationData.PromiseDate;
			                                                         	});
			yield return RiskCreateFixedTermLoanApplicationCommand.New(r =>
			                                                           	{
			                                                           		r.ApplicationId = ApplicationId;
			                                                           		r.AccountId = ConsumerAccountBase.Id;
			                                                           		r.BankAccountId = primaryBankAccountGuid;
			                                                           		r.LoanAmount = ConsumerApplicationData.LoanAmount;
			                                                           		r.PromiseDate = ConsumerApplicationData.PromiseDate;
			                                                           		r.PaymentCardId = primaryPaymentCardGuid;
			                                                           	});
			yield return VerifyFixedTermLoanCommand.New(r =>
			                                            	{
			                                            		r.AccountId = ConsumerAccountBase.Id;
			                                            		r.ApplicationId = ApplicationId;
			                                            	});
		}

		protected override void WaitForApplicationToBecomeLive()
		{
			Do.Until(() => Drive.Api.Queries.Post(new GetAccountSummaryQuery{AccountId = ConsumerAccountBase.Id}).Values["HasCurrentLoan"].Single() == "true");
		}
	}
}
