using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreTopUpConfirmationEmailResponse </summary>
    [XmlRoot("IWantToCreateAndStoreTopUpConfirmationEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreTopUpConfirmationEmailResponseEvent : MsmqMessage<IWantToCreateAndStoreTopUpConfirmationEmailResponseEvent>
    {
        public Guid TopUpId { get; set; }
        public Guid FileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
