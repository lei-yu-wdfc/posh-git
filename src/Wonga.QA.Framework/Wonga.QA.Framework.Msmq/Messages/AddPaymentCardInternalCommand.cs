using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AddPaymentCardInternal </summary>
    [XmlRoot("AddPaymentCardInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class AddPaymentCardInternalCommand : MsmqMessage<AddPaymentCardInternalCommand>
    {
        public Int32 PaymentCardId { get; set; }
        public Boolean IsPrimary { get; set; }
        public Object CardNumber { get; set; }
        public Object SecurityCode { get; set; }
    }
}
