using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.Za.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.Za
{
    /// <summary> Wonga.PublicMessages.Payments.Za.IWantToVerifyPayUTransactionResponse </summary>
    [XmlRoot("IWantToVerifyPayUTransactionResponse", Namespace = "Wonga.PublicMessages.Payments.Za", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToVerifyPayUTransactionResponse : MsmqMessage<IWantToVerifyPayUTransactionResponse>
    {
        public Int32 PaymentId { get; set; }
        public String PaymentReferenceNumber { get; set; }
        public PayUTransactionResultEnum Result { get; set; }
        public DateTime DateProcessed { get; set; }
        public String RawResponse { get; set; }
    }
}
