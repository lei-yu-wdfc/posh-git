using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateExtensionReminderEmailResponse </summary>
    [XmlRoot("CreateExtensionReminderEmailResponse", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.FileStorage.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateExtensionReminderEmailResponse : MsmqMessage<CreateExtensionReminderEmailResponse>
    {
        public String Content { get; set; }
        public Guid SagaId { get; set; }
    }
}
