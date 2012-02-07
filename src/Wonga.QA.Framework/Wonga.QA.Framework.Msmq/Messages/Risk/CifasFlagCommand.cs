using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("CIFASFlagMessage", Namespace = "Wonga.Risk.InternalMessages.Salesforce", DataType = "Wonga.Risk.InternalMessages.Salesforce.NeedManualVerificationMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class CifasFlagCommand : MsmqMessage<CifasFlagCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String Description { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
