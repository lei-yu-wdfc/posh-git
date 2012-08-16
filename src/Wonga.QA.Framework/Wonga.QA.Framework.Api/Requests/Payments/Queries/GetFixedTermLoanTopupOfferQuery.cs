using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetFixedTermLoanTopupOffer")]
	public partial class GetFixedTermLoanTopupOfferQuery : ApiRequest<GetFixedTermLoanTopupOfferQuery>
	{
		public Object AccountId { get; set; }
	}
}
