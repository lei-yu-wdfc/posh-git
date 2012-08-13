using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendDeclineEmail </summary>
    [XmlRoot("IWantToSendDeclineEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendDeclineEmail : MsmqMessage<IWantToSendDeclineEmail>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid MessageFileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
        public Boolean ManualVerificationWasRequired { get; set; }
    }
}
