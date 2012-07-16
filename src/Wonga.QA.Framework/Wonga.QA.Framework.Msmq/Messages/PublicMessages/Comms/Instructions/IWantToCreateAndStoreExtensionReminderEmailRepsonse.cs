using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreExtensionReminderEmailRepsonse </summary>
    [XmlRoot("IWantToCreateAndStoreExtensionReminderEmailRepsonse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreExtensionReminderEmailRepsonse : MsmqMessage<IWantToCreateAndStoreExtensionReminderEmailRepsonse>
    {
        public Guid FileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
