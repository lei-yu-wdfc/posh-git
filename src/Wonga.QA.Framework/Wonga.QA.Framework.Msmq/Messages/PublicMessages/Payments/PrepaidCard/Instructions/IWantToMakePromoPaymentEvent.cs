using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.PrepaidCard.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToMakePromoPayment </summary>
    [XmlRoot("IWantToMakePromoPayment", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToMakePromoPaymentEvent : MsmqMessage<IWantToMakePromoPaymentEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PrepaidCardId { get; set; }
        public Decimal Amount { get; set; }
        public PromoPaymentEnum PromoPaymentType { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Guid SagaId { get; set; }
    }
}
