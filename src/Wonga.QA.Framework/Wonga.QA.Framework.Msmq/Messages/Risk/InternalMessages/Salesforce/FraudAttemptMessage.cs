using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Salesforce
{
    /// <summary> Wonga.Risk.InternalMessages.Salesforce.FraudAttemptMessage </summary>
    [XmlRoot("FraudAttemptMessage", Namespace = "Wonga.Risk.InternalMessages.Salesforce", DataType = "Wonga.Risk.InternalMessages.Salesforce.NeedManualVerificationMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Salesforce, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class FraudAttemptMessage : MsmqMessage<FraudAttemptMessage>
    {
        public Int32 CutOff { get; set; }
        public Int32 FraudScore { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String Description { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
