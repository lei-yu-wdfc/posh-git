using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.decisions
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.decisions.IBusinessOrganisationWorkflowAccepted </summary>
    [XmlRoot("IBusinessOrganisationWorkflowAccepted", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.decisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowAccepted,Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IBusinessOrganisationWorkflowAcceptedWbUkEvent : MsmqMessage<IBusinessOrganisationWorkflowAcceptedWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
