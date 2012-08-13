using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.WorkflowDecisions
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IWorkflowAccepted </summary>
    [XmlRoot("IWorkflowAccepted", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowDecision" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWorkflowAccepted : MsmqMessage<IWorkflowAccepted>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
