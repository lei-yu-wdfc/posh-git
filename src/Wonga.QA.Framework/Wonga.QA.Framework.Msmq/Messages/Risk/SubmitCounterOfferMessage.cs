using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SubmitCounterOfferMessage </summary>
    [XmlRoot("SubmitCounterOfferMessage", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SubmitCounterOfferMessage : MsmqMessage<SubmitCounterOfferMessage>
    {
        public Decimal NewLoanAmount { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
