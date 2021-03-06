using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ProcessDebitCardPaymentMessage </summary>
    [XmlRoot("ProcessDebitCardPaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ProcessDebitCardPaymentMessage : MsmqMessage<ProcessDebitCardPaymentMessage>
    {
        public Guid ApplicationId { get; set; }
        public Decimal CollectAmount { get; set; }
        public Guid SenderReference { get; set; }
        public Guid PaymentCardId { get; set; }
        public Guid PaymentExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
        public RepaymentRequestEnum RepaymentRequestType { get; set; }
    }
}
