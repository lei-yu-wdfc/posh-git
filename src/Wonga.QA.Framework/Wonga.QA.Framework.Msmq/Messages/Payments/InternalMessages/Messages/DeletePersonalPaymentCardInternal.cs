using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.DeletePersonalPaymentCardInternal </summary>
    [XmlRoot("DeletePersonalPaymentCardInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class DeletePersonalPaymentCardInternal : MsmqMessage<DeletePersonalPaymentCardInternal>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
