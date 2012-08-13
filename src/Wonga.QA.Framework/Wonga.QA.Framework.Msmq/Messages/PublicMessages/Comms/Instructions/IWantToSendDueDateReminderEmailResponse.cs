using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendDueDateReminderEmailResponse </summary>
    [XmlRoot("IWantToSendDueDateReminderEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendDueDateReminderEmailResponse : MsmqMessage<IWantToSendDueDateReminderEmailResponse>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FileId { get; set; }
        public Boolean Successful { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
