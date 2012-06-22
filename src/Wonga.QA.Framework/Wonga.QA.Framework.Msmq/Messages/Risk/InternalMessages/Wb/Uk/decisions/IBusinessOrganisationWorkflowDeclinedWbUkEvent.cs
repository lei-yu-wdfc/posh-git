using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.decisions
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.decisions.IBusinessOrganisationWorkflowDeclined </summary>
    [XmlRoot("IBusinessOrganisationWorkflowDeclined", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.decisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowDeclined,Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IBusinessOrganisationWorkflowDeclinedWbUkEvent : MsmqMessage<IBusinessOrganisationWorkflowDeclinedWbUkEvent>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
