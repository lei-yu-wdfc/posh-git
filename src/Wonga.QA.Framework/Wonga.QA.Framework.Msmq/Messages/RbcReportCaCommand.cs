using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Rbc.Ca.RbcReportMessage </summary>
    [XmlRoot("RbcReportMessage", Namespace = "Wonga.BankGateway.InternalMessages.Rbc.Ca", DataType = "")]
    public partial class RbcReportCaCommand : MsmqMessage<RbcReportCaCommand>
    {
        public String FileName { get; set; }
        public Byte[] FileContent { get; set; }
    }
}
