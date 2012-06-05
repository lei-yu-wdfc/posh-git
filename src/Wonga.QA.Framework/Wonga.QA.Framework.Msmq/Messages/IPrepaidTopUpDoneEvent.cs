using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.IEventPrepaidTopUpDone </summary>
    [XmlRoot("IEventPrepaidTopUpDone", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "Wonga.PublicMessages.PrepaidCard.IEventPrepaidTopUp")]
    public partial class IPrepaidTopUpDoneEvent : MsmqMessage<IPrepaidTopUpDoneEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime TransactionTimeStamp { get; set; }
    }
}
