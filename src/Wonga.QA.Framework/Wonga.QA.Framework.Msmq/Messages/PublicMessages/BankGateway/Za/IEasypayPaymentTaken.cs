using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.BankGateway.Za
{
    /// <summary> Wonga.PublicMessages.BankGateway.Za.IEasypayPaymentTaken </summary>
    [XmlRoot("IEasypayPaymentTaken", Namespace = "Wonga.PublicMessages.BankGateway.Za", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.BankGateway.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IEasypayPaymentTaken : MsmqMessage<IEasypayPaymentTaken>
    {
        public DateTime CreatedOn { get; set; }
        public String RepaymentNumber { get; set; }
        public Decimal TransactionAmount { get; set; }
        public DateTime ValueDate { get; set; }
        public Int32 AcknowledgeId { get; set; }
    }
}
