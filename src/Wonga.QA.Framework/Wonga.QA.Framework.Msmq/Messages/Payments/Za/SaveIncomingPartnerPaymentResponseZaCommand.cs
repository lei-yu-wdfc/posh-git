using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.Za
{
    /// <summary> Wonga.Payments.Za.SaveIncomingPartnerPaymentResponse </summary>
    [XmlRoot("SaveIncomingPartnerPaymentResponse", Namespace = "Wonga.Payments.Za", DataType = "")]
    public partial class SaveIncomingPartnerPaymentResponseZaCommand : MsmqMessage<SaveIncomingPartnerPaymentResponseZaCommand>
    {
        public Guid ApplicationId { get; set; }
        public String PaymentReference { get; set; }
        public String RawRequestResponse { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
