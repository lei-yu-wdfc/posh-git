using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IRiskPaymentCardAdded </summary>
    [XmlRoot("IRiskPaymentCardAdded", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IRiskPaymentCardAdded : MsmqMessage<IRiskPaymentCardAdded>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
