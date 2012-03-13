using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.ExperianBulk.InternalMessages.SendExperianBulkFileMessage </summary>
    [XmlRoot("SendExperianBulkFileMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "Wonga.ExperianBulk.InternalMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendExperianBulkFileCommand : MsmqMessage<SendExperianBulkFileCommand>
    {
        public String CreatedFilePath { get; set; }
        public Int32 CustomerCount { get; set; }
        public Guid SagaId { get; set; }
    }
}
