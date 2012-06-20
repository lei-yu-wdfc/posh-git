using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreTopUpConfirmationEmail </summary>
    [XmlRoot("IWantToCreateAndStoreTopUpConfirmationEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreTopUpConfirmationEmailEvent : MsmqMessage<IWantToCreateAndStoreTopUpConfirmationEmailEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid TopUpId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
