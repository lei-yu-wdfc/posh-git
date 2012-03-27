using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.InternalMessages.Salesforce.business.GeneralManualVerificationRequestMessage </summary>
    [XmlRoot("GeneralManualVerificationRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Salesforce.business", DataType = "Wonga.Risk.InternalMessages.Salesforce.NeedManualVerificationMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class GeneralManualVerificationRequestCommand : MsmqMessage<GeneralManualVerificationRequestCommand>
    {
        public Guid OrganisationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String Description { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
