using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Account;
using Wonga.QA.Framework.Account.Queries;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Payments.PayLater.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.PayLater.Uk
{
	public class PayLaterApplicationBuilder : PayLaterApplicationBuilderBase
	{
		public PayLaterApplicationBuilder(PayLaterAccount account, PayLaterApplicationDataBase applicationData) : base(account, applicationData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
            var paymentCardGuid = AccountQueries.PayLater.PaymentDetails.GetPrimaryPaymentCardGuid(Account.Id);
		    var postCode = AccountQueries.PayLater.CustomerDetails.GetCustomerPostCode(Account.Id);

		    yield return RiskCreatePayLaterApplicationUkCommand.New(r =>
		                                                                {
		                                                                    r.AccountId = Account.Id;
		                                                                    r.ApplicationId = ApplicationId;
		                                                                    r.PaymentCardId = paymentCardGuid;
		                                                                    r.TotalAmount = PayLaterApplicationData.TotalAmount;
		                                                                });

		    yield return CreateApplicationPayLaterUkCommand.New(r =>
		                                                            {
		                                                                r.AccountId = Account.Id;
		                                                                r.ApplicationId = ApplicationId;
		                                                                r.MerchantId = PayLaterApplicationData.MerchantId;
		                                                                r.MerchantReference = PayLaterApplicationData.MerchantReference;
		                                                                r.MerchantOrderId = PayLaterApplicationData.MerchantOrderId;
		                                                                r.TotalAmount = PayLaterApplicationData.TotalAmount;
		                                                                r.Currency = CurrencyCodeEnum.GBP;
		                                                                r.PostCode = postCode;
		                                                            });

		    yield return VerifyPaylaterApplicationUkCommand.New(r =>
		                                                            {
		                                                                r.AccountId = Account.Id;
		                                                                r.ApplicationId = ApplicationId;
		                                                            });
		}

		protected override void WaitForApplicationToBecomeLive()
		{
		    Do.Until(
		        () =>
		        Drive.Api.Queries.Post(new GetPayLaterCustomerTransactionsStatusPayLaterUkQuery {AccountId = Account.Id}).
		            Values["Status"].Single() == "HasPayLaterLiveTransactionsOnly");
		}
	}
}
