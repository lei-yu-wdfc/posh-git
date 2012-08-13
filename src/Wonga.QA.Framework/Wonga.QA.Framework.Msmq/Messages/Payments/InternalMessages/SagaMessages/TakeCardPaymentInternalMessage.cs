using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.TakeCardPaymentInternalMessage </summary>
    [XmlRoot("TakeCardPaymentInternalMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class TakeCardPaymentInternalMessage : MsmqMessage<TakeCardPaymentInternalMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
        public RepaymentRequestEnum RepaymentRequestType { get; set; }
        public Guid? PaymentExternalId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
