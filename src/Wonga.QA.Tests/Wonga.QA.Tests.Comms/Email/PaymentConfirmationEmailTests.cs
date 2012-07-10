using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Email
{
	[TestFixture]
	public class PaymentConfirmationEmailTests
	{
		public class GivenACustomer : PaymentConfirmationEmailTests
		{
			private TestLocal<Customer> _customer;

			[SetUp]
			public virtual void SetUp()
			{
				_customer = new TestLocal<Customer> {Value = CustomerBuilder.New().Build()};
			}

			public class WhenTheirApplicationIsFunded : GivenACustomer
			{
				private TestLocal<Application> _application;

				public override void  SetUp()
				{
					base.SetUp();

					_application = new TestLocal<Application> {Value = ApplicationBuilder.New(_customer.Value).Build()};
				}

				[Test, AUT(AUT.Za), JIRA("WIN-1130", "WIN-886"), Pending("Doesn't work")]
				public void PaymentConfirmationEmailIsSent()
				{
					var tokens =
						CreateEmailTokensQuery(
							_customer.Value.Email,
							GetEmailTemplateId("Email.PaymentConfirmationEmailTemplate"));

					WaitForFirstToken(tokens);

					VerifyEmailTokenValue(tokens, "First_name", _customer.Value.GetCustomerForename());
					VerifyEmailTokenValue(tokens, "Loan_amount", _application.Value.LoanAmount.ToString("0.00"));
					VerifyEmailTokenValue(tokens, "Total_repayable", _application.Value.GetDueDateBalance().ToString("0.00"));
					VerifyEmailTokenValue(tokens, "Promise_date",
						DateTime.Now.AddDays(_application.Value.LoanTerm).ToShortDateString());
				}

				private static dynamic CreateEmailTokensQuery(string emailAddress, string templateName)
				{
					dynamic emailTokenTable = Drive.Data.QaData.Db.EmailToken;

					return Do.Until<object>(() => emailTokenTable.FindAll(
						emailTokenTable.Email.EmailAddress == emailAddress &&
						emailTokenTable.Email.TemplateName == templateName));
				}

				private static void VerifyEmailTokenValue(dynamic tokens, string key, string value)
				{
					var token = GetEmailToken(tokens, key);

					Assert.IsNotNull(token);
					Assert.AreEqual(token.Value, value);
				}

				private static dynamic GetEmailToken(dynamic tokens, string key)
				{
					return Do.Until(() => tokens.FindBy(Key: key));
				}

				private static void WaitForFirstToken(dynamic tokens)
				{
					var sagaId = Drive.Data.OpsSagas.Db.HyphenBatchCashOutEntity.All().First().Id;

					Do.With.Interval(10).Timeout(1).Until<object>(
						() =>
							{
                                Drive.Msmq.BankGatewayHyphen.Send(new TimeoutMessage
								                       	{
								                       		SagaId = sagaId,
								                       		Expires = DateTime.UtcNow
								                       	});

								return tokens.FirstOrDefault();
							});
				}

				private static string GetEmailTemplateId(string emailTemplateName)
				{
					return Drive.Data.Ops.GetServiceConfiguration<string>(emailTemplateName);
				}
			}
		}
	}
}
