using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("LoadReportDetailRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za", DataType = "")]
    public class LoadReportDetailRecordZaCommand : MsmqMessage<LoadReportDetailRecordZaCommand>
    {
        public String TransactionId { get; set; }
        public String Error { get; set; }
        public Int32? AcknowledgeTypeId { get; set; }
        public String FileName { get; set; }
        public String RawContents { get; set; }
        public Int32 BatchNumber { get; set; }
    }
}
