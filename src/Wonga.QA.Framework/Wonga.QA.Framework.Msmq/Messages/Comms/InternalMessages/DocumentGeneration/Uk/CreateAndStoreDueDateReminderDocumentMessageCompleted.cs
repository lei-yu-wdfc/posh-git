using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Uk
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Uk.CreateAndStoreDueDateReminderDocumentMessageCompleted </summary>
    [XmlRoot("CreateAndStoreDueDateReminderDocumentMessageCompleted", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Uk", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.DocumentGeneration.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateAndStoreDueDateReminderDocumentMessageCompleted : MsmqMessage<CreateAndStoreDueDateReminderDocumentMessageCompleted>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FileId { get; set; }
    }
}
