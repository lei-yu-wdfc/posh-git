using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
	[XmlRoot("GetCustomerDetails")]
	public partial class GetCustomerDetailsQuery : ApiRequest<GetCustomerDetailsQuery>
	{
		public Object AccountId { get; set; }
	}
}
