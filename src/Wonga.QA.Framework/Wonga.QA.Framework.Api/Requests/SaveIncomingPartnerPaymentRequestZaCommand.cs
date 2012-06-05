using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
	/// <summary> Wonga.Payments.Commands.Za.SaveIncomingPartnerPaymentRequest </summary>
	[XmlRoot("SaveIncomingPartnerPaymentRequest")]
	public partial class SaveIncomingPartnerPaymentRequestZaCommand : ApiRequest<SaveIncomingPartnerPaymentRequestZaCommand>
	{
		public Object ApplicationId { get; set; }
		public Object PaymentReference { get; set; }
		public Object TransactionAmount { get; set; }
		public Object RequestedOn { get; set; }
	}
}
