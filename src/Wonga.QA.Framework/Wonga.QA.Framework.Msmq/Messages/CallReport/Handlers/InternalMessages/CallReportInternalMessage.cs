using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.CallReport.Handlers.InternalMessages
{
    /// <summary> Wonga.CallReport.Handlers.InternalMessages.CallReportInternalMessage </summary>
    [XmlRoot("CallReportInternalMessage", Namespace = "Wonga.CallReport.Handlers.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class CallReportInternalMessage : MsmqMessage<CallReportInternalMessage>
    {
        public Guid SagaId { get; set; }
        public Guid AccountId { get; set; }
        public Object Request { get; set; }
    }
}
