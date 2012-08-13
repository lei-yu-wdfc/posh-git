using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.ExperianBulk.InternalMessages
{
    /// <summary> Wonga.ExperianBulk.InternalMessages.ExperianBulkParseDownloadedOutputFile </summary>
    [XmlRoot("ExperianBulkParseDownloadedOutputFile", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "Wonga.ExperianBulk.InternalMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.ExperianBulk.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ExperianBulkParseDownloadedOutputFile : MsmqMessage<ExperianBulkParseDownloadedOutputFile>
    {
        public Guid SagaId { get; set; }
        public String DownloadOutputFilePath { get; set; }
    }
}
