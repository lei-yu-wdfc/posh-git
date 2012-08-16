using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk
{
	[XmlRoot("RiskAddBankAccount")]
	public partial class RiskAddBankAccountUkCommand : ApiRequest<RiskAddBankAccountUkCommand>
	{
		public Object AccountId { get; set; }
		public Object BankAccountId { get; set; }
		public Object BankName { get; set; }
		public Object AccountNumber { get; set; }
	}
}
