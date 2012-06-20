using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.ColdStorage
{
    /// <summary> Wonga.PublicMessages.Payments.ColdStorage.ISecureCardDetailsAdded </summary>
    [XmlRoot("ISecureCardDetailsAdded", Namespace = "Wonga.PublicMessages.Payments.ColdStorage", DataType = "")]
    public partial class ISecureCardDetailsAddedEvent : MsmqMessage<ISecureCardDetailsAddedEvent>
    {
        public Guid PaymentCardId { get; set; }
    }
}
