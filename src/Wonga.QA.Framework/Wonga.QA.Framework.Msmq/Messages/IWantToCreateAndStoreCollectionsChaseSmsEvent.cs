using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
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
