using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.IManualVerificationWorkflowAccepted </summary>
    [XmlRoot("IManualVerificationWorkflowAccepted", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowAccepted,Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IManualVerificationWorkflowAcceptedWbUkEvent : MsmqMessage<IManualVerificationWorkflowAcceptedWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
