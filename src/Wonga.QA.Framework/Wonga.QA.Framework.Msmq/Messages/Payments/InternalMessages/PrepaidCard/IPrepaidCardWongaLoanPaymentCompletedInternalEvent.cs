using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.PrepaidCard
{
    /// <summary> Wonga.Payments.InternalMessages.PrepaidCard.IPrepaidCardWongaLoanPaymentCompletedInternalEvent </summary>
    [XmlRoot("IPrepaidCardWongaLoanPaymentCompletedInternalEvent", Namespace = "Wonga.Payments.InternalMessages.PrepaidCard", DataType = "")]
    public partial class IPrepaidCardWongaLoanPaymentCompletedInternalEvent : MsmqMessage<IPrepaidCardWongaLoanPaymentCompletedInternalEvent>
    {
        public Guid SagaId { get; set; }
        public DateTime? SucceededOn { get; set; }
        public DateTime? FailedOn { get; set; }
        public String PpsLocalTimeStamp { get; set; }
        public Guid? TransactionId { get; set; }
        public String AdditionalInformation { get; set; }
    }
}
