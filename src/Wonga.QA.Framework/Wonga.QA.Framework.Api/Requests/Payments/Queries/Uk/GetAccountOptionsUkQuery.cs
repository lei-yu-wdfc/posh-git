using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk
{
	[XmlRoot("GetAccountOptions")]
	public partial class GetAccountOptionsUkQuery : ApiRequest<GetAccountOptionsUkQuery>
	{
		public Object AccountId { get; set; }
		public Object TrustRating { get; set; }
	}
}
