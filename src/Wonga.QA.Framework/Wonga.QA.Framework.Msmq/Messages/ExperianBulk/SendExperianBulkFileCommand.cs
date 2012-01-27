using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ExperianBulk
{
    [XmlRoot("SendExperianBulkFileMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "Wonga.ExperianBulk.InternalMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public class SendExperianBulkFileCommand : MsmqMessage<SendExperianBulkFileCommand>
    {
        public String CreatedFilePath { get; set; }
        public Int32 CustomerCount { get; set; }
        public Guid SagaId { get; set; }
    }
}
