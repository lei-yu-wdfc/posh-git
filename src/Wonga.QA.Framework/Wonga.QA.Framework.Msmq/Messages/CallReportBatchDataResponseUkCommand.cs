using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.CallReport.CallReportBatchDataResponseMessage </summary>
    [XmlRoot("CallReportBatchDataResponseMessage", Namespace = "Wonga.Risk.CallReport", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class CallReportBatchDataResponseUkCommand : MsmqMessage<CallReportBatchDataResponseUkCommand>
    {
        public Object Response { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
