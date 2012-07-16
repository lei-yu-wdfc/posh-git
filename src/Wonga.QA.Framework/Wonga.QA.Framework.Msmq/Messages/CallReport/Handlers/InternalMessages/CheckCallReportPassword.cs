using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.CallReport.Handlers.InternalMessages
{
    /// <summary> Wonga.CallReport.Handlers.InternalMessages.CheckCallReportPassword </summary>
    [XmlRoot("CheckCallReportPassword", Namespace = "Wonga.CallReport.Handlers.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class CheckCallReportPassword : MsmqMessage<CheckCallReportPassword>
    {
        public Guid SagaId { get; set; }
    }
}
