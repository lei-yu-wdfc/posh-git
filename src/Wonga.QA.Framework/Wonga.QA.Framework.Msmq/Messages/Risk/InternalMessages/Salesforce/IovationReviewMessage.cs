using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Salesforce
{
    /// <summary> Wonga.Risk.InternalMessages.Salesforce.IovationReviewMessage </summary>
    [XmlRoot("IovationReviewMessage", Namespace = "Wonga.Risk.InternalMessages.Salesforce", DataType = "Wonga.Risk.InternalMessages.Salesforce.NeedManualVerificationMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class IovationReviewMessage : MsmqMessage<IovationReviewMessage>
    {
        public String Reason { get; set; }
        public String TrackingNumber { get; set; }
        public Object Details { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String Description { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
