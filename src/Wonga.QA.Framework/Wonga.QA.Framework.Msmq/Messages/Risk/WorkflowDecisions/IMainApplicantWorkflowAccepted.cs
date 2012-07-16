using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.WorkflowDecisions
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IMainApplicantWorkflowAccepted </summary>
    [XmlRoot("IMainApplicantWorkflowAccepted", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowAccepted,Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IMainApplicantWorkflowAccepted : MsmqMessage<IMainApplicantWorkflowAccepted>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
