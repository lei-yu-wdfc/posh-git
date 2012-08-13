using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.MixedScorePrediction
{
    /// <summary> Wonga.Risk.MixedScorePrediction.MixedScorePredictionResponseMessage </summary>
    [XmlRoot("MixedScorePredictionResponseMessage", Namespace = "Wonga.Risk.MixedScorePrediction", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class MixedScorePredictionResponseMessage : MsmqMessage<MixedScorePredictionResponseMessage>
    {
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
