using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Uk
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Uk.CreateAndStoreDueDateReminderDocumentMessageCompleted </summary>
    [XmlRoot("CreateAndStoreDueDateReminderDocumentMessageCompleted", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Uk", DataType = "")]
    public partial class CreateAndStoreDueDateReminderDocumentCompletedUkCommand : MsmqMessage<CreateAndStoreDueDateReminderDocumentCompletedUkCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FileId { get; set; }
    }
}
