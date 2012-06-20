using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStorePreTopUpDocument </summary>
    [XmlRoot("IWantToCreateAndStorePreTopUpDocument", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStorePreTopUpDocumentEvent : MsmqMessage<IWantToCreateAndStorePreTopUpDocumentEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid TopupId { get; set; }
        public Guid SagaId { get; set; }
    }
}
