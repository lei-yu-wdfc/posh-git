using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
	[XmlRoot("GetPrepaidAvailableAccountBalance")]
	public partial class GetPrepaidAvailableAccountBalanceQuery : ApiRequest<GetPrepaidAvailableAccountBalanceQuery>
	{
		public Object AccountId { get; set; }
	}
}
