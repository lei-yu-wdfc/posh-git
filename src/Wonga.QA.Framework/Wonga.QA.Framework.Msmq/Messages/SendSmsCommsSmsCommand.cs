using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Sms.SendSmsMessage </summary>
    [XmlRoot("SendSmsMessage", Namespace = "Wonga.Comms.InternalMessages.Sms", DataType = "")]
    public partial class SendSmsCommsSmsCommand : MsmqMessage<SendSmsCommsSmsCommand>
    {
        public String ToNumberFormatted { get; set; }
        public String MessageText { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
