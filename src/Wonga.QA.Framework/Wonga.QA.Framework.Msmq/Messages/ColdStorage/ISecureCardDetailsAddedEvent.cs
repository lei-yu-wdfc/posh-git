using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ColdStorage
{
    [XmlRoot("ISecureCardDetailsAdded", Namespace = "Wonga.ColdStorage.PublicMessages", DataType = "")]
    public partial class ISecureCardDetailsAddedEvent : MsmqMessage<ISecureCardDetailsAddedEvent>
    {
        public Guid PaymentCardId { get; set; }
    }
}
