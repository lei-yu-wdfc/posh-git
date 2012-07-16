using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.WorkflowDecisions
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IGuarantorWorkflowAccepted </summary>
    [XmlRoot("IGuarantorWorkflowAccepted", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowAccepted,Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IGuarantorWorkflowAccepted : MsmqMessage<IGuarantorWorkflowAccepted>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
