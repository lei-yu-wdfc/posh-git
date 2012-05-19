using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreDueDateReminderDocument </summary>
    [XmlRoot("IWantToCreateAndStoreDueDateReminderDocument", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreDueDateReminderDocumentEvent : MsmqMessage<IWantToCreateAndStoreDueDateReminderDocumentEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
