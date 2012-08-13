using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendPasswordChangedNotificationEmail </summary>
    [XmlRoot("IWantToSendPasswordChangedNotificationEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendPasswordChangedNotificationEmail : MsmqMessage<IWantToSendPasswordChangedNotificationEmail>
    {
        public Guid AccountId { get; set; }
        public Guid MessageFileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
