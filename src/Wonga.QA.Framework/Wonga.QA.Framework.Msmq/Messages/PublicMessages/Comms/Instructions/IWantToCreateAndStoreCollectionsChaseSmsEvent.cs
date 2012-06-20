using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Comms.InternalMessages;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreCollectionsChaseSms </summary>
    [XmlRoot("IWantToCreateAndStoreCollectionsChaseSms", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreCollectionsChaseSmsEvent : MsmqMessage<IWantToCreateAndStoreCollectionsChaseSmsEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public CollectionsChaseSmsEnum Type { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
