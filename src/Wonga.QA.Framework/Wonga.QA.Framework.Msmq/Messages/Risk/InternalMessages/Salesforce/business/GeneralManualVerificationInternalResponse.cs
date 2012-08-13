using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Salesforce.business
{
    /// <summary> Wonga.Risk.InternalMessages.Salesforce.business.GeneralManualVerificationInternalResponse </summary>
    [XmlRoot("GeneralManualVerificationInternalResponse", Namespace = "Wonga.Risk.InternalMessages.Salesforce.business", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Salesforce, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class GeneralManualVerificationInternalResponse : MsmqMessage<GeneralManualVerificationInternalResponse>
    {
        public Guid ApplicationId { get; set; }
        public Boolean IsAccepted { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
