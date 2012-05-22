using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreDueDateReminderDocumentResponse </summary>
    [XmlRoot("IWantToCreateAndStoreDueDateReminderDocumentResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreDueDateReminderDocumentResponseEvent : MsmqMessage<IWantToCreateAndStoreDueDateReminderDocumentResponseEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
