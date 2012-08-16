using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("SetBankAccountPrimary")]
	public partial class SetBankAccountPrimaryCommand : ApiRequest<SetBankAccountPrimaryCommand>
	{
		public Object AccountId { get; set; }
		public Object BankAccountId { get; set; }
	}
}
