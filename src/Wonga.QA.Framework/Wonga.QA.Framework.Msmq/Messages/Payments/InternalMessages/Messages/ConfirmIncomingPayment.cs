using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ConfirmIncomingPayment </summary>
    [XmlRoot("ConfirmIncomingPayment", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ConfirmIncomingPayment : MsmqMessage<ConfirmIncomingPayment>
    {
        public Int32 PaymentId { get; set; }
    }
}
