using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.SendCollectionsChaseSmsMessage </summary>
    [XmlRoot("SendCollectionsChaseSmsMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class SendCollectionsChaseSmsMessage : MsmqMessage<SendCollectionsChaseSmsMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public CollectionsChaseSmsEnum Type { get; set; }
    }
}
