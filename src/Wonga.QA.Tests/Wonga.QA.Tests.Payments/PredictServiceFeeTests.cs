using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
	[Parallelizable(TestScope.Self)]
	public class PredictServiceFeeTests
	{
		private dynamic _applications = Drive.Data.Payments.Db.Applications;
		private dynamic _transactions = Drive.Data.Payments.Db.Transactions;
		private dynamic _debtCollections = Drive.Data.Payments.Db.DebtCollection;

		private Customer _30DayTermCustomer;
		private Application _30DayTermApplication;

		private Customer _40DayTermCustomer;
		private Application _40DayTermApplication;
		//private const int delay = 15000;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			//SetUtcNow(NowServiceConfigKey, new DateTime(2012, 03, 15, 13, 14, 15));
			_40DayTermCustomer = CustomerBuilder.New().Build();
			_40DayTermApplication = ApplicationBuilder.New(_40DayTermCustomer).WithExpectedDecision(ApplicationDecisionStatus.Accepted)
												.WithLoanAmount(1000)
												.WithLoanTerm(40)
												.Build();

			_30DayTermCustomer = CustomerBuilder.New().Build();
			_30DayTermApplication = ApplicationBuilder.New(_30DayTermCustomer).WithExpectedDecision(ApplicationDecisionStatus.Accepted)
												.WithLoanAmount(1000)
												.WithLoanTerm(30)
												.Build();


		}

		#region Standard Loan
		[Test, AUT(AUT.Za), JIRA("ZA-2634"), Parallelizable]
		public void NumberOfServiceFeePostedFor30DayLoan_Equals_1()
		{
			var loanApp = Do.Until(() => _applications.FindAllByExternalId(_30DayTermApplication.Id).Single());

			var numberOfServiceFeeTransactionsPosted = Do.Until(() => _transactions.FindAll(_transactions.ApplicationId == loanApp.ApplicationId &&
																			_transactions.Type == PaymentTransactionEnum.ServiceFee.ToString())
																			.Count());
			Assert.IsNotNull(loanApp);
			Assert.AreEqual(1, numberOfServiceFeeTransactionsPosted);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2634"), Parallelizable]
		public void TodaysBalanceFor30DayLoan_Equals_1228()
		{
			var loanApp = Do.Until(() => _applications.FindAllByExternalId(_30DayTermApplication.Id).Single());
			var todaysBalance = decimal.Parse(GetLoanAgreements(_30DayTermCustomer).Values["TodaysBalance"].SingleOrDefault());

			Assert.IsNotNull(loanApp);
			//1000	 CashAdvance
			//+57	 Fee1
			//+171	 InitFee
			Assert.AreEqual(1228m, todaysBalance);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2634"), Parallelizable]
		public void FinalBalanceFor30DayLoan_Equals_1282()
		{
			var loanApp = Do.Until(() => _applications.FindAllByExternalId(_30DayTermApplication.Id).Single());
			var finalBalance = decimal.Parse(GetLoanAgreements(_30DayTermCustomer).Values["FinalBalance"].SingleOrDefault());

			Assert.IsNotNull(loanApp);
			//1000	 CashAdvance
			//+57	 Fee1
			//+171	 InitFee
			//+54.5	 Interest
			Assert.AreEqual(1282.5m, finalBalance);
		} 
		#endregion

		[Test, AUT(AUT.Za), JIRA("ZA-2367"), Parallelizable]
		public void NumberOfServiceFeePostedFor40DayLoan_Equals_1()
		{
			var loanApp = Do.Until(() => _applications.FindAllByExternalId(_40DayTermApplication.Id).Single());

			var numberOfServiceFeeTransactionsPosted = Do.Until(() => _transactions.FindAll(_transactions.ApplicationId == loanApp.ApplicationId &&
																			_transactions.Type == PaymentTransactionEnum.ServiceFee.ToString())
																			.Count());
			Assert.IsNotNull(loanApp);
			Assert.AreEqual(1, numberOfServiceFeeTransactionsPosted);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2367"), Parallelizable]
		public void TodaysBalanceFor40DayLoan_Equals_1228()
		{
			var loanApp = Do.Until(() => _applications.FindAllByExternalId(_40DayTermApplication.Id).Single());
			var todaysBalance = decimal.Parse(GetLoanAgreements(_40DayTermCustomer).Values["TodaysBalance"].SingleOrDefault());

			Assert.IsNotNull(loanApp);
			//1000	 CashAdvance
			//+57	 Fee1
			//+171	 InitFee
			Assert.AreEqual(1228m, todaysBalance);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2367")]
		public void FinalBalanceFor40DayLoan_Equals_1360()
		{
			var loanApp = Do.Until(() => _applications.FindAllByExternalId(_40DayTermApplication.Id).Single());
			var finalBalance = decimal.Parse(GetLoanAgreements(_40DayTermCustomer).Values["FinalBalance"].SingleOrDefault());

			Assert.IsNotNull(loanApp);
			//1000	 CashAdvance
			//+57	 Fee1
			//+171	 InitFee
			//+57	 PredictedFee
			//+75.63 Interest
			Assert.AreEqual(1360.63m, finalBalance);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2367")]
		[DependsOn("FinalBalanceFor40DayLoan_Equals_1360")]
		public void Move40DayApplicationToDca__ShouldPost_SuspendServiceFeeTransaction_And_FinalBalanceEquals_1302()
		{
			var flagToDcaCommand = new Framework.Cs.FlagApplicationToDcaCommand
			{
				ApplicationId = _40DayTermApplication.Id
			};

			//Act
			Drive.Cs.Commands.Post(flagToDcaCommand);

			//Assert
			var debtCollection = Do.Until(() => _debtCollections.FindAll(_debtCollections.Applications.ExternalId == _40DayTermApplication.Id)
																			.OrderByDescending(_debtCollections.CreatedOn)
																			.FirstOrDefault());
			Assert.IsNotNull(debtCollection);
			Assert.AreEqual(true, debtCollection.MovedToAgency);


			var finalBalance = decimal.Parse(GetLoanAgreements(_40DayTermCustomer).Values["FinalBalance"].SingleOrDefault());

			var loanApp = Do.Until(() => _applications.FindAllByExternalId(_40DayTermApplication.Id).Single());
			var numberOfSuspendServiceFeeTransactionsPosted = Do.Until(() => _transactions.FindAll(_transactions.ApplicationId == loanApp.ApplicationId &&
																								   _transactions.Type == "SuspendServiceFee")
																									.Count());
			Assert.AreEqual(1, numberOfSuspendServiceFeeTransactionsPosted);

			//1000	 CashAdvance
			//+57	 Fee1
			//+171	 InitFee
			//+74.69 Interest
			Assert.AreEqual(1302.69m, finalBalance);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2367")]
		[DependsOn("Move40DayApplicationToDca__ShouldPost_SuspendServiceFeeTransaction_And_FinalBalanceEquals_1302")]
		public void Revoke40DayApplicationFromDca__ShouldPost_SuspendServiceFeeTransaction_And_FinalBalanceEquals_1360()
		{
			var revokeFromDcaCommand = new Framework.Cs.RevokeApplicationFromDcaCommand
			{
				ApplicationId = _40DayTermApplication.Id
			};

			//Act
			Drive.Cs.Commands.Post(revokeFromDcaCommand);

			var loanApp = Do.Until(() => _applications.FindAllByExternalId(_40DayTermApplication.Id).Single());
			var numberOfResumeServiceFeeTransactionsPosted = Do.Until(() => _transactions.FindAll(_transactions.ApplicationId == loanApp.ApplicationId &&
																			_transactions.Type == "ResumeServiceFee").FirstOrDefault());

			var finalBalance = decimal.Parse(GetLoanAgreements(_40DayTermCustomer).Values["FinalBalance"].SingleOrDefault());

			//1000	 CashAdvance
			//+57	 Fee1
			//+171	 InitFee
			//+57	 PredictedFee
			//+75.63 Interest
			Assert.AreEqual(1360.63m, finalBalance);
			Assert.IsNotNull(numberOfResumeServiceFeeTransactionsPosted);
		}

		private CsResponse GetLoanAgreements(Customer customer)
		{
			var query = new GetLoanAgreementsQuery() { AccountId = customer.Id, IsActive = null };
			return Drive.Cs.Queries.Post(query);
		}
	}
}