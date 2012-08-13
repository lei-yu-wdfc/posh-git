using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.LogCardPaymentMessageBase </summary>
    [XmlRoot("LogCardPaymentMessageBase", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class LogCardPaymentMessageBase : MsmqMessage<LogCardPaymentMessageBase>
    {
        public Guid LogEntryExternalId { get; set; }
        public Int32 ApplicationId { get; set; }
        public Int32 PaymentCardId { get; set; }
    }
}
