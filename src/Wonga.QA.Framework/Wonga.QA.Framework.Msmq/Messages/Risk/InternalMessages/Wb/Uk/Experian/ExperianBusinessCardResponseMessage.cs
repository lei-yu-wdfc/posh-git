using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.Experian
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Experian.ExperianBusinessCardResponseMessage </summary>
    [XmlRoot("ExperianBusinessCardResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Experian", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ExperianBusinessCardResponseMessage : MsmqMessage<ExperianBusinessCardResponseMessage>
    {
        public Object Response { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
