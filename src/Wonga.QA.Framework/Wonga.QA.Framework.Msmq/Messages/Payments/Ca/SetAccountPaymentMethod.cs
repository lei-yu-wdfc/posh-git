using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Ca
{
    /// <summary> Wonga.Payments.Ca.SetAccountPaymentMethod </summary>
    [XmlRoot("SetAccountPaymentMethod", Namespace = "Wonga.Payments.Ca", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SetAccountPaymentMethod : MsmqMessage<SetAccountPaymentMethod>
    {
        public Guid AccountId { get; set; }
        public PaymentMethodEnum CashoutPaymentMethod { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
