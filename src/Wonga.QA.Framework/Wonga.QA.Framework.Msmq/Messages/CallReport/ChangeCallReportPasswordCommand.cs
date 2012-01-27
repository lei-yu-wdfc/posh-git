using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.CallReport
{
    [XmlRoot("ChangeCallReportPasswordMessage", Namespace = "Wonga.CallReport.InternalMessages", DataType = "")]
    public class ChangeCallReportPasswordCommand : MsmqMessage<ChangeCallReportPasswordCommand>
    {
    }
}
