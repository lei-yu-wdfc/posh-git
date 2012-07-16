using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendExtensionReminderEmailResponse </summary>
    [XmlRoot("IWantToSendExtensionReminderEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendExtensionReminderEmailResponse : MsmqMessage<IWantToSendExtensionReminderEmailResponse>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid FileId { get; set; }
        public Boolean IsSuccessful { get; set; }
    }
}
