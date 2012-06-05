using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Payments.Za.IWantToVerifyPayUTransactionResponse </summary>
    [XmlRoot("IWantToVerifyPayUTransactionResponse", Namespace = "Wonga.PublicMessages.Payments.Za", DataType = "")]
    public partial class IWantToVerifyPayUTransactionResponseZaEvent : MsmqMessage<IWantToVerifyPayUTransactionResponseZaEvent>
    {
        public Int32 PaymentId { get; set; }
        public String PaymentReferenceNumber { get; set; }
        public PayUTransactionResultEnum Result { get; set; }
        public DateTime DateProcessed { get; set; }
        public String RawResponse { get; set; }
    }
}
