using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreScheduledPingFailedDocument </summary>
    [XmlRoot("IWantToCreateAndStoreScheduledPingFailedDocument", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreScheduledPingFailedDocument : MsmqMessage<IWantToCreateAndStoreScheduledPingFailedDocument>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
