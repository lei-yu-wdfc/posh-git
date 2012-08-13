using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IDmpRepaymentArrangementRemoved </summary>
    [XmlRoot("IDmpRepaymentArrangementRemoved", Namespace = "Wonga.Payments.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Payments.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IDmpRepaymentArrangementRemoved : MsmqMessage<IDmpRepaymentArrangementRemoved>
    {
        public Guid ApplicationId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
        public Guid AccountId { get; set; }
    }
}
