using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.ExperianBulk.InternalMessages
{
    /// <summary> Wonga.ExperianBulk.InternalMessages.ExperianBulkResponseMessage </summary>
    [XmlRoot("ExperianBulkResponseMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "")]
    public partial class ExperianBulkResponseCommand : MsmqMessage<ExperianBulkResponseCommand>
    {
        public Boolean ProcessingCompleted { get; set; }
    }
}
