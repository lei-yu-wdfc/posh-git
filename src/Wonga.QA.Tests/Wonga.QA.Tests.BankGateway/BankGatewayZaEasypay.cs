using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
	[TestFixture, AUT(AUT.Za), Parallelizable(TestScope.All)]
	public class BankGatewayZaEasypay
	{
		private const string TEST_FILE1 = "easy5390.001";
		private const string TEST_FILE2 = "easy5390.002";
		private const string TEST_FILE3 = "easy5390.003";
		private const string TEST_GATEWAY = "Easypay";

		private dynamic _filesTable = Drive.Data.BankGateway.Db.Files;
		private dynamic _acknowledgesTable = Drive.Data.BankGateway.Db.Acknowledges;
		private dynamic _bankIntegrationsTable = Drive.Data.BankGateway.Db.BankIntegrations;

		private int _pollingInterval;
		private const string BANK_INT_DESC = "Easypay";

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			//Clear Test Data
			_filesTable.DeleteAll(_filesTable.FileName == TEST_FILE1);
			_acknowledgesTable.DeleteAll(_acknowledgesTable.FileName == TEST_FILE1);

			_filesTable.DeleteAll(_filesTable.FileName == TEST_FILE2);
			_acknowledgesTable.DeleteAll(_acknowledgesTable.FileName == TEST_FILE2);

			_filesTable.DeleteAll(_filesTable.FileName == TEST_FILE3);
			_acknowledgesTable.DeleteAll(_acknowledgesTable.FileName == TEST_FILE3);

			//Make test run faster with min polling interval
			var bankIntegration = _bankIntegrationsTable.FindByDescription(BANK_INT_DESC);
			_pollingInterval = bankIntegration.PollingInterval;
			bankIntegration.PollingInterval = 10;

			_bankIntegrationsTable.Update(bankIntegration);
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			//Reset polling interval
			var bankIntegration = _bankIntegrationsTable.FindByDescription(BANK_INT_DESC);
			bankIntegration.PollingInterval = _pollingInterval;

			_bankIntegrationsTable.Update(bankIntegration);			
		}

		[SetUp]
		public void SetUp()
		{
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2394")]
		public void ProcessEasypayFile_WillCreate_Acknowledgements()
		{
			//Arrange	
			string fileContent =
							"0120120417000001539004                                                    093398\n" + //Header
							"4020120417082909006001007036034 0                                         098568\n" + //Transaction
							"50201204170829090000000000000000000000953901098645686240+000004000+0000538138522\n" + //Payment
							"64201204170829090000000000000000000000000000000000012345+000004000+0000000134192\n" + //Tender
							"99000001+00000004000+00000000538 000001+00000004000+00000000000           121113\n";  //Trailer

			IncomingBankGatewayFile incomingBankGatewayFile = new IncomingBankGatewayFile();
			incomingBankGatewayFile.FileData = new ASCIIEncoding().GetBytes(fileContent);
			incomingBankGatewayFile.FileName = TEST_FILE1;
			incomingBankGatewayFile.Gateway = TEST_GATEWAY;

			//Act
			Drive.Db.QaData.IncomingBankGatewayFiles.InsertOnSubmit(incomingBankGatewayFile);
			incomingBankGatewayFile.Submit();

			//Assert
			var acknowledgedfile = Do.With.Timeout(20).Until(() => _filesTable.FindAll(_filesTable.FileName == TEST_FILE1).FirstOrDefault());
			Assert.IsNotNull(acknowledgedfile);
			Assert.AreEqual(TEST_FILE1, acknowledgedfile.FileName);

			var acknowledgeTransaction = Do.With.Timeout(20).Until(() => _acknowledgesTable.FindAll(_acknowledgesTable.FileName == TEST_FILE1)).ToList();
			Assert.AreEqual(1, acknowledgeTransaction.Count);
			Assert.AreEqual(TEST_FILE1, acknowledgeTransaction[0].FileName);
			Assert.AreEqual("953901098645686240", acknowledgeTransaction[0].IncomingReference);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2394")]
		public void ProcessEasypayFile_PartialRepayment_WillCreate_DirectBankPaymentTransaction()
		{
			//Arrange
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			var app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);

			//Request to generate easypay number via csapi
			var generateRepaymentNumberCommand = new Framework.Cs.GenerateRepaymentNumberCommand
			{
				AccountId = customer.Id
			};

			Drive.Cs.Commands.Post(generateRepaymentNumberCommand);

			dynamic repaymentAccount = Drive.Data.Payments.Db.RepaymentAccount;
			var ra = Do.Until(() => repaymentAccount.FindAll(repaymentAccount.AccountId == customer.Id)
													.FirstOrDefault());

			string fileContentTemplate =
						"0120120417000002539004                                                    093398\n" + //Header
						"4020120417082909006001007036034 0                                         098568\n" + //Transaction
						"502012041708290900000000000000000000{0}+000004000+0000538138522\n" + //Payment
						"64201204170829090000000000000000000000000000000000012345+000004000+0000000134192\n" + //Tender
						"99000001+00000004000+00000000538 000001+00000004000+00000000000           121113\n";  //Trailer

			string fileContent = string.Format(fileContentTemplate, ra.RepaymentNumber);

			IncomingBankGatewayFile incomingBankGatewayFile = new IncomingBankGatewayFile();

			incomingBankGatewayFile.FileData = new ASCIIEncoding().GetBytes(fileContent);
			incomingBankGatewayFile.FileName = TEST_FILE2;
			incomingBankGatewayFile.Gateway = TEST_GATEWAY;

			//Act
			Drive.Db.QaData.IncomingBankGatewayFiles.InsertOnSubmit(incomingBankGatewayFile);
			incomingBankGatewayFile.Submit();

			//Assert
			var transaction = Do.With.Timeout(20).Until(() => Drive.Db.Payments.Transactions.Single(r => r.Amount == -40M && 
																										 r.ApplicationId == app.ApplicationId));
			Assert.AreEqual("Payment from EasyPay", transaction.Reference);
			Assert.AreEqual("DirectBankPayment", transaction.Type);

		}

		[Test, AUT(AUT.Za), JIRA("ZA-2394")]
		public void ProcessEasypayFile_RepayFullAmount_WillCreate_DirectBankPaymentTransaction_AND_CloseApplication()
		{
			//Arrange
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			var app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);

			//Request to generate easypay number via csapi
			var generateRepaymentNumberCommand = new Framework.Cs.GenerateRepaymentNumberCommand
			{
				AccountId = customer.Id
			};

			Drive.Cs.Commands.Post(generateRepaymentNumberCommand);

			dynamic repaymentAccount = Drive.Data.Payments.Db.RepaymentAccount;
			var ra = Do.Until(() => repaymentAccount.FindAll(repaymentAccount.AccountId == customer.Id)
													.FirstOrDefault());

			string fileContentTemplate =
			"0120120417{5}539004                                                    093398\n" + //Header
			"4020120417082909006001007036034 0                                         098568\n" + //Transaction
			"5020120417082909{0}+{1}+{3}138522\n" + //Payment
			"64201204170829090000000000000000000000000000000000012345+{1}+0000000134192\n" + //Tender
			"99000001+{2}+{4} 000001+{2}+00000000000           121113\n";  //Trailer

			string fileSequence = "000003";
			string fees = "538";
			string repayAmount = "64250";
			string repaymentNumber = ra.RepaymentNumber;

			string fileContent = string.Format(fileContentTemplate, repaymentNumber.PadLeft(40, '0'),		//{0}
																	repayAmount.PadLeft(9, '0'),			//{1}
																	repayAmount.PadLeft(11, '0'),			//{2}
																	fees.PadLeft(7, '0'),					//{3}
																	fees.PadLeft(11, '0'),					//{4}
																	fileSequence.PadLeft(6,'0')				//{5}
																	);

			IncomingBankGatewayFile incomingBankGatewayFile = new IncomingBankGatewayFile();

			incomingBankGatewayFile.FileData = new ASCIIEncoding().GetBytes(fileContent);
			incomingBankGatewayFile.FileName = TEST_FILE3;
			incomingBankGatewayFile.Gateway = TEST_GATEWAY;

			//Act
			Drive.Db.QaData.IncomingBankGatewayFiles.InsertOnSubmit(incomingBankGatewayFile);
			incomingBankGatewayFile.Submit();

			app =  Do.With.Timeout(20).Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id && a.ClosedOn != null));

			Assert.IsNotNull(app);

		}

		private string CreateEasypayTestFile(DateTime valueDate, int fileSequence, decimal repayAmount, decimal fees, string repaymentNumber, string accountNumber)
		{
			string fileContentTemplate =
										"01{6}{5}539004{12}093398\n" + //Header
										"40{6}{7}{11}{15}{13}098568\n" + //Transaction
										"50{6}{7}{0}+{1}+{3}138522\n" + //Payment
										"64{6}{7}{8}+{1}+{17}134192\n" + //Tender
										"99{9}+{2}+{4} {10}+{2}+{16}{14}121113\n";  //Trailer

			string valueDateStr = valueDate.ToString("yyyyMMdd");
			string timeStr = valueDate.ToString("HHmmss");

			string repayAmountStr = repayAmount.ToString();
			repayAmountStr = Regex.Replace(repayAmountStr, @"\.", String.Empty);

			string feesStr = fees.ToString();
			feesStr = Regex.Replace(feesStr, @"\.", String.Empty);

			string fileSequenceStr = fileSequence.ToString();

			int numberOfPayments = 1;
			int numberOfTenders = 1;

			string collector = "6001007036034";
			collector = collector.PadLeft(15, '0');
			collector = collector.PadRight(16, ' ');

			string POS = "0022";
			string filler1 = "";
			string filler2 = "";
			string filler3 = "";

			decimal bankCost = 0.00m;
			string bankCostStr = bankCost.ToString();
			bankCostStr = Regex.Replace(bankCostStr, @"\.", String.Empty);

			decimal bankCostTotal = 0.00m;
			string bankCostTotalStr = bankCostTotal.ToString();
			bankCostTotalStr = Regex.Replace(bankCostTotalStr, @"\.", String.Empty);

			string fileContent = string.Format(fileContentTemplate, repaymentNumber.PadLeft(40, '0'),			//{0}
																	repayAmountStr.PadLeft(9, '0'),				//{1}
																	repayAmountStr.PadLeft(11, '0'),			//{2}
																	feesStr.PadLeft(7, '0'),					//{3}
																	feesStr.PadLeft(11, '0'),					//{4}
																	fileSequenceStr.PadLeft(6, '0'),			//{5}
																	valueDateStr,								//{6}	20120417
																	timeStr,									//{7}	082909
																	accountNumber.PadLeft(40, '0'),				//{8}   12345
																	numberOfPayments.ToString().PadLeft(6, '0'),	//{9}
																	numberOfTenders.ToString().PadLeft(6, '0'),	//{10}
																	collector,									//{11}
																	filler1.PadLeft(52, ' '),					//{12}
																	filler2.PadLeft(34, ' '),					//{13}
																	filler3.PadLeft(11, ' '),					//{14}
																	POS.PadRight(8, ' '),						//{15}
																	bankCostTotalStr.PadLeft(11, '0'),			//{16}
																	bankCostStr.PadLeft(7, '0')					//{17}
																	);
			return fileContent;
		}

	}
}