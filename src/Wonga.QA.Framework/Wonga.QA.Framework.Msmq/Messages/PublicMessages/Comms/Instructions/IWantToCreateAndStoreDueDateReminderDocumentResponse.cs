using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreDueDateReminderDocumentResponse </summary>
    [XmlRoot("IWantToCreateAndStoreDueDateReminderDocumentResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreDueDateReminderDocumentResponse : MsmqMessage<IWantToCreateAndStoreDueDateReminderDocumentResponse>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
