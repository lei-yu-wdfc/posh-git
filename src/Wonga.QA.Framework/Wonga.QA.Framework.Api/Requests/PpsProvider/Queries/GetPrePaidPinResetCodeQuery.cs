using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
	[XmlRoot("GetPrePaidPinResetCode")]
	public partial class GetPrePaidPinResetCodeQuery : ApiRequest<GetPrePaidPinResetCodeQuery>
	{
		public Object AccountId { get; set; }
	}
}
