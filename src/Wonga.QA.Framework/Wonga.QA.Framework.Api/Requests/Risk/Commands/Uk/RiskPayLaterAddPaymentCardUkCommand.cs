using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
	[XmlRoot("RiskPayLaterAddPaymentCard")]
	public partial class RiskPayLaterAddPaymentCardUkCommand : ApiRequest<RiskPayLaterAddPaymentCardUkCommand>
	{
		public Object AccountId { get; set; }
		public Object PaymentCardId { get; set; }
		public Object CardType { get; set; }
		public Object Number { get; set; }
		public Object ExpiryDate { get; set; }
		public Object SecurityCode { get; set; }
	}
}
