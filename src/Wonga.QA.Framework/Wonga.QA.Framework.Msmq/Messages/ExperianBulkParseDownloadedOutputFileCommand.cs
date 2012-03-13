using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.ExperianBulk.InternalMessages.ExperianBulkParseDownloadedOutputFile </summary>
    [XmlRoot("ExperianBulkParseDownloadedOutputFile", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "Wonga.ExperianBulk.InternalMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class ExperianBulkParseDownloadedOutputFileCommand : MsmqMessage<ExperianBulkParseDownloadedOutputFileCommand>
    {
        public Guid SagaId { get; set; }
        public String DownloadOutputFilePath { get; set; }
    }
}
