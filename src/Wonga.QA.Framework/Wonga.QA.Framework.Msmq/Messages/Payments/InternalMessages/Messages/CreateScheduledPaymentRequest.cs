using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CreateScheduledPaymentRequest </summary>
    [XmlRoot("CreateScheduledPaymentRequest", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CreateScheduledPaymentRequest : MsmqMessage<CreateScheduledPaymentRequest>
    {
        public Guid RepaymentRequestId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
