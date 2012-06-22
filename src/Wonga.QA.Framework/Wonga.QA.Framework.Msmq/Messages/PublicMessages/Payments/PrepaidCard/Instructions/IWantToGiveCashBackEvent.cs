using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToGiveCashBack </summary>
    [XmlRoot("IWantToGiveCashBack", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToGiveCashBackEvent : MsmqMessage<IWantToGiveCashBackEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal Amount { get; set; }
        public Guid PrepaidCardId { get; set; }
        public Guid SagaId { get; set; }
    }
}
