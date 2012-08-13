using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.ExperianBulk.InternalMessages
{
    /// <summary> Wonga.ExperianBulk.InternalMessages.SendExperianBulkFileMessage </summary>
    [XmlRoot("SendExperianBulkFileMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "Wonga.ExperianBulk.InternalMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.ExperianBulk.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendExperianBulkFileMessage : MsmqMessage<SendExperianBulkFileMessage>
    {
        public String CreatedFilePath { get; set; }
        public Int32 CustomerCount { get; set; }
        public Guid SagaId { get; set; }
    }
}
