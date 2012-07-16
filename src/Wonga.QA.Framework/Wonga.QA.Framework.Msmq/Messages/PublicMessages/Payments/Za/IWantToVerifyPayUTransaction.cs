using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.Za
{
    /// <summary> Wonga.PublicMessages.Payments.Za.IWantToVerifyPayUTransaction </summary>
    [XmlRoot("IWantToVerifyPayUTransaction", Namespace = "Wonga.PublicMessages.Payments.Za", DataType = "")]
    public partial class IWantToVerifyPayUTransaction : MsmqMessage<IWantToVerifyPayUTransaction>
    {
        public Guid SafeKey { get; set; }
        public Int32 PaymentId { get; set; }
        public String PaymentReferenceNumber { get; set; }
    }
}
