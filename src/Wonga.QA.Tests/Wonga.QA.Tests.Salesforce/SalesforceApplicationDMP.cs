﻿using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
﻿using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
﻿using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
﻿using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
﻿using Wonga.QA.Framework.Old;
﻿using Wonga.QA.Tests.Core;
using salesforceStatusAlias = Wonga.QA.Framework.ThirdParties.Salesforce.ApplicationStatus;
﻿using Owner = Wonga.QA.Tests.Core.Owner;

namespace Wonga.QA.Tests.Salesforce
{
	[TestFixture(Order = -1),Pending("Ticket Not implemented")]
	[Parallelizable(TestScope.Self)]
	public class SalesforceApplicationDMP
	{
		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
		public void LiveApplicationDMP()
		{
			Guid.NewGuid();
			var cust=CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(cust).Build(); 
			DMP(application);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
		public void DueTodayApplicationDMP()
		{
			Guid.NewGuid();
			var application = CreateLiveApplication();
			SalesforceOperations.RewindDatesToMakeDueToday(application);
			SalesforceOperations.MakeDueToday(application);
			SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DueToday);
			DMP(application);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
		public void ArrearApplicationDMP()
		{
			Guid.NewGuid();
			var application = CreateLiveApplication();
			application.PutIntoArrears(3);
			SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.InArrears);
			DMP(application);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
		public void SuspectFraudApplicationDMP()
		{
			var caseId = Guid.NewGuid();
			var customer = CreateCustomer();
			var application = SalesforceOperations.CreateApplication(customer);
			ApplicationOperations.SuspectFraud(application, customer, caseId);
			SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Fraud);
			DMP(application);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni), Pending("DCA not implemented")]
		public void DCAApplicationDMP()
		{
			Guid.NewGuid();
			var application = CreateLiveApplication();
			ApplicationOperations.Dca(application);
			SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DCA);
			DMP(application);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
		public void HardshipApplicationComplaintCycle()
		{
			var caseId = Guid.NewGuid();
			var application = CreateLiveApplication();
			ApplicationOperations.ReportHardship(application, caseId);
			SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Hardship);
			DMP(application);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-133"), Owner(Owner.AnilKrishnamaneni)]
		public void ComplaintApplicationHardshipCycle()
		{
			var caseId = Guid.NewGuid();
			var application = CreateLiveApplication();
			ApplicationOperations.ReportComplaint(application, caseId);
			SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.Complaint);
			DMP(application);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-163"), Owner(Owner.AnilKrishnamaneni)]
		public void ManagementReviewApplicationDMP()
		{
			var caseId = Guid.NewGuid();
			var application = CreateLiveApplication();
			ApplicationOperations.ManagementReview(application, caseId);
			SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.ManagementReview);
			DMP(application);
		}

		#region Helpers#

		private Application CreateLiveApplication()
		{
			var customer = CreateCustomer();
			Application application = SalesforceOperations.CreateApplication(customer);
			return application;
		}

		private static Customer CreateCustomer()
		{
			return CustomerBuilder.New().Build();
		}

		private void DMP(Application application)
		{
			
			Drive.Cs.Commands.Post(new CsCreateDmpRepaymentArrangementCommand()
			                       	{
			                       		ApplicationId = application.Id,
			                       		AccountId = application.AccountId,
			                       		RepaymentAmount = 100.02m,
			                       		ArrangementDetails = ArrangementDetails()
			                       	});

			SalesforceOperations.CheckSalesApplicationStatus(application, (double)salesforceStatusAlias.DMPRepaymentArrangement);
		}

		private static RepaymentArrangementDetailCsapi[] ArrangementDetails()
		{
			return new[]
			       	{
			       		new RepaymentArrangementDetailCsapi
			       			{Amount = 49.01m, Currency = CurrencyCodeIso4217Enum.GBP, DueDate = DateTime.Today},
			       		new RepaymentArrangementDetailCsapi
			       			{
			       				Amount = 51.01m,Currency = CurrencyCodeIso4217Enum.GBP,DueDate = DateTime.Today.AddDays(7)
			       			}
			       	};
		}

		//Needed for serialization in CreateExtendedRepaymentArrangementCommand
		public class RepaymentArrangementDetailCsapi
		{
			public decimal Amount { get; set; }
			public CurrencyCodeIso4217Enum Currency { get; set; }
			public DateTime DueDate { get; set; }
		}
		#endregion helpers#
	}
}
