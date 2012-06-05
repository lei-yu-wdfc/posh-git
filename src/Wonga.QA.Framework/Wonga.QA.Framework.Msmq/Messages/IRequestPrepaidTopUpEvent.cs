using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.IRequestPrepaidTopUp </summary>
    [XmlRoot("IRequestPrepaidTopUp", Namespace = "Wonga.PublicMessages.PrepaidCard", DataType = "")]
    public partial class IRequestPrepaidTopUpEvent : MsmqMessage<IRequestPrepaidTopUpEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
