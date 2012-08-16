using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("SetPaymentCardPrimary")]
	public partial class SetPaymentCardPrimaryCommand : ApiRequest<SetPaymentCardPrimaryCommand>
	{
		public Object AccountId { get; set; }
		public Object PaymentCardId { get; set; }
	}
}
