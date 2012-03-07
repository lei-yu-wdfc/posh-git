using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.SetPaymentCardPrimary </summary>
    [XmlRoot("SetPaymentCardPrimary", Namespace = "Wonga.Payments", DataType = "")]
    public partial class SetPaymentCardPrimaryCommand : MsmqMessage<SetPaymentCardPrimaryCommand>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
