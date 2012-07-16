using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.FileStorage.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;
using CreateFixedTermLoanTopupCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.CreateFixedTermLoanTopupCommand;

namespace Wonga.QA.Tests.Comms.Email
{

	[TestFixture]
	public class LoanTopUpCommsTests
	{

		Guid fixedTermLoanTopupId = Guid.NewGuid();
		const int topupAmount = 150;
		Guid customerId;
		private Application application;

		[FixtureSetUp]
		public void Setup()
		{

			Customer customer = CustomerBuilder.New().Build();
			customerId = customer.Id;

			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			application = ApplicationBuilder.New(customer).Build();
			Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
			{
				AccountId = customer.Id,
				ApplicationId = application.Id,
				FixedTermLoanTopupId = fixedTermLoanTopupId,
				TopupAmount = topupAmount
			});
		}

		[Test, AUT(AUT.Uk), JIRA("UK-789"), Parallelizable]
		public void CreateFixedTermLoanTopUpSecciTest()
		{
			var legalDocsTab = Drive.Data.Comms.Db.LegalDocuments;
			dynamic legalDocumentEntity = null;

			Assert.DoesNotThrow(() =>
					legalDocumentEntity = Do.Until(() => legalDocsTab.Find(
						legalDocsTab.ApplicationId == application.Id && legalDocsTab.DocumentType == 4)),
								"Topup SECCI LegalDocument not found");

			Guid fileId = (Guid)legalDocumentEntity.ExternalId;
			var filesTab = Drive.Data.FileStorage.Db.Files;

			Assert.DoesNotThrow(() =>
				  Do.Until(() => filesTab.Find(filesTab.FileId == fileId)),
				  "SECCI File {0} not found in files table.", fileId
				);

		}

		[Test, AUT(AUT.Uk), JIRA("UK-789"), Parallelizable]
		public void CreateFixedTermLoanTopUpAgreementTest()
		{

			var legDocsTab = Drive.Data.Comms.Db.LegalDocuments;
			dynamic legalDocumentEntity = null;

			Assert.DoesNotThrow(() => Do.Until(() =>
					legalDocumentEntity = legDocsTab.Find(legDocsTab.ApplicationId == application.Id &&
							legDocsTab.DocumentType == 5)),
							"Agreement LegalDocument not found");


			Guid fileId = (Guid)legalDocumentEntity.ExternalId;
			var filesTab = Drive.Data.FileStorage.Db.Files;

			Assert.DoesNotThrow(() =>
				  Do.Until(() => filesTab.Find(filesTab.FileId == fileId)),
				  "Topup agreement File {0} not found in files table.", fileId
				);

		}

		[Test, AUT(AUT.Uk), JIRA("UK-789"), Parallelizable]
		public void EmailFixedTermLoanTopUpAgreementTest()
		{
			var emailTopupAgreementTab = Drive.Data.OpsSagas.Db.EmailTopupAgreementEntity;
			Assert.DoesNotThrow(() =>
				Do.Until(() => emailTopupAgreementTab.Find(emailTopupAgreementTab.TopUpId == fixedTermLoanTopupId)),
				"Email TopUp Agreement Saga not in progress");

			Drive.Msmq.Comms.Send(new ILoanToppedUp
			{
				AccountId = customerId,
				ApplicationId = application.Id,
				CreatedOn = DateTime.UtcNow,
				TopupId = fixedTermLoanTopupId,
			});

			
			Assert.DoesNotThrow(() =>
				Do.Until(() => !emailTopupAgreementTab.FindAll(emailTopupAgreementTab.TopUpId == fixedTermLoanTopupId).Any()),
					 "Email TopUp Agreement Saga hasn't completed.");
			
		}

		[Test, AUT(AUT.Uk), JIRA("UK-789"), Parallelizable]
		public void GetLoanTopUpAgreementApiTest()
		{
			var legDocsTab = Drive.Data.Comms.Db.LegalDocuments;
			dynamic legalDocumentEntity = null;

			Assert.DoesNotThrow(() => Do.Until(() =>
					legalDocumentEntity = legDocsTab.Find(legDocsTab.ApplicationId == application.Id &&
							legDocsTab.DocumentType == 5)),
							"Agreement LegalDocument not found");


			Guid fileId = (Guid)legalDocumentEntity.ExternalId;
			var filesTab = Drive.Data.FileStorage.Db.Files;

			Assert.DoesNotThrow(() =>
				  Do.Until(() => filesTab.Find(filesTab.FileId == fileId)),
				  "Topup agreement File {0} not found in files table.", fileId
				);

			ApiResponse apiResponse = null;
			Assert.DoesNotThrow(() =>
				apiResponse =
				Drive.Api.Queries.Post(new GetLoanTopUpAgreementQuery { AccountId = customerId, FixedTermLoanTopupId = fixedTermLoanTopupId })
				, "Exception thrown by GetLoanTopUpAgreementQuery API call."
				);

			Assert.IsNotNull(apiResponse, "GetLoanTopUpAgreementQuery returned null");
			string agreementText = apiResponse.Values["AgreementContent"].FirstOrDefault();
			Assert.Contains(agreementText, "Fixed-Sum Loan Agreement regulated by the Consumer Credit Act 1974", "Agreement Header not found in document text.");
			Assert.Contains(agreementText, "&pound;" + topupAmount, "Topup amount not found in agreement text.");
		}
	}
}
