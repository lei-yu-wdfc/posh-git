using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.SetPaymentCardPrimary </summary>
    [XmlRoot("SetPaymentCardPrimary", Namespace = "Wonga.Payments", DataType = "")]
    public partial class SetPaymentCardPrimary : MsmqMessage<SetPaymentCardPrimary>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
