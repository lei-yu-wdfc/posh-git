using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.Za.SaveIncomingPartnerPaymentResponse </summary>
    [XmlRoot("SaveIncomingPartnerPaymentResponse")]
    public partial class SaveIncomingPartnerPaymentResponseZaCommand : ApiRequest<SaveIncomingPartnerPaymentResponseZaCommand>
    {
        public Object ApplicationId { get; set; }
        public Object PaymentReference { get; set; }
        public Object RawRequestResponse { get; set; }
    }
}
