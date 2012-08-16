using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetPaymentCardIsValid")]
	public partial class GetPaymentCardIsValidQuery : ApiRequest<GetPaymentCardIsValidQuery>
	{
		public Object PaymentCardId { get; set; }
	}
}
