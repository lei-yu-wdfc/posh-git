using System.Globalization;
using System.Xml.Linq;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Framework.Mocks.Entities;

namespace Wonga.QA.Framework.Mocks
{
	public class ScotiaResponseBuilder
	{
		private readonly DbDriver _dbDriver;
		private long _bankAccountNumber;
		private decimal? _amount;

		public ScotiaResponseBuilder()
		{
			_dbDriver = new DbDriver();
		}

		public static ScotiaResponseBuilder New()
		{
			return new ScotiaResponseBuilder();
		}

		public ScotiaResponseBuilder ForBankAccountNumber(long bankAccountNumber)
		{
			_bankAccountNumber = bankAccountNumber;
			return this;
		}

		public ScotiaResponseBuilder ForAmount(decimal amount)
		{
			_amount = amount;
			return this;
		}

		public ScotiaResponse Reject(string message = "Rejected through QAF setup.")
		{
			var response = new XElement("Setup",
			                            new XElement("InputValidationTransaction",
			                                         new XAttribute("ResultCode", "R"),
			                                         new XAttribute("RejectMessage", message)));

			var entity = InsertSetup(response);

			return new ScotiaResponse(entity.BankGatewayResponseSetupId);
		}

		private BankGatewayResponseSetup InsertSetup(XElement response)
		{
			var setup = new BankGatewayResponseSetup
			            	{
			            		Amount = _amount,
			            		BankAccount = _bankAccountNumber.ToString(CultureInfo.InvariantCulture),
			            		Gateway = "Scotia",
			            		Persistent = false,
			            		Response = response.ToString(SaveOptions.DisableFormatting)
			            	};
			
			_dbDriver.QaData.BankGatewayResponseSetups.Insert(setup);
			_dbDriver.QaData.SubmitChanges();

			return setup;
		}
	}
}