using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetPaymentCards")]
	public partial class GetPaymentCardsQuery : ApiRequest<GetPaymentCardsQuery>
	{
		public Object AccountId { get; set; }
	}
}
