using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Easypay.Za
{
    /// <summary> Wonga.BankGateway.InternalMessages.Easypay.Za.PaymentResponseDetailRecordMessage </summary>
    [XmlRoot("PaymentResponseDetailRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Easypay.Za", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Easypay.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class PaymentResponseDetailRecordMessage : MsmqMessage<PaymentResponseDetailRecordMessage>
    {
        public String RepaymentNumber { get; set; }
        public Decimal Amount { get; set; }
        public DateTime ActionDate { get; set; }
        public String RawContent { get; set; }
        public String Collector { get; set; }
        public String Filename { get; set; }
    }
}
