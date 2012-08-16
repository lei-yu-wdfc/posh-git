using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
	[XmlRoot("GetCurrentAddress")]
	public partial class GetCurrentAddressQuery : ApiRequest<GetCurrentAddressQuery>
	{
		public Object AccountId { get; set; }
	}
}
