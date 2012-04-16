using System.Globalization;
using System.Xml.Linq;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Framework.Mocks.Entities;

namespace Wonga.QA.Framework.Mocks
{
	public class BmoSetupBuilder
	{
		private readonly DbDriver _dbDriver;
		private long _bankAccountNumber;
		private decimal? _amount;

		public BmoSetupBuilder()
		{
			_dbDriver = new DbDriver();
		}

		public static BmoSetupBuilder New()
		{
			return new BmoSetupBuilder();
		}

		public BmoSetupBuilder ForBankAccountNumber(long bankAccountNumber)
		{
			_bankAccountNumber = bankAccountNumber;
			return this;
		}

		public BmoSetupBuilder ForAmount(decimal amount)
		{
			_amount = amount;
			return this;
		}

		public BmoSetup Reject(string message = "Rejected through QAF setup.")
		{
			var response = new XElement("Setup",
			                            new XElement("InputValidationTransaction",
			                                         new XAttribute("ResultCode", "R"),
			                                         new XAttribute("RejectMessage", message)));

			var entity = InsertSetup(response);

			return new BmoSetup(entity.BankGatewayResponseSetupId);
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