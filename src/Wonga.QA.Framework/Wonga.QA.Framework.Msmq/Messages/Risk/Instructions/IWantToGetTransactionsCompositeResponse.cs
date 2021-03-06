using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Instructions
{
    /// <summary> Wonga.Risk.Instructions.IWantToGetTransactionsCompositeResponse </summary>
    [XmlRoot("IWantToGetTransactionsCompositeResponse", Namespace = "Wonga.Risk.Instructions", DataType = "" )
    , SourceAssembly("Wonga.Risk.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToGetTransactionsCompositeResponse : MsmqMessage<IWantToGetTransactionsCompositeResponse>
    {
        public Guid ApplicationId { get; set; }
        public Int32? PaymentTransactionCount { get; set; }
        public Int32? ManualPaymentTransactionCount { get; set; }
        public Double? MainCardRepayRate { get; set; }
    }
}
