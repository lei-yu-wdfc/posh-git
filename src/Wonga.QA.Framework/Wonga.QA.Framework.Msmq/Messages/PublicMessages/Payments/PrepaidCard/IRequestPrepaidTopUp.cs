using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.IRequestPrepaidTopUp </summary>
    [XmlRoot("IRequestPrepaidTopUp", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.PrepaidCard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRequestPrepaidTopUp : MsmqMessage<IRequestPrepaidTopUp>
    {
        public Guid CustomerExternalId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
