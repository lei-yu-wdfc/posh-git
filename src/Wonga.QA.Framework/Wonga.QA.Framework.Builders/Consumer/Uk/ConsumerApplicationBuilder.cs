using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Account.Consumer;
using Wonga.QA.Framework.Account.Queries;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer.Uk
{
	public class ConsumerApplicationBuilder : ConsumerApplicationBuilderBase
	{
		public ConsumerApplicationBuilder(ConsumerAccount account, ConsumerApplicationDataBase applicationData) : base(account, applicationData)
		{
		}
		
		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			var primaryPaymentCardGuid = AccountQueries.Consumer.PaymentDetails.GetPrimaryPaymentCardGuid(Account.Id);
			var primaryBankAccountGuid = AccountQueries.Consumer.PaymentDetails.GetPrimaryBankAccountGuid(Account.Id);

			yield return CreateFixedTermLoanApplicationUkCommand.New(r =>
			                                                         	{
			                                                         		r.ApplicationId = ApplicationId;
			                                                         		r.AccountId = Account.Id;
			                                                         		r.BankAccountId = primaryBankAccountGuid;
			                                                         		r.PaymentCardId = primaryPaymentCardGuid;
			                                                         		r.LoanAmount = ConsumerApplicationData.LoanAmount;
			                                                         		r.PromiseDate = ConsumerApplicationData.PromiseDate;
			                                                         	});
			yield return RiskCreateFixedTermLoanApplicationCommand.New(r =>
			                                                           	{
			                                                           		r.ApplicationId = ApplicationId;
			                                                           		r.AccountId = Account.Id;
			                                                           		r.BankAccountId = primaryBankAccountGuid;
			                                                           		r.LoanAmount = ConsumerApplicationData.LoanAmount;
			                                                           		r.PromiseDate = ConsumerApplicationData.PromiseDate;
			                                                           		r.PaymentCardId = primaryPaymentCardGuid;
			                                                           	});
			yield return VerifyFixedTermLoanCommand.New(r =>
			                                            	{
			                                            		r.AccountId = Account.Id;
			                                            		r.ApplicationId = ApplicationId;
			                                            	});
		}

		protected override void WaitForApplicationToBecomeLive()
		{
			Do.Until(() => Drive.Api.Queries.Post(new GetAccountSummaryQuery{AccountId = Account.Id}).Values["HasCurrentLoan"].Single() == "true");
		}
	}
}
