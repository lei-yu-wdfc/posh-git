using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Decisions
{
    /// <summary> Wonga.Risk.Decisions.IMainApplicantWorkflowDeclined </summary>
    [XmlRoot("IMainApplicantWorkflowDeclined", Namespace = "Wonga.Risk.Decisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowDeclined,Wonga.Risk.WorkflowDecisions.IWorkflowDecision" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IMainApplicantWorkflowDeclined : MsmqMessage<IMainApplicantWorkflowDeclined>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
