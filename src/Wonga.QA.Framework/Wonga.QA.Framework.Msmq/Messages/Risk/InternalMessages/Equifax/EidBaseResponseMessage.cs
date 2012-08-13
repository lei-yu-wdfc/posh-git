using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Equifax
{
    /// <summary> Wonga.Risk.InternalMessages.Equifax.EidBaseResponseMessage </summary>
    [XmlRoot("EidBaseResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Equifax", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class EidBaseResponseMessage : MsmqMessage<EidBaseResponseMessage>
    {
        public String TransactionKey { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
