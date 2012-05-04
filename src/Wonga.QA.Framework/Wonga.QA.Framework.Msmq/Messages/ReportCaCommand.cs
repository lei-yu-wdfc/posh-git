using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bmo.Ca.ReportMessage </summary>
    [XmlRoot("ReportMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bmo.Ca", DataType = "")]
    public partial class ReportCaCommand : MsmqMessage<ReportCaCommand>
    {
        public String FileName { get; set; }
        public Byte[] FileContents { get; set; }
    }
}
