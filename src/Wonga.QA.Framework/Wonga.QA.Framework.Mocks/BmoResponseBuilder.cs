using System.Globalization;
using System.Xml.Linq;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Framework.Mocks.Entities;

namespace Wonga.QA.Framework.Mocks
{
	public class BmoResponseBuilder
	{
		private readonly DbDriver _dbDriver;
		private long _bankAccountNumber;
		private decimal? _amount;

		public BmoResponseBuilder()
		{
			_dbDriver = new DbDriver();
		}

		public static BmoResponseBuilder New()
		{
			return new BmoResponseBuilder();
		}

		public BmoResponseBuilder ForBankAccountNumber(long bankAccountNumber)
		{
			_bankAccountNumber = bankAccountNumber;
			return this;
		}

		public BmoResponseBuilder ForAmount(decimal amount)
		{
			_amount = amount;
			return this;
		}

		public BmoResponse RejectFile(string message = "Rejected through QAF setup.")
		{
			var response = new XElement("Setup",
				new XElement("AcknowledgeFile",
					new XAttribute("Accepted", "false")),
				new XElement("SettlementReportDetail", new XAttribute("LogicalRecordNumber", -1)));

			var entity = InsertSetup(response);

			return new BmoResponse(entity.BankGatewayResponseSetupId);
		}

		public BmoResponse RejectTransaction(string message = "Rejected through QAF setup.")
		{
			var response = new XElement("Setup",
				new XElement("RejectedTransactionsReportDetail"),
				new XElement("SettlementReportDetail", new XAttribute("LogicalRecordNumber", -1)));

			var entity = InsertSetup(response);

			return new BmoResponse(entity.BankGatewayResponseSetupId);
		}

		private BankGatewayResponseSetup InsertSetup(XElement response)
		{
			var setup = new BankGatewayResponseSetup
			            	{
			            		Amount = _amount,
			            		BankAccount = _bankAccountNumber.ToString(CultureInfo.InvariantCulture),
			            		Gateway = "Bmo",
			            		Persistent = false,
			            		Response = response.ToString(SaveOptions.DisableFormatting)
			            	};
			
			_dbDriver.QaData.BankGatewayResponseSetups.Insert(setup);
			_dbDriver.QaData.SubmitChanges();

			return setup;
		}
	}
}