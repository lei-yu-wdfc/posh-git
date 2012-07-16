using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreExtensionReminderEmail </summary>
    [XmlRoot("IWantToCreateAndStoreExtensionReminderEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreExtensionReminderEmail : MsmqMessage<IWantToCreateAndStoreExtensionReminderEmail>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
