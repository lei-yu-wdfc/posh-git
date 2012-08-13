using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Transunion
{
    /// <summary> Wonga.Risk.InternalMessages.Transunion.TransunionResponseMessage </summary>
    [XmlRoot("TransunionResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Transunion", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class TransunionResponseMessage : MsmqMessage<TransunionResponseMessage>
    {
        public Guid AccountId { get; set; }
        public Object Response { get; set; }
        public String InputForename { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
