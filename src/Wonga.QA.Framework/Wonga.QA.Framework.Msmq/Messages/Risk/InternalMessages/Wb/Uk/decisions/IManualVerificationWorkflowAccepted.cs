using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.decisions
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.decisions.IManualVerificationWorkflowAccepted </summary>
    [XmlRoot("IManualVerificationWorkflowAccepted", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.decisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowAccepted,Wonga.Risk.WorkflowDecisions.IWorkflowDecision" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IManualVerificationWorkflowAccepted : MsmqMessage<IManualVerificationWorkflowAccepted>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
