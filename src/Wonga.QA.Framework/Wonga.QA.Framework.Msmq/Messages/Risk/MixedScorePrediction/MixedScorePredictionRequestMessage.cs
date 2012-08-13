using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.MixedScorePrediction
{
    /// <summary> Wonga.Risk.MixedScorePrediction.MixedScorePredictionRequestMessage </summary>
    [XmlRoot("MixedScorePredictionRequestMessage", Namespace = "Wonga.Risk.MixedScorePrediction", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class MixedScorePredictionRequestMessage : MsmqMessage<MixedScorePredictionRequestMessage>
    {
        public Guid PaymentCardID { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
