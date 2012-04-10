using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.SendCollectionsChaseSmsMessage </summary>
    [XmlRoot("SendCollectionsChaseSmsMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class SendCollectionsChaseSmsCommand : MsmqMessage<SendCollectionsChaseSmsCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public CollectionsChaseSmsEnum Type { get; set; }
    }
}
