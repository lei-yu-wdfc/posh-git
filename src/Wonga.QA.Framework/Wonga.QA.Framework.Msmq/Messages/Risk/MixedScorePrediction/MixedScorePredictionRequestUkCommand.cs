using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk.MixedScorePrediction
{
    /// <summary> Wonga.Risk.MixedScorePrediction.MixedScorePredictionRequestMessage </summary>
    [XmlRoot("MixedScorePredictionRequestMessage", Namespace = "Wonga.Risk.MixedScorePrediction", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class MixedScorePredictionRequestUkCommand : MsmqMessage<MixedScorePredictionRequestUkCommand>
    {
        public Guid PaymentCardID { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
