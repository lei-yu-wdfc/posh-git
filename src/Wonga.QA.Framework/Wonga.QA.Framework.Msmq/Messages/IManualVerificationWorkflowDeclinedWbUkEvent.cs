using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.IManualVerificationWorkflowDeclined </summary>
    [XmlRoot("IManualVerificationWorkflowDeclined", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowDeclined,Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IManualVerificationWorkflowDeclinedWbUkEvent : MsmqMessage<IManualVerificationWorkflowDeclinedWbUkEvent>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
