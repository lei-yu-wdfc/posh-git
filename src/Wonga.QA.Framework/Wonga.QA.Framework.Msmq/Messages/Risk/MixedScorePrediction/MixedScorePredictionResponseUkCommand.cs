using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk.MixedScorePrediction
{
    /// <summary> Wonga.Risk.MixedScorePrediction.MixedScorePredictionResponseMessage </summary>
    [XmlRoot("MixedScorePredictionResponseMessage", Namespace = "Wonga.Risk.MixedScorePrediction", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class MixedScorePredictionResponseUkCommand : MsmqMessage<MixedScorePredictionResponseUkCommand>
    {
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
