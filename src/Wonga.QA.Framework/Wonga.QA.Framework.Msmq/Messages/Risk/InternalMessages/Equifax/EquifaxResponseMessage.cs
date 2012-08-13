using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Equifax
{
    /// <summary> Wonga.Risk.InternalMessages.Equifax.EquifaxResponseMessage </summary>
    [XmlRoot("EquifaxResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Equifax", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class EquifaxResponseMessage : MsmqMessage<EquifaxResponseMessage>
    {
        public Guid AccountId { get; set; }
        public Object Response { get; set; }
        public Object CreditReport { get; set; }
        public Object ErrorReport { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
