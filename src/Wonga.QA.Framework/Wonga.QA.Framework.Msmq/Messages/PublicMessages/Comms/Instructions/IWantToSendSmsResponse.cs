using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendSmsResponse </summary>
    [XmlRoot("IWantToSendSmsResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendSmsResponse : MsmqMessage<IWantToSendSmsResponse>
    {
        public Guid SmsId { get; set; }
        public Boolean Successful { get; set; }
        public Guid SagaId { get; set; }
    }
}
