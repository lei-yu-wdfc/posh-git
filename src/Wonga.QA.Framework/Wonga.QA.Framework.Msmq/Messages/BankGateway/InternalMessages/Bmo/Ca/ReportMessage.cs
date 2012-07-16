using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Bmo.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bmo.Ca.ReportMessage </summary>
    [XmlRoot("ReportMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bmo.Ca", DataType = "")]
    public partial class ReportMessage : MsmqMessage<ReportMessage>
    {
        public String FileName { get; set; }
        public Byte[] FileContents { get; set; }
    }
}
