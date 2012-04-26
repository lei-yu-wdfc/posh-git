using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Ca.SetAccountPaymentMethod </summary>
    [XmlRoot("SetAccountPaymentMethod", Namespace = "Wonga.Payments.Ca", DataType = "")]
    public partial class SetAccountPaymentMethodCaCommand : MsmqMessage<SetAccountPaymentMethodCaCommand>
    {
        public Guid AccountId { get; set; }
        public PaymentMethodEnum CashoutPaymentMethod { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
