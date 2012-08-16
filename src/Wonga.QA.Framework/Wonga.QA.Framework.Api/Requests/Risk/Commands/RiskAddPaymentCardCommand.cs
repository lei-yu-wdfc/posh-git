using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
	[XmlRoot("RiskAddPaymentCard")]
	public partial class RiskAddPaymentCardCommand : ApiRequest<RiskAddPaymentCardCommand>
	{
		public Object AccountId { get; set; }
		public Object PaymentCardId { get; set; }
		public Object CardType { get; set; }
		public Object Number { get; set; }
		public Object HolderName { get; set; }
		public Object StartDate { get; set; }
		public Object ExpiryDate { get; set; }
		public Object SecurityCode { get; set; }
	}
}
