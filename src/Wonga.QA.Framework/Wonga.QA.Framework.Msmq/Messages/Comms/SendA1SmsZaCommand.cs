using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SendA1SmsMessage", Namespace = "Wonga.Comms.InternalMessages.Za", DataType = "")]
    public class SendA1SmsZaCommand : MsmqMessage<SendA1SmsZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid? LastRecordedPaymentTransactionId { get; set; }
    }
}
