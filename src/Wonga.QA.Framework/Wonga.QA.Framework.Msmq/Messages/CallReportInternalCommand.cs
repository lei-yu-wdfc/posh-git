using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.CallReport.Handlers.InternalMessages.CallReportInternalMessage </summary>
    [XmlRoot("CallReportInternalMessage", Namespace = "Wonga.CallReport.Handlers.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class CallReportInternalCommand : MsmqMessage<CallReportInternalCommand>
    {
        public Guid SagaId { get; set; }
        public Guid AccountId { get; set; }
        public Object Request { get; set; }
    }
}
