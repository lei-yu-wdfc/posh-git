using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IRepaymentArrangementCreated </summary>
    [XmlRoot("IRepaymentArrangementCreated", Namespace = "Wonga.Payments.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Payments.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRepaymentArrangementCreated : MsmqMessage<IRepaymentArrangementCreated>
    {
        public Guid ApplicationId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
        public Guid AccountId { get; set; }
    }
}
