using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
	[XmlRoot("GetReviewDetails")]
	public partial class GetReviewDetailsQuery : ApiRequest<GetReviewDetailsQuery>
	{
		public Object AccountId { get; set; }
	}
}
