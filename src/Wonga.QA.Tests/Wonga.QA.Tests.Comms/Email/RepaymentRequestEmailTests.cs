using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Email
{
	[TestFixture, Parallelizable(TestScope.All)]
	public class RepaymentRequestEmailTests
	{
		private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-Za");

		private readonly dynamic _emailTable = Drive.Data.QaData.Db.Email;
		private readonly dynamic _emailTokenTable = Drive.Data.QaData.Db.EmailToken;
		private const string TemplateName = "34156";

		[Test, AUT(AUT.Za), JIRA("ZA-2099", "ZA-2188")]
		public void WhenEarlyPartialRepaymentIsSetUpPartialEmailIsSent()
		{
			const decimal loanAmount = 1000m;
			const int loanTerm = 30;
			const decimal repayAmount = 900m;
			DateTime actionDate = DateTime.Now.AddDays(20.0).GetNextWorkingDay();

			var customer = CustomerBuilder.New().Build();
			var application = BuildApplication(loanAmount, loanTerm, customer);

			MakeEarlyRepayment(application, repayAmount, actionDate);

			VerifyCorrectEmailIsSent(customer, actionDate, repayAmount, true);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2099", "ZA-2188")]
		public void WhenEarlyFullRepaymentIsSetUpFullEmailIsSent()
		{
			const decimal loanAmount = 1000m;
			const int loanTerm = 30;
			const decimal repayAmount = 1280m;
			DateTime actionDate = DateTime.Now.AddDays(20.0).GetNextWorkingDay();

			var customer = CustomerBuilder.New().Build();
			var application = BuildApplication(loanAmount, loanTerm, customer);

			MakeEarlyRepayment(application, repayAmount, actionDate);

			VerifyCorrectEmailIsSent(customer, actionDate, repayAmount, false);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2358")]
		public void WhenEarlyPartialRepaymentGreaterOrEqualToPrincipalIsSetUpPartialEmailIsSent()
		{
			const decimal loanAmount = 1000m;
			const int loanTerm = 30;
			const decimal repayAmount = 1100m;
			DateTime actionDate = DateTime.Now.AddDays(20.0).GetNextWorkingDay();

			var customer = CustomerBuilder.New().Build();
			var application = BuildApplication(loanAmount, loanTerm, customer);

			MakeEarlyRepayment(application, repayAmount, actionDate);

			VerifyCorrectEmailIsSent(customer, actionDate, repayAmount, true);
		}

		private static Application BuildApplication(decimal loanAmount, int loanTerm, Customer customer)
		{
			return ApplicationBuilder.New(customer)
				.WithLoanTerm(loanTerm)
				.WithLoanAmount(loanAmount)
				.Build();
		}

		private static void MakeEarlyRepayment(Application application, decimal amount, DateTime onDate)
		{
			var command = new RepayLoanViaBankCommand
			              	{
			              		ApplicationId = application.Id,
			              		ActionDate = new Date
			              		             	{
			              		             		DateTime = onDate,
			              		             		DateFormat = DateFormat.Date
			              		             	},
			              		Amount = amount,
			              		RepaymentRequestId = Guid.NewGuid()
			              	};
			Drive.Api.Commands.Post(command);
		}

		private void VerifyCorrectEmailIsSent(Customer customer, DateTime actionDate, decimal repayAmount, bool partial)
		{
			var email = Do.Until(() => _emailTable.FindBy(EmailAddress: customer.Email, TemplateName: TemplateName));
			Assert.IsNotNull(email);

			var htmlToken = _emailTokenTable.FindBy(EmailId: email.EmailId, Key: "Html_body");
			Assert.IsNotNull(htmlToken);

			var plainToken = _emailTokenTable.FindBy(EmailId: email.EmailId, Key: "Plain_body");
			Assert.IsNotNull(plainToken);

			string plainSubstring =
				string.Format(Culture,
				              partial
				              	? "partial repayment of {0:C2} on {1:D}"
				              	: "single repayment of {0:C2} on {1:D}",
				              repayAmount,
				              actionDate);
			Assert.Contains(plainToken.Value, plainSubstring);

			string htmlSubstring =
				string.Format(Culture,
				              partial
				              	? "partial repayment of <strong>{0:C2}</strong> on <strong>{1:D}</strong>"
				              	: "single repayment of <strong>{0:C2}</strong> on <strong>{1:D}</strong>",
				              repayAmount,
				              actionDate);
			Assert.Contains(htmlToken.Value, htmlSubstring);
		}
	}
}
