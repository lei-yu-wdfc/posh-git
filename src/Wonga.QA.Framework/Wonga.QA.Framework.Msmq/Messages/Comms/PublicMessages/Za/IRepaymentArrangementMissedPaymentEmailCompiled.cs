using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Za
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IRepaymentArrangementMissedPaymentEmailCompiled </summary>
    [XmlRoot("IRepaymentArrangementMissedPaymentEmailCompiled", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "Wonga.Comms.PublicMessages.Za.IEmailCompiled" )
    , SourceAssembly("Wonga.Comms.PublicMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRepaymentArrangementMissedPaymentEmailCompiled : MsmqMessage<IRepaymentArrangementMissedPaymentEmailCompiled>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
    }
}
