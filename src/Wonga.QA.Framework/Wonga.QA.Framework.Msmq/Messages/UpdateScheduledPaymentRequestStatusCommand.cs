using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.UpdateScheduledPaymentRequestStatus </summary>
    [XmlRoot("UpdateScheduledPaymentRequestStatus", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class UpdateScheduledPaymentRequestStatusCommand : MsmqMessage<UpdateScheduledPaymentRequestStatusCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid RepaymentRequestId { get; set; }
        public RepaymentRequestStatusEnum StatusCode { get; set; }
        public DateTime ActionDate { get; set; }
        public Decimal Amount { get; set; }
        public String StatusMessage { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
