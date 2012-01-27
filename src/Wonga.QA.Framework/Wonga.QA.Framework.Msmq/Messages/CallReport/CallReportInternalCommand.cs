using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.CallReport
{
    [XmlRoot("CallReportInternalMessage", Namespace = "Wonga.CallReport.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public class CallReportInternalCommand : MsmqMessage<CallReportInternalCommand>
    {
        public Guid SagaId { get; set; }
        public Guid AccountId { get; set; }
        public Object Request { get; set; }
    }
}
