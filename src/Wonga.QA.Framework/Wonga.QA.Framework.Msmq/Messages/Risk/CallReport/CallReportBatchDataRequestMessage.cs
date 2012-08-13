using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.CallReport
{
    /// <summary> Wonga.Risk.CallReport.CallReportBatchDataRequestMessage </summary>
    [XmlRoot("CallReportBatchDataRequestMessage", Namespace = "Wonga.Risk.CallReport", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CallReportBatchDataRequestMessage : MsmqMessage<CallReportBatchDataRequestMessage>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
