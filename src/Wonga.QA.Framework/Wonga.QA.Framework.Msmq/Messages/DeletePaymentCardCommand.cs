using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.DeletePaymentCard </summary>
    [XmlRoot("DeletePaymentCard", Namespace = "Wonga.Payments", DataType = "")]
    public partial class DeletePaymentCardCommand : MsmqMessage<DeletePaymentCardCommand>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}