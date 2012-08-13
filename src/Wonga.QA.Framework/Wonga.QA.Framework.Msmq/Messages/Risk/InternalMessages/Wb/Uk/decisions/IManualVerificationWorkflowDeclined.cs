using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.decisions
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.decisions.IManualVerificationWorkflowDeclined </summary>
    [XmlRoot("IManualVerificationWorkflowDeclined", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.decisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowDeclined,Wonga.Risk.WorkflowDecisions.IWorkflowDecision" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IManualVerificationWorkflowDeclined : MsmqMessage<IManualVerificationWorkflowDeclined>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
