using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("AddColdStoragePaymentCardMessage", Namespace = "Wonga.Payments.InternalMessages.ColdStorage", DataType = "")]
    public partial class AddColdStoragePaymentCardCommand : MsmqMessage<AddColdStoragePaymentCardCommand>
    {
        public Guid ExternalId { get; set; }
        public Object CardNumber { get; set; }
        public Object CV2 { get; set; }
    }
}
