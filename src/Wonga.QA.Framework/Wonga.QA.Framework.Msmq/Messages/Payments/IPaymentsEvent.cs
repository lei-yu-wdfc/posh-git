using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("IPaymentsEvent", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public class IPaymentsEvent : MsmqMessage<IPaymentsEvent>
    {
        public DateTime CreatedOn { get; set; }
    }
}
