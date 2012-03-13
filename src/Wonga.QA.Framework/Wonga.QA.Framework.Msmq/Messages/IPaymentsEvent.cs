using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.IPaymentsEvent </summary>
    [XmlRoot("IPaymentsEvent", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IPaymentsEvent : MsmqMessage<IPaymentsEvent>
    {
        public DateTime CreatedOn { get; set; }
    }
}
