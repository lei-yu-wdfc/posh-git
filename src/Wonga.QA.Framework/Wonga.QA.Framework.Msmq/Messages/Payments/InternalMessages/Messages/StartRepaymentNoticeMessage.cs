using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.StartRepaymentNoticeMessage </summary>
    [XmlRoot("StartRepaymentNoticeMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class StartRepaymentNoticeMessage : MsmqMessage<StartRepaymentNoticeMessage>
    {
        public Int32 RepaymentArrangementId { get; set; }
        public Int32 RepaymentDetailId { get; set; }
    }
}
