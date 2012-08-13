using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.Za
{
    /// <summary> Wonga.PublicMessages.Payments.Za.IWantToVerifyPayUTransaction </summary>
    [XmlRoot("IWantToVerifyPayUTransaction", Namespace = "Wonga.PublicMessages.Payments.Za", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToVerifyPayUTransaction : MsmqMessage<IWantToVerifyPayUTransaction>
    {
        public Guid SafeKey { get; set; }
        public Int32 PaymentId { get; set; }
        public String PaymentReferenceNumber { get; set; }
    }
}
