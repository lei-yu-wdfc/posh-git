using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetBankAccounts")]
	public partial class GetBankAccountsQuery : ApiRequest<GetBankAccountsQuery>
	{
		public Object AccountId { get; set; }
	}
}
