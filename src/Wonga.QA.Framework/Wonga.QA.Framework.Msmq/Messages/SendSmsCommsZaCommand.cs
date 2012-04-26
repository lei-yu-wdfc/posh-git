using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Za.SendSmsMessage </summary>
    [XmlRoot("SendSmsMessage", Namespace = "Wonga.Comms.InternalMessages.Za", DataType = "")]
    public partial class SendSmsCommsZaCommand : MsmqMessage<SendSmsCommsZaCommand>
    {
        public String FormattedPhoneNumber { get; set; }
        public String MessageText { get; set; }
        public Guid AccountId { get; set; }
        public String SalesforceActivityType { get; set; }
        public String SalesforceActivitySubject { get; set; }
    }
}
