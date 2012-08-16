using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("DeleteBankAccount")]
	public partial class DeleteBankAccountCommand : ApiRequest<DeleteBankAccountCommand>
	{
		public Object AccountId { get; set; }
		public Object BankAccountId { get; set; }
	}
}
