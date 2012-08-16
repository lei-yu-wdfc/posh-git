using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetPersonalPromoCode")]
	public partial class GetPersonalPromoCodeQuery : ApiRequest<GetPersonalPromoCodeQuery>
	{
		public Object AccountId { get; set; }
	}
}
