using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Uk
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Uk.CreateAndStoreDueDateReminderDocumentMessage </summary>
    [XmlRoot("CreateAndStoreDueDateReminderDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Uk", DataType = "")]
    public partial class CreateAndStoreDueDateReminderDocumentUkCommand : MsmqMessage<CreateAndStoreDueDateReminderDocumentUkCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
