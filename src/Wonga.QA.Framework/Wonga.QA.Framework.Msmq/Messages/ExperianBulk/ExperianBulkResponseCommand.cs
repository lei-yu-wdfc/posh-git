using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ExperianBulk
{
    [XmlRoot("ExperianBulkResponseMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "")]
    public class ExperianBulkResponseCommand : MsmqMessage<ExperianBulkResponseCommand>
    {
        public Boolean ProcessingCompleted { get; set; }
    }
}
