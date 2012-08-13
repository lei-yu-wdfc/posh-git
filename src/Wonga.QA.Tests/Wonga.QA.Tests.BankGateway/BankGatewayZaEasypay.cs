﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
	[TestFixture, AUT(AUT.Za), Parallelizable(TestScope.All), Pending("Bankgateway service is locking db, can't clean up test files")]
	public class BankGatewayZaEasypay
	{
		private Application _application;

		private const string TEST_FILE1 = "easy5390.001";
		private const string TEST_FILE2 = "easy5390.002";
		private const string TEST_GATEWAY = "Easypay";

		private dynamic _filesTable = Drive.Data.BankGateway.Db.Files;
		private dynamic _acknowledgesTable = Drive.Data.BankGateway.Db.Acknowledges;
		private dynamic _incomingBankGatewayFileTable = Drive.Data.QaData.Db.IncomingBankGatewayFile;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			DeleteEasypayTestFie(TEST_FILE1);
			DeleteEasypayTestFie(TEST_FILE2);

			var customer = CustomerBuilder.New().Build();
			_application = ApplicationBuilder.New(customer).Build();
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			DeleteEasypayTestFie(TEST_FILE1);
			DeleteEasypayTestFie(TEST_FILE2);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2394"), Parallelizable(TestScope.All)]
		public void PartialRepaymentCreatesTransaction()
		{
			var app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == _application.Id);

			dynamic repaymentAccount = Drive.Data.Payments.Db.RepaymentAccount;
			var ra = Do.Until(() => repaymentAccount.FindAll(repaymentAccount.AccountId == _application.GetCustomer().Id)
													.FirstOrDefault());
			int fileSequence = 1;
			string repayNumber = ra.RepaymentNumber;
			string fileContent = CreateEasypayTestFile(DateTime.Now, fileSequence, 40.00M, 5.38m, repayNumber, "12345");

			dynamic incomingBankGatewayFile = new ExpandoObject();
			incomingBankGatewayFile.FileData = new ASCIIEncoding().GetBytes(fileContent);
			incomingBankGatewayFile.FileName = TEST_FILE1;
			incomingBankGatewayFile.Gateway = TEST_GATEWAY;
			//Act
			_incomingBankGatewayFileTable.Insert(incomingBankGatewayFile);				

			//Assert
			var transaction = Do.Until(() => Drive.Data.Payments.Db.Transactions.FindByApplicationIdAndAmount(ApplicationId: app.ApplicationId, Amount: -40M));
			Assert.AreEqual("Payment from EasyPay", transaction.Reference);
			Assert.AreEqual("DirectBankPayment", transaction.Type);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2394"), Parallelizable(TestScope.All), DependsOn("PartialRepaymentCreatesTransaction"), Pending]
		public void ProcessingFileCreatesAcknowledgements()
		{
			dynamic repaymentAccount = Drive.Data.Payments.Db.RepaymentAccount;
			var ra = Do.Until(() => repaymentAccount.FindAll(repaymentAccount.AccountId == _application.GetCustomer().Id)
										.FirstOrDefault());
			string repayNumber = ra.RepaymentNumber;

			//Assert
			var acknowledgedfile = Do.With.Timeout(20).Until(() => _filesTable.FindAll(_filesTable.FileName == TEST_FILE1).FirstOrDefault());
			Assert.IsNotNull(acknowledgedfile);
			Assert.AreEqual(TEST_FILE1, acknowledgedfile.FileName);

			var acknowledgeTransaction = Do.With.Timeout(20).Until(() => _acknowledgesTable.FindAll(_acknowledgesTable.FileName == TEST_FILE1)).ToList();
			Assert.AreEqual(1, acknowledgeTransaction.Count);
			Assert.AreEqual(TEST_FILE1, acknowledgeTransaction[0].FileName);
			Assert.AreEqual(repayNumber, acknowledgeTransaction[0].IncomingReference);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2394"), DependsOn("PartialRepaymentCreatesTransaction"), Parallelizable(TestScope.All), Pending]
		public void FullRepaymentClosesLoan()
		{
			//var app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == _application.Id);
			var balance = _application.GetBalanceToday();

			dynamic repaymentAccount = Drive.Data.Payments.Db.RepaymentAccount;
			var ra = Do.Until(() => repaymentAccount.FindAll(repaymentAccount.AccountId == _application.GetCustomer().Id)
													.FirstOrDefault());

			int fileSequence = 2;
			string repayNumber = ra.RepaymentNumber;
			string fileContent = CreateEasypayTestFile(DateTime.Now, fileSequence, balance, 5.38m, repayNumber, "12345");

			dynamic incomingBankGatewayFile = new ExpandoObject();
			incomingBankGatewayFile.FileData = new ASCIIEncoding().GetBytes(fileContent);
			incomingBankGatewayFile.FileName = TEST_FILE2;
			incomingBankGatewayFile.Gateway = TEST_GATEWAY;
			//Act
			_incomingBankGatewayFileTable.Insert(incomingBankGatewayFile);

			Do.Until(() => _application.IsClosed);
		}

		#region Helpers

		private void DeleteEasypayTestFie(string fileName)
		{
			if (_filesTable.FindAllByFileName(fileName).ToList().Count > 0)
				_filesTable.DeleteAll(_filesTable.FileName == fileName);

			if (_acknowledgesTable.FindAllByFileName(fileName).ToList().Count > 0)
				_acknowledgesTable.DeleteAll(_acknowledgesTable.FileName == fileName);

			if (_incomingBankGatewayFileTable.FindAll(_incomingBankGatewayFileTable.FileName == fileName).ToList().Count > 0)
				_incomingBankGatewayFileTable.DeleteAll(_incomingBankGatewayFileTable.FileName == fileName);
		}

		private string CreateEasypayTestFile(DateTime valueDate, int fileSequence, decimal repayAmount, decimal fees, string repaymentNumber, string accountNumber)
		{
			string fileContentTemplate =
										"01{6}{5}{18}{19}{12}093398\n" + //Header
										"40{6}{7}{11}{15}{13}098568\n" + //Transaction
										"50{6}{7}{0}+{1}+{3}138522\n" + //Payment
										"64{6}{7}{8}+{1}+{17}134192\n" + //Tender
										"99{9}+{2}+{4} {10}+{2}+{16}{14}121113\n";  //Trailer

			string fileVersion = "04";
			string receiverId = "5390";

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
																	bankCostStr.PadLeft(7, '0'),				//{17}
																	receiverId,									//{18}
																	fileVersion									//{19}
																	);
			return fileContent;
		}
		
		#endregion
	}
}