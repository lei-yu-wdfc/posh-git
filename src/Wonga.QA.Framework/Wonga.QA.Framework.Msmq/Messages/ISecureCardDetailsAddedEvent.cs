using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.ColdStorage.PublicMessages.ISecureCardDetailsAdded </summary>
    [XmlRoot("ISecureCardDetailsAdded", Namespace = "Wonga.ColdStorage.PublicMessages", DataType = "")]
    public partial class ISecureCardDetailsAddedEvent : MsmqMessage<ISecureCardDetailsAddedEvent>
    {
        public Guid PaymentCardId { get; set; }
    }
}
