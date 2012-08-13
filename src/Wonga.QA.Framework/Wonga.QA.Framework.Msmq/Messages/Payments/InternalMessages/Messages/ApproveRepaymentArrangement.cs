using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ApproveRepaymentArrangement </summary>
    [XmlRoot("ApproveRepaymentArrangement", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ApproveRepaymentArrangement : MsmqMessage<ApproveRepaymentArrangement>
    {
        public Int32 RepaymentArrangementId { get; set; }
    }
}
