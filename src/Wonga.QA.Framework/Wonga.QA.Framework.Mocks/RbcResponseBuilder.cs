using System.Globalization;
using System.Xml.Linq;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Framework.Mocks.Entities;

namespace Wonga.QA.Framework.Mocks
{
	public class RbcResponseBuilder
	{
		private readonly DbDriver _dbDriver;
		private string _bankAccountNumber;
		private decimal? _amount;

        public RbcResponseBuilder()
		{
			_dbDriver = new DbDriver();
		}

        public static RbcResponseBuilder New()
		{
            return new RbcResponseBuilder();
		}

        public RbcResponseBuilder ForBankAccountNumber(string bankAccountNumber)
		{
			_bankAccountNumber = bankAccountNumber;
			return this;
		}

        public RbcResponseBuilder ForAmount(decimal amount)
		{
			_amount = amount;
			return this;
		}

		public RbcResponse Reject()
		{
			var response = new XElement("Setup",
                                        new XElement("ReportCreditRecord",
                                                     new XAttribute("PaymentStatus", "U")));

			var entity = InsertSetup(response);

            return new RbcResponse(entity.BankGatewayResponseSetupId);
		}

		private BankGatewayResponseSetup InsertSetup(XElement response)
		{
			var setup = new BankGatewayResponseSetup
			            	{
			            		Amount = _amount,
			            		BankAccount = _bankAccountNumber.ToString(CultureInfo.InvariantCulture),
			            		Gateway = "Rbc",
			            		Persistent = false,
			            		Response = response.ToString(SaveOptions.DisableFormatting)
			            	};
			
			_dbDriver.QaData.BankGatewayResponseSetups.Insert(setup);
			_dbDriver.QaData.SubmitChanges();

			return setup;
		}
	}
}