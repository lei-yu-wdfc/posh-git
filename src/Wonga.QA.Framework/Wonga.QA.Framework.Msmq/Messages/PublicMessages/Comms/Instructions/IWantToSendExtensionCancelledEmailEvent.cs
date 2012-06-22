using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendExtensionCancelledEmail </summary>
    [XmlRoot("IWantToSendExtensionCancelledEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendExtensionCancelledEmailEvent : MsmqMessage<IWantToSendExtensionCancelledEmailEvent>
    {
        public Guid SagaId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public String EMailAddress { get; set; }
        public String Forename { get; set; }
    }
}
