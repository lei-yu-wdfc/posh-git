using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Hyphen.Za
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.LoadReportDetailRecordMessage </summary>
    [XmlRoot("LoadReportDetailRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za", DataType = "")]
    public partial class LoadReportDetailRecordMessage : MsmqMessage<LoadReportDetailRecordMessage>
    {
        public String TransactionId { get; set; }
        public String Error { get; set; }
        public Int32? AcknowledgeTypeId { get; set; }
        public String FileName { get; set; }
        public String RawContents { get; set; }
        public Int32 BatchNumber { get; set; }
    }
}
