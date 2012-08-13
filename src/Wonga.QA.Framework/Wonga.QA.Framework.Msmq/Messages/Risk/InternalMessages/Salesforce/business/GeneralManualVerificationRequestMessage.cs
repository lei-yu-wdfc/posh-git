using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Salesforce.business
{
    /// <summary> Wonga.Risk.InternalMessages.Salesforce.business.GeneralManualVerificationRequestMessage </summary>
    [XmlRoot("GeneralManualVerificationRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Salesforce.business", DataType = "Wonga.Risk.InternalMessages.Salesforce.NeedManualVerificationMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Salesforce, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class GeneralManualVerificationRequestMessage : MsmqMessage<GeneralManualVerificationRequestMessage>
    {
        public Guid OrganisationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String Description { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
