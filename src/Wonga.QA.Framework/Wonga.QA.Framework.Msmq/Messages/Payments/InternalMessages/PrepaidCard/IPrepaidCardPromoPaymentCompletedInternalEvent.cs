using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.PrepaidCard
{
    /// <summary> Wonga.Payments.InternalMessages.PrepaidCard.IPrepaidCardPromoPaymentCompletedInternalEvent </summary>
    [XmlRoot("IPrepaidCardPromoPaymentCompletedInternalEvent", Namespace = "Wonga.Payments.InternalMessages.PrepaidCard", DataType = "")]
    public partial class IPrepaidCardPromoPaymentCompletedInternalEvent : MsmqMessage<IPrepaidCardPromoPaymentCompletedInternalEvent>
    {
        public Guid SagaId { get; set; }
        public DateTime? SucceededOn { get; set; }
        public DateTime? FailedOn { get; set; }
        public DateTime? TransactionTimeStamp { get; set; }
        public DateTime? ProviderTransactionTimeStamp { get; set; }
        public Guid? TransactionId { get; set; }
        public String AdditionalInformation { get; set; }
        public String ErrorMessage { get; set; }
    }
}
