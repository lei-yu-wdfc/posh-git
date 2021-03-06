using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.UpdateScheduledPaymentRequestStatus </summary>
    [XmlRoot("UpdateScheduledPaymentRequestStatus", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdateScheduledPaymentRequestStatus : MsmqMessage<UpdateScheduledPaymentRequestStatus>
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
