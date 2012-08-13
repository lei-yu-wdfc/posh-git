using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AddPaymentCardInternal </summary>
    [XmlRoot("AddPaymentCardInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AddPaymentCardInternal : MsmqMessage<AddPaymentCardInternal>
    {
        public Int32 PaymentCardId { get; set; }
        public Boolean IsPrimary { get; set; }
        public Object CardNumber { get; set; }
        public Object SecurityCode { get; set; }
    }
}
