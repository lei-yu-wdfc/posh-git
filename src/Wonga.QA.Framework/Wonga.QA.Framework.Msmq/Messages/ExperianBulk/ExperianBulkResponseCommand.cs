using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ExperianBulk
{
    /// <summary> Wonga.ExperianBulk.InternalMessages.ExperianBulkResponseMessage </summary>
    [XmlRoot("ExperianBulkResponseMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "")]
    public partial class ExperianBulkResponseCommand : MsmqMessage<ExperianBulkResponseCommand>
    {
        public Boolean ProcessingCompleted { get; set; }
    }
}
